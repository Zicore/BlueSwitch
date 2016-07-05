using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSwitch.Base.Components.Switches.FileSystem
{
    public class FileReader
    {
        private StreamReader _reader;

        private void Open(FileHandle handle)
        {
            if (_reader == null)
            {
                _reader = new StreamReader(handle.Stream);
            }
        }

        public string ReadLine(FileHandle handle)
        {
            Open(handle);
            return _reader.ReadLine();
        }

        public void Close(FileHandle handle)
        {
            Open(handle);
            _reader.Close();
            handle.Close();
            _reader = null;
        }

        public bool EndOfFile(FileHandle handle)
        {
            Open(handle);
            return _reader.EndOfStream;
        }
    }
}
