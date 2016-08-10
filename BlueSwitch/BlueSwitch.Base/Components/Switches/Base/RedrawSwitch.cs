using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class RedrawSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Base;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());
            UniqueName = "BlueSwitch.Base.Components.Switches.Base.Redraw";
            DisplayName = "Redraw";
            Description = "Redraw";
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            p.RenderingEngine.RequestRedraw();
            base.OnProcess(p, node);
        }
    }
}
