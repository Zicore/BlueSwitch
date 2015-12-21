using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSwitch.Base.Components.Switches.Debug
{
    public class DownloadInfo
    {
        public int Status { get; set; }
        public bool IsDownloading { get; set; }

        public override string ToString()
        {
            return $"Status: {Status}";
        }
    }
}
