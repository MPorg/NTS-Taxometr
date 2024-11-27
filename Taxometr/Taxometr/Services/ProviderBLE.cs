using Plugin.BLE.Abstractions.Contracts;
using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using Taxometr.Data;
using Xamarin.Forms;

namespace Taxometr.Services
{
    public static class ProviderBLE
    {
        private static Guid BLUETOOTH_LE_INCOTEX_SERVICE = new Guid("0000fff0-0000-1000-8000-00805f9b34fb");
        private static Guid BLUETOOTH_LE_INCOTEX_CHAR_R = new Guid("0000fff1-0000-1000-8000-00805f9b34fb");
        private static Guid BLUETOOTH_LE_INCOTEX_CHAR_W = new Guid("0000fff2-0000-1000-8000-00805f9b34fb");


        private static byte[] STX = new byte[2] { 0x7F, 0x02 };
        private static byte[] ETX = new byte[2] { 0x7F, 0x03 };
        private static byte DLE = 0x10;

        #region CRC7

        public static string GetCrc7(string strInput)
        {
            var poly = 0b10001001;
            byte crc = 0x00;
            byte[] data = HexToBytes(strInput);

            for (int i = 0; i < data.Length; i++)
            {
                crc ^= (byte)(data[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    crc = ((crc & 0x80) > 0) ? (byte)((crc << 1) ^ (poly << 1)) : (crc <<= 1);
                }
            }
            return crc.ToString("X4");
        }

        public static byte GetCrc7(byte[] data)
        {
            var poly = 9;
            byte crc = 0;
            for (int i = 0; i < data.Length; i++)
            {
                crc ^= (byte)(data[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    crc = ((byte)(crc & 0x80u) > 0) ? (byte)((crc << 1) ^ (poly << 1)) : (crc <<= 1);
                }
            }
            return crc;
        }

        public static string ByteArrToStringX2(byte[] bytes)
        {
            string data = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                data += bytes[i].ToString("x2");
            }
            return data;
        }

        #endregion

        #region CRC16


        private static byte[] HexToBytes(string input)
        {
            byte[] result = new byte[input.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(input.Substring(2 * i, 2), 16);
            }
            return result;
        }

        public static string GetCrc16(string strInput)
        {
            ushort crc = 0xFFFF;
            byte[] data = HexToBytes(strInput);
            for (int i = 0; i < data.Length; i++)
            {
                crc ^= (ushort)(data[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x8000) > 0)
                        crc = (ushort)((crc << 1) ^ 0x1021);
                    else
                        crc <<= 1;
                }
            }
            return crc.ToString("X4");
        }

        #endregion

        private static async void SentToBLE(byte[] data)
        {
            //var adapter = AppData.BLEAdapter;
            var adapter = Plugin.BluetoothLE.CrossBleAdapter.Current;
            try
            {
                var device = (await adapter.GetConnectedDevices()).ToList()[0];
                Debug.WriteLine($"device == null ? {device == null} connected ? {device.IsConnected()}");
                List<IGattService> gattServices = new List<IGattService>();
                device.DiscoverServices().Subscribe((s) => 
                {
                    Debug.WriteLine($"Find {s.Uuid}");
                    gattServices.Add(s);
                });

                if (gattServices.Count > 0)
                {
                    foreach (var s  in gattServices)
                    {
                        Debug.WriteLine($"service == null ? {s == null} UUID = {s.Uuid}");
                        /*var characterW = s.GetKnownCharacteristics(BLUETOOTH_LE_INCOTEX_CHAR_W);
                        Debug.WriteLine($"char_W == null ? {characterW == null}");
                        DebugByteStr(data);
                        await characterW.Wait().Write(data);

                        var characterR = await s.GetKnownCharacteristics(BLUETOOTH_LE_INCOTEX_CHAR_R);
                        Debug.WriteLine($"char_R == null ? {characterR == null}");
                        var answer = (await characterR.Read()).Data;
                        Debug.WriteLine($"Answer: {answer.ToString()}");*/
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Не удалось прочитать сообщение {ex.Message} {ex.Source}");
            }
        }

        private static void DebugByteStr(byte[] data)
        {
            string dbg = "";
            for (int i = 0; i < data.Length - 1; i++)
            {
                dbg += data[i].ToString("x2") + "|";
            }
            dbg += data[data.Length - 1].ToString("x2");
            Debug.WriteLine(dbg);
        }

        public static async void StatusSCNO()
        {
            byte id = 1;
            byte flag = 1;
            byte rnd = (byte)new Random().Next(0, 255);
            byte cmd = 0x35;
            byte param = 1;

            string num = await AppData.Properties.GetSerialNumber();
            byte[] numB = new byte[num.Length];
            for (int i = 0; i < num.Length; i++)
            {
                numB[i] = (byte)num[i];
            }

            string data = "";
            foreach (byte b in numB)
            {
                data += b.ToString("x2");
            }
            data += cmd.ToString("x2");
            data += param.ToString("x2");
            var crc = GetCrc16(data);

            var byteCrc = HexToBytes(crc);
            Debug.WriteLine($"{id.ToString("x2")} | {flag.ToString("x2")} | {rnd.ToString("x2")} | {cmd.ToString("x2")} | {param.ToString("x2")} | {byteCrc[1].ToString("x2")} | {byteCrc[0].ToString("x2")}");

            var adapter = AppData.BLEAdapter;
            try
            {
                var services = await AppData.AutoConnectDevice.GetServicesAsync();
                var chars = await services[3].GetCharacteristicsAsync();
                foreach (var character in chars)
                {
                    if (character.CanWrite)
                    {
                        await character.WriteAsync(new byte[7] { id, flag, rnd, cmd, param, byteCrc[1], byteCrc[0]});
                    }

                    if (character.CanRead)
                    {
                        var answer = await character.ReadAsync();
                        Debug.WriteLine("Can read");
                        Debug.WriteLine($"Answer: {answer}");
                    }
                }
            }
            catch
            {
                Debug.WriteLine("Не удалось отправить сообщение");
            }
}

        public static async void QuitanceRDY()
        {
            byte[] stx = STX;
            byte addr = 1;
            byte flc = 0b11000100;

            string num = await AppData.Properties.GetSerialNumber();
            byte[] n = new byte[num.Length + 2];
            for (int i = 0; i < num.Length; i++)
            {
                n[i] = (byte)num[i];
            }
            n[n.Length - 2] = addr;
            n[n.Length - 1] = flc;
            byte crc = GetCrc7(n);
            List<byte> data = new List<byte>();

            foreach (var b in stx)
            {
                data.Add(b);
            }
            data.Add(addr);
            data.Add(flc);
            //Debug.WriteLine($"{crc}");
            //data.Add(0x6d);
            data.Add(0x49);
            byte[] bufer = data.ToArray();

            SentToBLE(bufer);
        }
    }
}
