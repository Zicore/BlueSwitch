using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Trigger.Types;
using NLog;

namespace BlueSwitch.Runtime
{
    class Program
    {
       private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Log.Debug("Starting BlueSwitch.Runtime");
            Engine engine = new RuntimeEngine();
            engine.DebugMode = false;
            
            engine.LoadAddons();

            String filePath = @"C:\Users\avit\scenario.bs.json";
            engine.LoadProject(filePath);

            engine.CompileAndStart();
            Console.ReadLine();
        }
    }
}
