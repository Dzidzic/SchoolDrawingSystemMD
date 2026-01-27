using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using SchoolDrawingSystemMD.Services;
using SchoolDrawingSystemMD.ViewModels;

namespace SchoolDrawingSystemMD
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<TxtFileServices>();

            builder.Services.AddTransient<DrawingSystemViewModel>();
            builder.Services.AddTransient<SchoolClassFormViewModel>();
            builder.Services.AddTransient<StudentFormViewModel>();

            builder.Services.AddTransient<Views.DrawPage>();

            return builder.Build();
        }
    }
}
