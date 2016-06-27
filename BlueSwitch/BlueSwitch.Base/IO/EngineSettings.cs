using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;

namespace BlueSwitch.Base.IO
{
    public class EngineSettings : JsonSerializable
    {
        public PerformanceMode PerformanceMode { get; set; } = PerformanceMode.HighQuality;

        public int SnapToGridWidth { get; set; } = 10;
        public bool SnapToGridEnabled { get; set; } = true;

        public bool DrawGrid { get; set; } = true;
        public bool DrawSubGrid { get; set; } = true;
    }
}