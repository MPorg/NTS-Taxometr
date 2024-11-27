using Android.App;
using Android.Content;
using Android.Widget;
using Taxometr.Droid.Services;
using Taxometr.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMaker))]
namespace Taxometr.Droid.Services
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