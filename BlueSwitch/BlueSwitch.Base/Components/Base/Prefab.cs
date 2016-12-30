using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Switches.Base;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Base
{
    public class Prefab
    {
        [JsonProperty("Items")]
        public List<SwitchBase> Items { get; set; } = new List<SwitchBase>();

        [JsonProperty("Connections")]
        public List<Connection> Connections { get; set; } = new List<Connection>();
    }
}
