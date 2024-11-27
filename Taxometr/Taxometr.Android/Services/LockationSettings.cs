using Android.Content;
using Taxometr.Droid.Services;
using Taxometr.Interfaces;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(LockationSettings))]
namespace Taxometr.Droid.Services
{
    internal class LockationSettings : ILockationSettings
    {
        public void Show()
        {
            Intent intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
            Platform.CurrentActivity.StartActivity(intent);
        }
    }
}