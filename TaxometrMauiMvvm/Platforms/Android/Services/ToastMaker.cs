using Android.App;
using Android.Content;
using Android.Widget;
using System.Diagnostics;
using TaxometrMauiMvvm.Interfaces;
using AndroidApp = Android.App;

namespace TaxometrMauiMvvm.Platforms.Android.Services
{
    public class ToastMaker : IToastMaker
    {
        Context _context;
        public void Show(string message)
        {
            try
            {
                _context = AndroidApp.Application.Context;
                Toast.MakeText(_context, message, ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
