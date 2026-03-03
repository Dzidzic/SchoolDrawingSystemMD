using SchoolDrawingSystemMD.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDrawingSystemMD.Services
{
    public class TxtFileServices : IFileService
    {
        private readonly string _studentsFilePath = Path.Combine(FileSystem.AppDataDirectory, "StudentsData.txt");
        private readonly string _schoolClassesfilePath = Path.Combine(FileSystem.AppDataDirectory, "SchoolClassesData.txt");

        private void EnsureFileExists()
        {
            if(!File.Exists(_studentsFilePath))
                File.Create(_studentsFilePath).Close();

            if (!File.Exists(_schoolClassesfilePath))
                File.Create(_schoolClassesfilePath).Close();
        }

        public async Task<ObservableCollection<SchoolClass>> LoadData()
        {
            EnsureFileExists();
            var studentsTask = LoadStudents();
            var classesTask = LoadSchoolClasses();

            await Task.WhenAll(studentsTask, classesTask);

            var studentsDictionary = await studentsTask;
            var schoolClasses = await classesTask;

            var classLookup = schoolClasses.ToDictionary(c => c.Id);
            foreach (var studentEntry in studentsDictionary)
            {
                var student = studentEntry.Key;
                var schoolClassId = studentEntry.Value;

                if (classLookup.TryGetValue(schoolClassId, out var targetClass))
                {
                    student.StudentNumber = targetClass.Students.Count() + 1;
                    targetClass.Students.Add(student);
                }             
            }

            return schoolClasses;
        }

        private async Task<Dictionary<Student, Guid>> LoadStudents()
        {
            string[] lines = await File.ReadAllLinesAsync(_studentsFilePath);
            var studentsDictionary = new Dictionary<Student, Guid>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] studentData = line.Split('|');

                var student = new Student
                {
                    Id = Guid.Parse(studentData[0].Trim()),
                    StudentNumber = -1,
                    FirstName = studentData[1].Trim(),
                    LastName = studentData[2].Trim(),
                    IsPresent = bool.Parse(studentData[3].Trim()),
                    DrawCooldown = short.Parse(studentData[4].Trim())
                };
                Guid schoolClassId = Guid.Parse(studentData[5]);

                studentsDictionary.Add(student, schoolClassId);
            }

            return studentsDictionary;
        }

        private async Task<ObservableCollection<SchoolClass>> LoadSchoolClasses()
        {
            string[] lines = await File.ReadAllLinesAsync(_schoolClassesfilePath);
            var schoolClasses = new ObservableCollection<SchoolClass>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split('|');

                var schoolClass = new SchoolClass
                {
                    Id = Guid.Parse(parts[0].Trim()),
                    Name = parts[1].Trim(),
                    Students = []
                };

                schoolClasses.Add(schoolClass);
            }

            return schoolClasses;
        }
        public async Task SaveData(ObservableCollection<SchoolClass> allSchoolClasses)
        {
            EnsureFileExists();
            var schoolClassesDictionary = new Dictionary<Guid, string>();
            var studentsDictionary = new Dictionary<Student, Guid>();

            foreach (var schoolClass in allSchoolClasses)
            {
                schoolClassesDictionary[schoolClass.Id] = schoolClass.Name;           
                var students = schoolClass.Students;

                foreach (var student in students)
                    studentsDictionary[student] = schoolClass.Id;      
            }       
            
            var studentsTask = SaveStudents(studentsDictionary);
            var classesTask = SaveSchoolClasses(schoolClassesDictionary);

            await Task.WhenAll(studentsTask, classesTask);
        }

        private async Task SaveStudents(Dictionary<Student, Guid> studentsDictionary)
        {
            var lines = studentsDictionary.Select(keyValuePair =>
            {
                var student = keyValuePair.Key;
                var classId = keyValuePair.Value;

                return $"{student.Id}|{student.FirstName}|{student.LastName}|{student.IsPresent}|{student.DrawCooldown}|{classId}";
            });

            await File.WriteAllLinesAsync(_studentsFilePath, lines, Encoding.UTF8);
        }

        private async Task SaveSchoolClasses(Dictionary<Guid, string> schoolClasses)
        {
            var lines = schoolClasses.Select(keyValuePair => $"{keyValuePair.Key}|{keyValuePair.Value}");

            await File.WriteAllLinesAsync(_schoolClassesfilePath, lines, Encoding.UTF8);
        }
    }
}
