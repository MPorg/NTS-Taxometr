using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using System.Text;
using TaxometrMauiMvvm.Interfaces;
using TaxometrMauiMvvm.Models.Cells;
using TaxometrMauiMvvm.Models.Pages;
using TaxometrMauiMvvm.Platforms.Android.Services;
using TaxometrMauiMvvm.Views.Cells;
using TaxometrMauiMvvm.Views.Pages;

namespace TaxometrMauiMvvm
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "IconsRegular");
                    fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "IconsSolid");
                    fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "IconsBrands");

                    fonts.AddFont("Geologica-Bold.ttf", "Geologica-Bold");
                    fonts.AddFont("Geologica-Italic.ttf", "Geologica-Medium");
                    fonts.AddFont("Geologica-BoldItalic.ttf", "Geologica-SemiBold");
                    fonts.AddFont("Geologica-Regular.ttf", "Geologica");
                });

            builder.Services.AddSingleton<RemotePage>();
            builder.Services.AddSingleton<RemoteViewModel>();

            builder.Services.AddSingleton<DrivePage>();
            builder.Services.AddSingleton<DriveViewModel>();

            builder.Services.AddSingleton<PrintPage>();
            builder.Services.AddSingleton<PrintViewModel>();

            builder.Services.AddSingleton<SearchPage>();
            builder.Services.AddSingleton<SearchViewModel>();

            builder.Services.AddSingleton<SavedPage>();
            builder.Services.AddSingleton<SavedViewModel>();

            builder.Services.AddSingleton<SavedDeviceViewCell>();
            builder.Services.AddSingleton<SavedDeviceViewModel>();


            builder.Services.AddTransient<IToastMaker, ToastMaker>();
            builder.Services.AddTransient<ISettingsManager, SettingsManager>();
            builder.Services.AddTransient<IKeyboard, Platforms.Android.Services.Keyboard>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
