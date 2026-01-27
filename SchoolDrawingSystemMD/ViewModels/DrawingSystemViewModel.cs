using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using SchoolDrawingSystemMD.Models;
using SchoolDrawingSystemMD.Services;
using SchoolDrawingSystemMD.Views.Popups;
using System.Collections.ObjectModel;

namespace SchoolDrawingSystemMD.ViewModels
{
    public partial class DrawingSystemViewModel(TxtFileServices fileServices) : ObservableObject
    {
        [ObservableProperty]
        private short _drawedNumber;

        [ObservableProperty]
        private ObservableCollection<SchoolClass> _allSchoolClasses = [];

        [ObservableProperty]
        private SchoolClass? _selectedClass;

        public async Task LoadData()
        {
            AllSchoolClasses = await fileServices.LoadData();
            SelectedClass = AllSchoolClasses.FirstOrDefault();
        }

        [RelayCommand]
        private void DrawStudent()
        {
            short studentsCount = (short) SelectedClass.Students.Count;
            Random rng = new();

            DrawedNumber = (short) rng.Next(1, ++studentsCount);
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
            };

            return new StudentForm(vm); ;
        }
    }
}
