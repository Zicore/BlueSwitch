using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Diagnostics;
using BlueSwitch.Base.Trigger.Types;
using NLog;

namespace BlueSwitch.Runtime
{
    class Program
    {
        static void Main(string[] args)
        {
            RuntimeBase rt = new RuntimeBase();
            rt.Start(args);
        }
    }
}
