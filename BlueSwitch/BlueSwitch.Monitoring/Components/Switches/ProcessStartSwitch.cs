using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class ProcessStartSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return BlueSwitch.Monitoring.Base.Groups.Monitoring;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            UniqueName = "Monitoring.Process.Start";
            DisplayName = "Start Process";
            Description = "Starts the process";

            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());

            AddInput(typeof (string), new TextEdit());
            AddInput(typeof (string), new TextEdit());
        }

        protected override void OnInitializeMetaInformation(Engine engine)
        {
            engine.SearchService.AddTags(this, new [] {"Process", "Start", "Monitoring"});
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p, node);

            var path = GetDataValueOrDefault<string>(1);
            var arguments = GetDataValueOrDefault<string>(2);
            if (!String.IsNullOrEmpty(arguments))
            {
                System.Diagnostics.Process.Start(path, arguments);
            }
            else
            {
                System.Diagnostics.Process.Start(path);
            }
        }
    }
}
