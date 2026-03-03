using CommunityToolkit.Mvvm.Messaging;
using SchoolDrawingSystemMD.Services;
using SchoolDrawingSystemMD.ViewModels;
using static SchoolDrawingSystemMD.ViewModels.DrawingSystemViewModel;

namespace SchoolDrawingSystemMD.Views;

public partial class DrawPage : ContentPage
{
    public DrawPage(DrawingSystemViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        WeakReferenceMessenger.Default.Register<StartAnimationMessage>(this, async (r, m) =>
        {
            await RunSlotMachineAnimation(m.Value.pool, m.Value.winner, m.Value.student);
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is DrawingSystemViewModel vm)
        {
            await vm.LoadData();
        }
    }

    private async Task RunSlotMachineAnimation(int[] pool, int winner, string student)
    {
        if(winner == -1)
        {
            resultLabel.Text = "?";
            resultLabel.TextColor = Colors.White;

            drawedStudent.Text = "Pula uczniów jest pusta";
            drawedStudent.TextColor = Colors.OrangeRed;

            return;
        }

        drawedStudent.Text = "";
        Random rnd = new();
        int totalCycles = 20;

        Color colorStart = Color.FromArgb("#404040");
        Color colorEnd = Colors.White;

        for (int i = 0; i < totalCycles; i++)
        {
            resultLabel.Text = pool[rnd.Next(pool.Length)].ToString();

            int delay = 40;
            if (i >= 10)
                delay += (int)Math.Pow(i - 10, 3.15);

            float progress = (float)i / (totalCycles - 1);
            resultLabel.TextColor = Color.FromRgb(
                colorStart.Red + (colorEnd.Red - colorStart.Red) * progress,
                colorStart.Green + (colorEnd.Green - colorStart.Green) * progress,
                colorStart.Blue + (colorEnd.Blue - colorStart.Blue) * progress
            );

            uint animTime = (uint)Math.Min(delay / 2, 50);

            await resultLabel.ScaleTo(1.1, animTime, Easing.CubicOut);
            await Task.Delay(delay);
            await resultLabel.ScaleTo(1.0, animTime, Easing.CubicIn);
        }

        resultLabel.Text = winner.ToString();
        resultLabel.TextColor = Colors.Goldenrod;

        drawedStudent.Text = student;
        drawedStudent.TextColor = Colors.Goldenrod;

        await resultLabel.ScaleTo(1.7, 300, Easing.SpringOut);

        await Task.Delay(700);
        resultLabel.TextColor = Colors.Snow;
        await resultLabel.ScaleTo(1.0, 600, Easing.BounceOut);
    }
}