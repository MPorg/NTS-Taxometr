using Android.App;
using Android.Content;
using System.Diagnostics;

namespace TaxometrMauiMvvm.Services.Background
{
    [BroadcastReceiver(Name = "com.nts.taxometr.download_res", Exported = false)]
    [IntentFilter(new string[] { DownloadManager.ActionDownloadComplete })]
    public class DownloadReceiver : BroadcastReceiver
    {
        private long downloadId;

        public void Initialize(long id)
        {
            downloadId = id;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            long id = intent.GetLongExtra(DownloadManager.ExtraDownloadId, -1);
            if (id == downloadId)
            {
                DownloadManager downloadManager = (DownloadManager)context.GetSystemService(Context.DownloadService);
                DownloadManager.Query query = new DownloadManager.Query();
                query.SetFilterById(downloadId);
                var cursor = downloadManager.InvokeQuery(query);
                if (cursor.MoveToFirst())
                {
                    string uriString = cursor.GetString(cursor.GetColumnIndex(DownloadManager.ColumnLocalUri));
                    InstallApk(context, uriString);
                }
                cursor.Close();
            }
        }

        public static void InstallApk(Context context, string uriString)
        {
            string filePath = uriString;
            if (filePath.Contains("file://")) filePath = filePath.Replace("file://", "");
            Debug.WriteLine(filePath);

            var file = new Java.IO.File(filePath);
            var uri = FileProvider.GetUriForFile(context, $"{context.PackageName}.fileprovider", file);

            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, "application/vnd.android.package-archive");
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.AddFlags(ActivityFlags.GrantWriteUriPermission);
            intent.AddFlags(ActivityFlags.NewTask);
            context.StartActivity(intent);
        }
    }
}
