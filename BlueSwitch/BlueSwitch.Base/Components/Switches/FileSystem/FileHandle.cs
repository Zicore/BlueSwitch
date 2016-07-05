using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Switches.FileSystem
{
    public class FileHandle
    {
        [JsonIgnore]
        public String FilePath { get; set; }

        [JsonIgnore]
        public FileStream Stream { get; private set; }

        public void Open(String filePath)
        {
            this.FilePath = filePath;
            Close(); // Close existing stream before load in this object to avoid loosing this handle
            Stream = File.Open(FilePath, FileMode.Open);
        }
        
        public void Close()
        {
            Stream?.Dispose();
            Stream?.Close();
            Stream = null;
        }
    }
}
