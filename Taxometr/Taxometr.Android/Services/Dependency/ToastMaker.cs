using Android.App;
using Android.Content;
using Android.Widget;
using Taxometr.Droid.Services.Dependency;
using Taxometr.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMaker))]
namespace Taxometr.Droid.Services.Dependency
{
    internal class ToastMaker : IToastMaker
    {
        Context context = Application.Context;

        public void Show(string message)
        {
            Toast.MakeText(context, message, ToastLength.Long).Show();
        }
    }
}