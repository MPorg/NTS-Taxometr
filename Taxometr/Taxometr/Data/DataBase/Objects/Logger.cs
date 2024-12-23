using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace Taxometr.Data.DataBase.Objects
{
    public class Logger
    {
        private string _path;
        private List<string> _buffer = new List<string>();

        public Logger(string filePath)
        {
            _path = filePath;
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                SaveLog();
                return true;
            });
        }

        public void SaveLog()
        {
            List<string> buffer = new List<string>(_buffer);

            try
            {
                if (buffer.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Log write {buffer.Count} lines");
                    using (StreamWriter sw = new StreamWriter(new FileStream(_path, FileMode.Append, FileAccess.Write, FileShare.Write)))
                    {
                        foreach (var b in buffer)
                        {
                            sw.WriteLine(b);
                        }
                        sw.Close();
                        _buffer.RemoveRange(0, buffer.Count);
                        buffer.Clear();
                        if (_buffer.Count > 0)
                        {
                            SaveLog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = $"Не удалось сохранить лог. Системная ошибка: {ex.Message}";
                System.Diagnostics.Debug.WriteLine(msg);
                using (StreamWriter sw = new StreamWriter(new FileStream(_path, FileMode.Append, FileAccess.Write, FileShare.Write)))
                {
                    sw.WriteLine(msg);
                    sw.Close();
                }
            }
        }

        public void Log(string message)
        {
            _buffer.Add(message);
        }

    }
}
