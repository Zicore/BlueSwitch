using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;

namespace BlueSwitch.Monitoring.Base
{
    public static class Groups
    {
        public static readonly GroupBase Monitoring = new GroupBase { Name = "Monitoring", DisplayName = "Monitoring" };
    }
}
