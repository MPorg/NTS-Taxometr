using Android.Content;
using Taxometr.Data;
using Taxometr.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Taxometr.Droid.Resources.ButtonExtras;

namespace Taxometr.Droid.Services
{
    [BroadcastReceiver(Enabled = true, Description = "Notification Button Reciver", Name = "com.nts.taxometr.notif_btn_res")]
    public class NotificationButtonReciver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.GetBooleanExtra(CancelBtn, false))
            {
                OnCancelBtnClicked();
            }
        }

        private async void OnCancelBtnClicked()
        {
            AppData.Debug.WriteLine("_____________________CANCEL BUTTON CLICK_____________________");
            try
            {
                await AppData.SpecialDisconnect();
            }
            catch { }
            Android.App.Application.Context.GetActivity()?.Finish();
            DependencyService.Resolve<IBLEConnectionController>().Stop();
        }
    }
}