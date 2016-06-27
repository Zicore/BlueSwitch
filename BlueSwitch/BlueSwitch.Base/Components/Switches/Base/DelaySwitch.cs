using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class DelaySwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.CodeFlow;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "Delay";
            Description = "Waits a given time";
            AddInput(new ActionSignature());
            AddInput(typeof(int), new TextEdit() { AllowDecimalPoint = false, NumberMode = true});
            AddOutput(new ActionSignature());

            IsCompact = true;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            var value = GetDataValueOrDefault<int>(1);
            base.OnProcess(p, node);

            p.Wait(new TimeSpan(0, 0, 0, 0, value));
        }
    }
}
