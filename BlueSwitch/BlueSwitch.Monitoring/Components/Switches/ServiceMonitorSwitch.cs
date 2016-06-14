using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Monitoring.Components.Switches
{
    public class ServiceMonitorSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return BlueSwitch.Monitoring.Base.Groups.Monitoring;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            UniqueName = "Monitoring.Service.Status";
            DisplayName = "Service Status";
            Description = "Checks if service is running";

            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());

            AddInput(typeof (string), new TextEdit());

            AddOutput(typeof (bool));
            AddOutput(typeof (string));
        }

        public static ServiceController Find(String name)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (service.ServiceName == name)
                {
                    return service;
                }
            }
            return null;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p, node);

            var processName = GetDataValueOrDefault<string>(1);

            var service = Find(processName);

            SetData(1, new DataContainer(service != null));
            if (service != null)
            {
                SetData(2, new DataContainer(service.Status));
            }
        }
    }
}
