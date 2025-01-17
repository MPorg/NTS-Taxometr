using Android.App;
using Android.Content;
using Android.Widget;
using Taxometr.Droid.Modules.Dependency;
using Taxometr.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMaker))]

namespace Taxometr.Droid.Modules.Dependency
{
    internal class ToastMaker : IToastMaker
    {
        Context _context = Application.Context;

        public void Show(string message)
        {
            Toast.MakeText(_context, message, ToastLength.Long).Show();
        }
    }
}