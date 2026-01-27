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
    public partial class SchoolClassFormViewModel(TxtFileServices fileServices) : ObservableObject
    {
        [ObservableProperty]
        private SchoolClass _classInfo = new();

        public event Action<SchoolClass> OnSubmit;

        public void SetSchoolClass(SchoolClass existingSchoolClass)
        {
            ClassInfo = existingSchoolClass;
        }

        [RelayCommand]
        private void HandleSubmitBtn()
        {
            if (string.IsNullOrWhiteSpace(ClassInfo.Name)) return;

            if (ClassInfo.Id == Guid.Empty) 
                ClassInfo.Id = Guid.NewGuid();

            OnSubmit?.Invoke(ClassInfo);

            MopupService.Instance.PopAsync();
        }
    }
}
