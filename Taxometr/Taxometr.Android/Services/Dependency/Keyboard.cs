using Xamarin.Forms;
using Android.Views.InputMethods;
using Android.App;
using Android.Content;
using Taxometr.Interfaces;

[assembly: Dependency(typeof(Taxometr.Droid.Services.Dependency.Keyboard))]
namespace Taxometr.Droid.Services.Dependency
{
    internal class Keyboard : IKeyboard
    {
        public void Hide()
        {
            var context = Android.App.Application.Context;
            var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && context is Activity)
            {
                var activity = context as Activity;
                var token = activity.CurrentFocus?.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                activity.Window.DecorView.ClearFocus();
            }
        }
    }
}