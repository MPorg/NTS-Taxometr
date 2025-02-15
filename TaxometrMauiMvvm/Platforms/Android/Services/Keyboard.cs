using Android.Views.InputMethods;
using Android.App;
using Android.Content;
using TaxometrMauiMvvm.Interfaces;
using AndroidApp = Android.App;

namespace TaxometrMauiMvvm.Platforms.Android.Services
{
    internal class Keyboard : IKeyboard
    {
        public void Hide()
        {
            var context = AndroidApp.Application.Context;
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