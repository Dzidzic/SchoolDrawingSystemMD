using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using SchoolDrawingSystemMD.Models;
using SchoolDrawingSystemMD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDrawingSystemMD.ViewModels
{
    public partial class StudentFormViewModel(TxtFileServices fileServices) : ObservableObject
    {
        [ObservableProperty]
        private Student _studentInfo = new();
        public event Action<Student> OnSubmit;

        public void SetStudent(Student existingStudent)
        {
            StudentInfo = existingStudent;
        }


        [RelayCommand]
        private void HandleSubmitBtn()
        {
            if (string.IsNullOrWhiteSpace(StudentInfo.FirstName)) return;
            if (string.IsNullOrWhiteSpace(StudentInfo.LastName)) return;

            if (StudentInfo.Id == Guid.Empty)
                StudentInfo.Id = Guid.NewGuid();

            OnSubmit?.Invoke(StudentInfo);

            MopupService.Instance.PopAsync();
        }
    }
}
