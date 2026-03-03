using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mopups.Services;
using SchoolDrawingSystemMD.Models;
using SchoolDrawingSystemMD.Services;
using SchoolDrawingSystemMD.Views.Popups;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchoolDrawingSystemMD.ViewModels
{
    public partial class DrawingSystemViewModel(TxtFileServices fileServices) : ObservableObject
    {
        [ObservableProperty]
        private short _drawedNumber;

        [ObservableProperty]
        private short _luckyNumber;

        [ObservableProperty]
        private ObservableCollection<SchoolClass> _allSchoolClasses = [];

        [ObservableProperty]
        private SchoolClass? _selectedClass;

        public async Task LoadData()
        {
            AllSchoolClasses = await fileServices.LoadData();
            if(SelectedClass == null)
                SelectedClass = AllSchoolClasses.FirstOrDefault();
        }

        [RelayCommand]
        private async void DrawLuckyNumber()
        {
            int lastLuckyNumber = LuckyNumber;
            int maxRange = SelectedClass.Students.Count() + 1;
            Random rng = new();

            while (true)
            {        
                LuckyNumber = (short)rng.Next(1, maxRange);
                if(LuckyNumber != lastLuckyNumber)
                    break;
            }
        }

        [RelayCommand]
        private async Task ChangeStudentPresence(Student student)
        {
            student.IsPresent = !student.IsPresent;
            await fileServices.SaveData(AllSchoolClasses);
        }

        [RelayCommand]
        private async void DrawStudent()
        {
            int[] availableStudents = [];

            foreach (var student in SelectedClass.Students)
            {
                if (student.IsPresent && student.DrawCooldown == 0 && student.StudentNumber != LuckyNumber)
                    availableStudents = availableStudents.Append(student.StudentNumber).ToArray();
            }

            int availableStudentsCount = availableStudents.Length;
            Random rng = new();

            if (availableStudents.Length == 0)
            {
                DecreaseDrawCooldowns();
                DrawedNumber = -1;

                var data = new DrawData(availableStudents, DrawedNumber, "");
                WeakReferenceMessenger.Default.Send(new StartAnimationMessage(data));

                await fileServices.SaveData(AllSchoolClasses);
                return;
            }

            while (true)
            {
                short tempDrawedNumber = (short)rng.Next(1, ++availableStudentsCount);
                if (availableStudents.Contains(tempDrawedNumber))
                {
                    DecreaseDrawCooldowns();
                    DrawedNumber = tempDrawedNumber;

                    var drawedStudent = SelectedClass.Students.First(s => s.StudentNumber == DrawedNumber);
                    drawedStudent.DrawCooldown = 3;

                    string studentFullName = $"{drawedStudent.FirstName} {drawedStudent.LastName}";

                    var data = new DrawData(availableStudents, DrawedNumber, studentFullName);
                    WeakReferenceMessenger.Default.Send(new StartAnimationMessage(data));    

                    await fileServices.SaveData(AllSchoolClasses);
                    return;
                }
            }
        }
        public record DrawData(int[] pool, int winner, string student);
        public record StartAnimationMessage(DrawData Value);

        private void DecreaseDrawCooldowns()
        {
            foreach (var student in SelectedClass.Students)
                if (student.DrawCooldown > 0)
                    student.DrawCooldown--;
        }

        [RelayCommand]
        private async Task AddNewSchoolClass()
        {
            var popup = InitializeSchoolClassFormPopup();

            await MopupService.Instance.PushAsync(popup);
        }

        [RelayCommand]
        private async Task EditSchoolClass(SchoolClass classToEdit)
        {
            var popup = InitializeSchoolClassFormPopup();
            popup.InitializeForEdit(classToEdit);

            await MopupService.Instance.PushAsync(popup);
        }
        [RelayCommand]
        private async Task DeleteSchoolClass(SchoolClass classToEdit)
        {
            if(AllSchoolClasses.Count == 1)
            {
                await Shell.Current.DisplayAlert("Błąd", "System wymaga co najmniej jednej klasy", "Ok");
                return;
            }

            AllSchoolClasses.Remove(classToEdit);
            await fileServices.SaveData(AllSchoolClasses);
        }

        private SchoolClassForm InitializeSchoolClassFormPopup()
        {
            var vm = IPlatformApplication.Current.Services.GetService<SchoolClassFormViewModel>();

            vm.OnSubmit += async (processedSchoolClass) =>
            {
                if (!AllSchoolClasses.Contains(processedSchoolClass))           
                    AllSchoolClasses.Add(processedSchoolClass);

                var index = AllSchoolClasses.IndexOf(processedSchoolClass);
                if (index != -1)
                {
                    AllSchoolClasses[index] = null;
                    AllSchoolClasses[index] = processedSchoolClass;
                    SelectedClass = processedSchoolClass;
                }

                await fileServices.SaveData(AllSchoolClasses);
            };

            return new SchoolClassForm(vm); ;
        }

        [RelayCommand]
        private async Task AddNewStudent()
        {
            var popup = InitializeStudentFormPopup();

            await MopupService.Instance.PushAsync(popup);
        }

        [RelayCommand]
        private async Task EditStudent(Student studentToEdit)
        {
            var popup = InitializeStudentFormPopup();
            popup.InitializeForEdit(studentToEdit);

            await MopupService.Instance.PushAsync(popup);
        }

        [RelayCommand]
        private async Task DeleteStudent(Student studentToEdit)
        {
            SelectedClass.Students.Remove(studentToEdit);
            for (int i = 0; i < SelectedClass.Students.Count; i++)
                SelectedClass.Students[i].StudentNumber = i + 1;

            await fileServices.SaveData(AllSchoolClasses);
        }

        private StudentForm InitializeStudentFormPopup()
        {
            var vm = IPlatformApplication.Current.Services.GetService<StudentFormViewModel>();

            vm.OnSubmit += async (processedStudent) =>
            {
                var index = SelectedClass.Students.IndexOf(processedStudent);

                if (index == -1)       
                    SelectedClass.Students.Add(processedStudent);
                else
                    SelectedClass.Students[index] = processedStudent;

                await fileServices.SaveData(AllSchoolClasses);
                await LoadData();
            };

            return new StudentForm(vm); ;
        }
    }
}
