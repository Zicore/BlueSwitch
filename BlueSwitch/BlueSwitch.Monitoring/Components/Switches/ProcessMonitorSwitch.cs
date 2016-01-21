using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Monitoring.Components.Switches
{
    public class ProcessMonitorSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return BlueSwitch.Monitoring.Base.Groups.Monitoring;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            Name = "Process Running";
            Description = "Checks if process is running";

            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());

            AddInput(typeof (string), new TextEdit());
            AddOutput(typeof (bool));
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p, node);

            var processName = GetDataValueOrDefault<string>(1);

            var processes = System.Diagnostics.Process.GetProcesses();

            bool isProcessAlive = processes.Any(x => x.ProcessName.Contains(processName));

            SetData(1, new DataContainer(isProcessAlive));
        }
    }
}
