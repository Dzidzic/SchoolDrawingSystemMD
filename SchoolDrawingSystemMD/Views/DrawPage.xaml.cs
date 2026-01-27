using SchoolDrawingSystemMD.Services;
using SchoolDrawingSystemMD.ViewModels;

namespace SchoolDrawingSystemMD.Views;

public partial class DrawPage : ContentPage
{
    public DrawPage(DrawingSystemViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is DrawingSystemViewModel vm)
        {
            await vm.LoadData();
        }
    }
}