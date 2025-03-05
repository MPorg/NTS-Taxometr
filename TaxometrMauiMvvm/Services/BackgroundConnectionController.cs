using Android.Content;
using TaxometrMauiMvvm.Interfaces;
using TaxometrMauiMvvm.Services.Background;
using AndroidApp = global::Android.App;

namespace TaxometrMauiMvvm.Platforms.Android.Services;

public class BackgroundConnectionController : IBackgroundConnectionController
{
    public void Start()
    {
        Intent intent = CreateIntent();
        AndroidApp.Application.Context.StartService(intent);
    }

    public void Stop()
    {
        Intent intent = CreateIntent();
        AndroidApp.Application.Context.StopService(intent);
    }

    private Intent CreateIntent()
    {
        return new Intent(AndroidApp.Application.Context, typeof(ConnectionWorker));
    }
}
