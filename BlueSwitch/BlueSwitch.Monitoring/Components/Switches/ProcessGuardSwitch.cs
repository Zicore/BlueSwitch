using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Monitoring.Components.Switches
{
    public class ProcessGuardSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return BlueSwitch.Monitoring.Base.Groups.Monitoring;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            UniqueName = "BlueSwitch.Monitoring.Components.Switches.Process.Guard";
            DisplayName = "Process Guard";
            Description = "Checks if process is running and starts it new if it's not";

            AddInput(new ActionSignature());
            AddInput(typeof(bool), new CheckBox()); // 1
            AddInput(typeof(int), TextEdit.CreateNumeric(false)); // 2
            AddInput(typeof(string), new TextEdit()); // 3 Process Name
            AddInput(typeof(string), new TextEdit()); // 4 Pfad
            AddInput(typeof(string), new TextEdit()); // 5 Args

            AddOutput(new ActionSignature());
        }
        
        bool timeoutStarted = false;
        DateTime firstTime = DateTime.Now;

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p, node);

            const int sleepStep = 100;
            node.Value.ProcessData(p, node);

            var isRunning = GetDataValueOrDefault<bool>(1);
            var timeoutMax = GetDataValueOrDefault<int>(2);
            var processName = GetDataValueOrDefault<string>(3);
            var processPath = GetDataValueOrDefault<string>(4);
            var processArgs = GetDataValueOrDefault<string>(5);
            var processes = System.Diagnostics.Process.GetProcesses();
            bool isProcessAlive = processes.Any(x => x.ProcessName == processName);

            if (!isProcessAlive)
            {
                if (!timeoutStarted)
                {
                    firstTime = DateTime.Now;
                    timeoutStarted = true;
                }

                if (firstTime + new TimeSpan(0, 0, 0, 0, timeoutMax) < DateTime.Now)
                {
                    if (string.IsNullOrEmpty(processArgs))
                    {
                        System.Diagnostics.Process.Start(processPath);
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(processPath, processArgs);
                    }
                    timeoutStarted = false;
                }
            }

            p.Wait(new TimeSpan(0, 0, 0, 0, sleepStep));
            if (isRunning && !p.IsCancellationRequested)
            {
                node.Repeat = true;
            }
        }
    }
}
