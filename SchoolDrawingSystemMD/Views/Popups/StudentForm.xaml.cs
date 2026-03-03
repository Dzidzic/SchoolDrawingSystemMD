using Mopups.Pages;
using SchoolDrawingSystemMD.Models;
using SchoolDrawingSystemMD.ViewModels;

namespace SchoolDrawingSystemMD.Views.Popups;

public partial class StudentForm : PopupPage
{
	public StudentForm(StudentFormViewModel vw)
	{
		InitializeComponent();
        BindingContext = vw;
	}

    public void InitializeForEdit(Student existingStudent)
    {
        if (BindingContext is StudentFormViewModel vm)
        {
            vm.SetStudent(existingStudent);
            FormTitle.SetValue(Label.TextProperty, "EDYTJ UCZNIA");
            FormButton.SetValue(Button.TextProperty, "Zapisz");
        }
    }
}