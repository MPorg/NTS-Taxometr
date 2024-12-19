using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace Taxometr.Data.DataBase.Objects
{
    public class Logger
    {
        private string _path;
        private FileStream _stream;
        private List<string> _buffer = new List<string>();

        public Logger(string filePath)
        {
            _path = filePath;
            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                SaveLog();
                return true;
            });
        }

        public void SaveLog()
        {
            if (_buffer.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine($"Log write {_buffer.Count} lines");
                using (StreamWriter sw = new StreamWriter(new FileStream(_path, FileMode.Append, FileAccess.Write, FileShare.Write)))
                {
                    foreach (var b in _buffer)
                    {
                        sw.WriteLine(b);
                    }
                    sw.Close();
                    _buffer.Clear();
                }
            }
        }

        public void Log(string message)
        {
            _buffer.Add(message);
        }

    }
}
