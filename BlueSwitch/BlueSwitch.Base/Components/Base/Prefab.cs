using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.IO;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Base
{
    public class Prefab
    {
        public BlueSwitchProject Project { get; set; }
        
        public String FilePath { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
    }
}
