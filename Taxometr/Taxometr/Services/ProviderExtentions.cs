using Honoo.IO.Hashing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Xamarin.Forms.Xaml;

namespace Taxometr.Services
{
    internal static class ProviderExtentions
    {
        #region BLE UUID
        internal static Guid BLUETOOTH_LE_INCOTEX_SERVICE = new Guid("0000fff0-0000-1000-8000-00805f9b34fb");
        internal static Guid BLUETOOTH_LE_INCOTEX_CHAR_R = new Guid("0000fff1-0000-1000-8000-00805f9b34fb");
        internal static Guid BLUETOOTH_LE_INCOTEX_CHAR_W = new Guid("0000fff2-0000-1000-8000-00805f9b34fb");
        #endregion

        #region SYS CMD
        internal static byte ADDR = 0x01;
        internal static byte STX = 0x02;
        internal static byte ETX = 0x03;
        internal static byte DLE = 0x10;
        internal static byte PREFIX = 0x7F;
        #endregion

        #region CMD
        internal const byte TaxInfo = 0x01;
        internal const byte TaxDateTime = 0x04;
        internal const byte BtnEmit = 0x05;
        internal const byte TaxState = 0x11;
        internal const byte FrRead = 0x19;
        internal const byte SwitchMode = 0x20;
        internal const byte CashDeposWithdraw = 0x21;
        internal const byte CheckOpen = 0x22;
        internal const byte CheckClose = 0x23;
        internal const byte CheckBreak = 0x24;
        internal const byte CheckDuplicate = 0x25;
        internal const byte ShiftState = 0x29;
        internal const byte ShiftOpen = 0x30;
        internal const byte ScnoState = 0x35;
        internal const byte VmuState = 0x36;
        #endregion

        internal static byte CRC7(string serialNumber, params byte[] data)
        {
            byte[] dataArr = data.ToArray();

            Crc crc = Crc.Create(CrcName.CRC7);

            for (int i = 0; i < serialNumber.Length; i++)
            {
                crc.Update((byte)serialNumber[i]);
            }

            if (data.Length > 0)
            {
                foreach (byte b in dataArr)
                {
                    crc.Update(b);
                }
            }
            crc.ComputeFinal(out byte result);
            return result;
        }

        internal static byte[] CRC16(string serialNumber, params byte[] data)
        {
            Crc crc = Crc.Create(CrcName.CRC16_CCITT_FALSE);

            for (int i = 0; i < serialNumber.Length; i++)
            {
                crc.Update((byte)serialNumber[i]);
            }

            if (data.Length > 0)
            {
                foreach (byte b in data)
                {
                    crc.Update(b);
                }
            }

            byte[] result = crc.ComputeFinal(CrcEndian.LittleEndian);

            return result;
        }

        internal static void CombineFlc(this ref byte flc)
        {
            byte footer = 0b100;
            flc ^= footer;
            byte rndNum = (byte)new Random().Next(0, 4);
            flc ^= rndNum;
        }

        internal static byte CombineFlc(byte flc)
        {
            byte footer = 0b100;
            flc ^= footer;
            byte rndNum = 0b01; //(byte)new Random().Next(0, 4);
            flc ^= rndNum;
            return flc;
        }

        internal static byte[] CombineData(byte[] data, string serialNumber)
        {
            byte[] result = new byte[data.Length + 5];
            result[0] = 0x00;
            byte[] crc = CRC16(serialNumber, data);


            for (int i = 0; i < data.Length; i++)
            {
                result[i + 1] = data[i];
            }

            result[result.Length - 4] = PREFIX;
            result[result.Length - 3] = ETX;

            result[result.Length - 2] = crc[0];
            result[result.Length - 1] = crc[1];

            return result;
        }

        /// <summary>
        /// returns encoded data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="rnd"></param>
        /// <returns></returns>

        internal static byte[] RC4(this byte[] data, string key, int rnd)
        {

            data = Rc4Cipher(Rc4Init(key), rnd, data);

            return data;
        }

        private static RC4Context Rc4Init(string key)
        {
            int length = key.Length;

            RC4Context context = new RC4Context(0, 0, 256);

            for (int i = 0; i < 256; i++)
                context.s[i] = i;

            for (int i = 0, j = 0; i < 256; i++)
            {
                //Randomize the permutations using the supplied key
                j = (j + context.s[i] + key[i % length]) % 256;
                //Swap the values of S[i] and S[j]
                (context.s[j], context.s[i]) = (context.s[i], context.s[j]);
            }
            return context;
        }

        private static byte[] Rc4Cipher(RC4Context context, int randomOffset, byte[] input)
        {
            int length = input.Length;

            int i = (context.i + (randomOffset & 0xFF)) % 256;
            int j = (context.j + (randomOffset & 0xFF)) % 256;

            int[] s = context.s;

            int p = 0;
            byte[] output = new byte[length];
            while (length > 0)
            {
                //Adjust indices
                i = (i + 1) % 256;
                j = (j + s[i]) % 256;
                //Swap the values of S[i] and S[j]
                (s[i], s[j]) = (s[j],  s[i]);
                //Encrypt/decrypt data
                output[p] = (byte)(input[p] ^ s[(s[i] + s[j]) % 256]);
                //Increment data pointers
                p++;
                //Remaining bytes to process
                length--;
            }

            context.i = i;
            context.j = j;

            return output;
        }

        internal static string FormatRight(this string str, int width)
        {
            str = str.Trim((char)0x00);
            if (String.IsNullOrEmpty(str) || str.Length >= width)
            {
                return str;
            }
            else
            {
                string space = "";
                for (int i = 0; i < width - str.Length; i++)
                {
                    space += " ";
                }

                space += str;
                return space;
            }
        }

        internal static string FormatMiddle(this string str, int width)
        {
            str = str.Trim((char)0x00);
            if (String.IsNullOrEmpty(str) || str.Length >= width)
            {
                return str;
            }
            else
            {
                string space = "";
                for (int i = 0; i < (width - str.Length) / 2f; i++)
                {
                    space += " ";
                }

                space += str;
                return space;
            }
        }
    }

    internal struct RC4Context
    {
        public int i;
        public int j;
        public int[] s;

        public RC4Context(int i = 0, int j = 0, int length = 256)
        {
            this.i = i;
            this.j = j;
            s = new int[length];
        }
    }
}
