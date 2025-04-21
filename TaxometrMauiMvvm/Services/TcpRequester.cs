using System.Net.Sockets;
using System.Net;
using System.Text;
using TaxometrMauiMvvm.Services.Background;
using static AndroidX.Concurrent.Futures.CallbackToFutureAdapter;
using System.Diagnostics;
using Android.App;
using Android.Content;
using TaxometrMauiMvvm.Views.Banners;
using TaxometrMauiMvvm.Models.Banners;

namespace TaxometrMauiMvvm.Services
{
    public class TcpRequester
    {
        private int _versionCode;

        private Page _currentPage;

        private DownloadReceiver _receiver;

        public void Inject(DownloadReceiver receiver)
        {
            _receiver = receiver;
            _versionCode = Android.App.Application.Context.PackageManager.GetPackageInfo(AppInfo.PackageName, 0).VersionCode;
        }

        public async Task CheckForUpdates(Page page)
        {
            _currentPage = page;
            await Connect();
        }

        private bool _connectionCompleated = false;
        private bool _connectionCanceled = false;
        private async Task Connect()
        {
            CancellationTokenSource serverTimeoutException = new CancellationTokenSource();
            _connectionCompleated = false;
            _connectionCanceled = false;
            Dispatcher.GetForCurrentThread()?.StartTimer(TimeSpan.FromSeconds(5), new Func<bool>(() =>
            {
                _connectionCanceled = true;
                serverTimeoutException.Cancel();
                Debug.WriteLine("CANCELED");
                return false;
            }));

            using TcpClient client = new TcpClient();
            try
            {
                try
                {
                    await client.ConnectAsync("1c.ntsretail.by", 18888, serverTimeoutException.Token);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    try
                    {
                        await client.ConnectAsync(IPAddress.Parse("192.168.3.37"), 8888, serverTimeoutException.Token);
                    }
                    catch (Exception e1)
                    {
                        Debug.WriteLine(e1.Message);
                    }
                }
                finally
                {
                    try
                    {
                        await AfterConnection(client, serverTimeoutException.Token);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    finally
                    {
                        //if (!_connectionCompleated && _connectionCanceled) await PromptRetry();
                    }
                }
            }
            catch (SocketException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async Task PromptRetry()
        {
            if (await _currentPage.DisplayAlert("Не удалось подключиться к серверу", "Хотите повторить запрос?", "Да", "Нет"))
            {
                await Connect();
            }

            return;
        }

        private async Task PromptUpdateCompleate()
        {
            //await _currentPage.DisplayAlert("Обновление завершено", "", "Ок");
            UpdateCompleateBanner banner = new UpdateCompleateBanner(new UpdateCompleateViewModel());
            bool compleate = false;
            banner.Complete += () =>
            {
                _currentPage.Navigation.PopModalAsync();
                compleate = true;
            };
            await _currentPage.Navigation.PushModalAsync(banner);
            while (!compleate)
            {
                await Task.Delay(100);
            }
        }

        private async Task AfterConnection(TcpClient client, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Подключено");
            NetworkStream ns = client.GetStream();

            string request = AppInfo.PackageName;
            byte[] data = Encoding.ASCII.GetBytes(request);

            ns.Write(data, 0, data.Length);
            ns.Flush();

            byte[] bufer = new byte[1024];

            int lenth = await ns.ReadAsync(bufer, cancellationToken);
            string answer = Encoding.ASCII.GetString(bufer, 0, lenth);
            Debug.WriteLine(answer);
            if (lenth == 0)
            {
                return;
            }

            _connectionCompleated = true;
            try
            {

                string[] rows = answer.Split(',');

                int i = 0;
                foreach (string r in rows)
                {
                    Debug.WriteLine($"[{i}]: {r}");
                    i++;
                }

                string versionInt = (rows[0].Trim().Split(' '))[1];
                if (int.TryParse(versionInt, out int v))
                {
                    string filePath = (rows[1].Trim().Split(' '))[1];
                    string fileName = (rows[2].Trim().Split(' '))[1];
                    if (v > _versionCode)
                    {
                        await PromptUserToUpdate(filePath, fileName);
                    }
                    else
                    {
                        //await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Нет доступных обновлений", "", "Ок");
                        if (IsFileInDownloads(fileName, out string filePathOnDownloads))
                        {
                            try
                            {
                                File.Delete(filePathOnDownloads);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                            finally
                            {
                                await PromptUpdateCompleate();
                            }
                        }
                    }
                }


                data = Encoding.ASCII.GetBytes("OK");
                ns.Write(data, 0, data.Length);
                ns.Flush();
            }
            catch (Exception e)
            {
                Debug.WriteLine (e);
            }
        }

        private async Task PromptUserToUpdate(string apkUrl, string fileName)
        {
            AutoUpdateBanner banner = new AutoUpdateBanner();
            bool hasAnswer = false;
            banner.AnswerCompleate += new Action<bool>((result) =>
            {
                if (result)
                {
                    Dispatcher.GetForCurrentThread()?.Dispatch(async () =>
                    {
                        if (IsFileInDownloads(fileName, out string filePath))
                        {
                            Debug.WriteLine(filePath + " Exist");
                            DownloadReceiver.InstallApk(Android.App.Application.Context, filePath);
                            return;
                        }

                        // Загрузить APK и установить
                        await DownloadAndInstallApk(apkUrl, fileName);
                    });
                }
                _currentPage.Navigation.PopAsync();
                hasAnswer = true;
            });
            await _currentPage.Navigation.PushModalAsync(banner);
            while(!hasAnswer)
            {
                await Task.Delay(100);
            }
        }

        public bool IsFileInDownloads(string fileName, out string filePath)
        {
            // Получаем путь к папке загрузок
            var downloadsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;

            // Формируем полный путь к файлу
            filePath = Path.Combine(downloadsPath, fileName);

            // Проверяем существует ли файл
            return File.Exists(filePath);
        }

        private async Task<long> DownloadAPK(string apkUrl, string fileName)
        {
            var downloadManager = (Android.App.DownloadManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.DownloadService);
            var request = new Android.App.DownloadManager.Request(Android.Net.Uri.Parse(apkUrl));
            request.SetNotificationVisibility(Android.App.DownloadVisibility.VisibleNotifyCompleted);
            //request.SetNotificationVisibility(Android.App.DownloadVisibility.Hidden);
            request.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads, fileName);
            return downloadManager.Enqueue(request);
        }

        private async Task RegisterInstullApk(long downloadId)
        {
            _receiver.Initialize(downloadId);
            Android.App.Application.Context.RegisterReceiver(_receiver, new IntentFilter(DownloadManager.ActionDownloadComplete), ReceiverFlags.Exported);
        }
        private async Task DownloadAndInstallApk(string apkUrl, string fileName)
        {
            long downloadId = await DownloadAPK(apkUrl, fileName);
            await RegisterInstullApk(downloadId);
        }
    }
}
