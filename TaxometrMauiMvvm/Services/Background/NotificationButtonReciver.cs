using static TaxometrMauiMvvm.Services.Background.ButtonExtras;
using Android.Content;
using Microsoft.Maui.Platform;
using TaxometrMauiMvvm.Data;

namespace TaxometrMauiMvvm.Services.Background;

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
        //AppData.Debug.WriteLine("_____________________CANCEL BUTTON CLICK_____________________");
        try
        {
            await AppData.SpecialDisconnect();
        }
        catch { }
        Android.App.Application.Context.GetActivity()?.Finish();
        AppData.StopBackground();
    }
}