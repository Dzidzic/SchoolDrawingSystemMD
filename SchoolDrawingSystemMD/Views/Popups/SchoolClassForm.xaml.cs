using Mopups.Pages;
using SchoolDrawingSystemMD.Models;
using SchoolDrawingSystemMD.ViewModels;

namespace SchoolDrawingSystemMD.Views.Popups;

public partial class SchoolClassForm : PopupPage
{
	public SchoolClassForm(SchoolClassFormViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    public void InitializeForEdit(SchoolClass existingSchoolClass)
    {
        if (BindingContext is SchoolClassFormViewModel vm)
        {
            vm.SetSchoolClass(existingSchoolClass);
            FormTitle.SetValue(Label.TextProperty, "EDYTUJ KLASÊ");
            FormButton.SetValue(Button.TextProperty, "Zapisz");
        }
    }
}