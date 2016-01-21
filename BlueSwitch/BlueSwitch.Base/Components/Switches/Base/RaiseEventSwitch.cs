using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Trigger.Types;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class RaiseEventSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Trigger;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            Name = "Execute Event";

            Description = "Executes Event by given name";

            AddInput(new ActionSignature());
            AddInput(typeof (string), new TextEdit());

            AddOutput(new ActionSignature());
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p, node);

            var eventName = GetDataValueOrDefault<string>(1);

            RenderingEngine.EventManager.Run(EventTypeBase.StartSingle, eventName);
        }
    }
}
