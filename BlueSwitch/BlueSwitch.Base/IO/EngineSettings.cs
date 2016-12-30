using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using Newtonsoft.Json;

namespace BlueSwitch.Base.IO
{
    public class EngineSettings : JsonSerializable
    {
        static EngineSettings()
        {
            SettingsDirectoryPath = JsonSerializable.GetDirectoryPath(ApplicationName);
            SettingsFilePath = JsonSerializable.GetFilePath("BlueSwitch", "Settings.json");
        }

        public PerformanceMode PerformanceMode { get; set; } = PerformanceMode.HighQuality;

        public int SnapToGridWidth { get; set; } = 10;
        public bool SnapToGridEnabled { get; set; } = true;

        public bool DrawGrid { get; set; } = true;
        public bool DrawSubGrid { get; set; } = true;

        [JsonIgnore]
        public static string SettingsFilePath;

        [JsonIgnore]
        public static string SettingsDirectoryPath;
        
        [JsonIgnore]
        public static string ApplicationName = "BlueSwitch";
    }
}