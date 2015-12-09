using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.CodeFlow
{
    public class SequenceSwitch : SwitchBase
    {
        protected override void OnInitialize(RenderingEngine engine)
        {
            Name = "Sequence";

            AddInput(new ActionSignature());

            AddOutput(new ActionSignature());
            AddOutput(new ActionSignature());
            AddOutput(new ActionSignature());
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.CodeFlow;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p, node);
        }
    }
}
