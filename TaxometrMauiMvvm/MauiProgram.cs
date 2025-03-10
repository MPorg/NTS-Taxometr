using Android.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using System.Text;
using TaxometrMauiMvvm.Interfaces;
using TaxometrMauiMvvm.Models.Cells;
using TaxometrMauiMvvm.Models.Pages;
using TaxometrMauiMvvm.Models.Pages.SignIn;
using TaxometrMauiMvvm.Platforms.Android.Services;
using TaxometrMauiMvvm.Services.Background;
using TaxometrMauiMvvm.Views.Cells;
using TaxometrMauiMvvm.Views.Pages;
using TaxometrMauiMvvm.Views.Pages.SignIn;

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
                })
                .ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                    events.AddAndroid(android => android.OnCreate((activity, bandle) => MakeSatusBarTranslucent(activity)));

                    static void MakeSatusBarTranslucent(Android.App.Activity activity)
                    {
                        //activity.Window.SetFlags(Android.Views.WindowManagerFlags.Fullscreen, Android.Views.WindowManagerFlags.Fullscreen);
                        /*activity.Window?.SetFlags(Android.Views.WindowManagerFlags.LayoutInScreen | Android.Views.WindowManagerFlags.KeepScreenOn);
                        activity.Window?.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
                        activity.Window?.SetStatusBarColor(Android.Graphics.Color.Transparent);
                        activity.Window?.SetNavigationBarColor(Android.Graphics.Color.Transparent);*/
                        //activity.Window?.SetFormat(Android.Graphics.Format.Transparent);

                        activity.Window?.AddFlags(WindowManagerFlags.LayoutNoLimits);
                        activity.Window?.ClearFlags(WindowManagerFlags.TranslucentStatus);
                        activity.Window?.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                        activity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.HideNavigation | SystemUiFlags.ImmersiveSticky);

                        activity.Window?.SetStatusBarColor(activity.Resources.GetColor(Resource.Color.translucent));

                    }
#endif
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

            builder.Services.AddSingleton<SignInPage>();
            builder.Services.AddSingleton<SignInViewModel>();

            builder.Services.AddSingleton<SignUpPage>();
            builder.Services.AddSingleton<SignUpViewModel>();

            builder.Services.AddSingleton<SavedDeviceViewCell>();
            builder.Services.AddSingleton<SavedDeviceViewModel>();

            builder.Services.AddSingleton<TabBarViewModel>();

            builder.Services.AddTransient<IToastMaker, ToastMaker>();
            builder.Services.AddTransient<ISettingsManager, SettingsManager>();
            builder.Services.AddTransient<IKeyboard, Platforms.Android.Services.Keyboard>();
            builder.Services.AddTransient<IBackgroundConnectionController, BackgroundConnectionController>();
            builder.Services.AddTransient<INotificationService, NotificationService>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
