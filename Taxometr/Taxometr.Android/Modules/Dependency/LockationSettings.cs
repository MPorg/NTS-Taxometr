using Android.Content;
using Taxometr.Droid.Modules.Dependency;
using Taxometr.Interfaces;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(LockationSettings))]

namespace Taxometr.Droid.Modules.Dependency
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