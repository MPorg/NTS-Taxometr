using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Xml;
using TaxometrMauiMvvm.Data.Json;
using TaxometrMauiMvvm.Services.Background;

namespace TaxometrMauiMvvm
{
    [Activity(Theme = "@style/Theme.FullScreen", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static MainActivity Current;

        public event Action<string> ActivityResult;
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Current = this;
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (data == null) return;
            string? operationType = data.GetStringExtra(BPCProvider.EXTERNAL_OPERATION_TYPE_KEY);
            string? result = data.GetStringExtra(BPCProvider.EXTERNAL_RESULT_DATA_KEY);
            if (result == null) return;
            Response? response = JsonConvert.DeserializeObject<Response>(result);
            if (response == null) return;
            result = JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.Indented);

            System.Diagnostics.Debug.WriteLine(result);
            ActivityResult?.Invoke(result);
        }
    }
}
