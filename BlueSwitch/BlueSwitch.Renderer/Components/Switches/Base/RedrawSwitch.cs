using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class RedrawSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return GroupBase.Base;
        }

        protected override void OnInitialize(RenderingEngine engine)
        {
            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());
            Name = "Redraw";
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            p.RenderingEngine.RequestRedraw();
            base.OnProcess(p, node);
        }
    }
}
