using Android.Content;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using TaxometrMauiMvvm.Data.Json;

namespace TaxometrMauiMvvm.Services.Background
{
    public static class BPCProvider
    {
        public const string EXTERNAL_INPUT_DATA_KEY = "ru.m4bank.ExternalApplication.InputDataKey";
        public const string EXTERNAL_RESULT_DATA_KEY = "ru.m4bank.ExternalApplication.ResultDataKey";
        public const string EXTERNAL_OPERATION_TYPE_KEY = "ru.m4bank.ExternalApplication.OperationTypeKey";

        public static async Task<bool> Pay(int sum, Page page)
        {
            bool hasResult = false;
            bool res = false;
            var pay = new ExternalPaymentApp(sum, page);
            pay.Compleated += ((result) =>
            {
                res = result;
                hasResult = true;
            });
            pay.Transmit();
            while (!hasResult)
            {
                await Task.Delay(100);
            }

            return res;
        }
    }

    internal class ExternalPaymentApp
    {
        internal event Action<bool> Compleated;

        private int _sum;
        private Page _page;

        internal ExternalPaymentApp(int sum, Page page)
        {
            _sum = sum;
            _page = page;
        }

        internal async void Transmit()
        {
            string countStr = _sum.ToString();

            string token = await GetToken();
            Request request = new Request
            (
                new Request.Credentials
                (
                    token
                ),
                new Request.OperationData
                (
                    "CARD",
                    "01",
                    new Request.OperationData.AmountData
                    (
                        "933",
                        countStr,
                        "2"
                    ),
                    new Request.OperationData.Goods
                    (
                        [new Request.OperationData.Goods.Product
                        (
                            "Поездка на такси",
                            countStr,
                            "1",
                            "0",
                            "WITHOUT_TAX",
                            "SERVICE"
                        )]
                    )
                )
            );

            string jsonRequest = JsonConvert.SerializeObject(request, Newtonsoft.Json.Formatting.Indented);

            Intent intent = new Intent();
            intent.SetComponent(new ComponentName("ru.m4bank.softpos.npcby", "ru.m4bank.feature.externalapplication.ExternalApplicationActivity"));
            intent.SetFlags(ActivityFlags.SingleTop);
            intent.PutExtra(BPCProvider.EXTERNAL_OPERATION_TYPE_KEY, OperationType.PAYMENT.ToString());
            intent.PutExtra(BPCProvider.EXTERNAL_INPUT_DATA_KEY, jsonRequest);

            MainActivity.Current.ActivityResult += Current_ActivityResult;

            MainActivity.Current.StartActivityForResult(intent, 10);
        }
        private void Current_ActivityResult(string result)
        {
            MainActivity.Current.ActivityResult -= Current_ActivityResult;

            Debug.WriteLine( result );

            int code = -1;
            int codeIndex = result.IndexOf("\"code\": ");
            if (codeIndex != -1)
            {
                string c = result.Substring(codeIndex + 9, 2);
                if (c.Contains("\"")) c = c.Remove(c.IndexOf("\""));
                code = int.Parse(c);
                if (code == 0)
                {
                    Compleated?.Invoke(true);
                    return;
                }
            }

            Compleated?.Invoke(false);
        }

        public enum OperationType
        {
            PAYMENT,                    // Payment
            REFUND,                     // Return
            REVERSAL,                   // Cancel
            CLOSE_DAY,                  // Closing the trading day
            OPERATIONS_LIST,            // Getting the list of operations
            UPDATE_TERMINAL_KEYS,       // Loading keys to the device
            SIGN_IN_APPLICATION,        // Authorization without a response
            GET_INTERMEDIATE_REPORT,    // Getting transactions list for the intermediate report,
            SEND_LOGS,                  // Sending logs
        }


        private bool compleate = false;
        private async Task<string> GetToken()
        {
            string result = "";

            CancellationTokenSource serverTimeoutException = new CancellationTokenSource();
            compleate = false;
            bool canceled = false;
            Dispatcher.GetForCurrentThread()?.StartTimer(TimeSpan.FromSeconds(5), new Func<bool>(() =>
            {
                canceled = true;
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
                        result = await AfterConnection(client, serverTimeoutException.Token);
                    }
                    catch (Exception e1)
                    {
                        Debug.WriteLine(e1.Message);
                    }
                    finally
                    {
                        if (!compleate && canceled) await PromptRetry();
                    }
                }
            }
            catch (SocketException e)
            {
                Debug.WriteLine(e.Message);
            }
            return result;
        }
        private async Task<string> PromptRetry()
        {
            if (await _page.DisplayAlert("Не удалось подключиться к серверу", "Хотите повторить запрос?", "Да", "Нет"))
            {
                return await GetToken();
            }

            return "";
        }
        private async Task<string> AfterConnection(TcpClient client, CancellationToken cancellationToken)
        {
            string token = "";

            Debug.WriteLine("Подключено");
            NetworkStream ns = client.GetStream();

            string request = "/GetToken";
            byte[] data = Encoding.ASCII.GetBytes(request);

            ns.Write(data, 0, data.Length);
            ns.Flush();

            byte[] bufer = new byte[1024];
            int lenth = await ns.ReadAsync(bufer, cancellationToken);
            string answer = Encoding.ASCII.GetString(bufer, 0, lenth);
            Debug.WriteLine(answer);
            if (lenth == 0)
            {
                return "";
            }

            compleate = true;
            try
            {
                token = (answer.Trim().Split(' '))[1];

                data = Encoding.ASCII.GetBytes("OK");
                ns.Write(data, 0, data.Length);
                ns.Flush();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            return token;
        }


    }
}
