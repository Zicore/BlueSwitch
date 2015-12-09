using BlueSwitch.Renderer.Components.Base;
using BlueSwitch.Renderer.Components.Switches.Base;
using BlueSwitch.Renderer.Components.Types;
using BlueSwitch.Renderer.Processing;

namespace BlueSwitch.Renderer.Components.Switches.CodeFlow
{
    public class ConditionSwitch : SwitchBase
    {
        public ConditionSwitch()
        {
            
        }

        protected override void OnInitialize(RenderingEngine engine)
        {
            Name = "Condition";

            AddInput(new ActionSignature());
            AddInput(typeof(bool));

            AddOutput(new ActionSignature());
            AddOutput(new ActionSignature());
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.CodeFlow;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetData(1);

            if (data?.Value == null || !(bool) data.Value)
            {
                node.Skip = new SkipNode(0);
            }
            else
            {
                node.Skip = new SkipNode(1);
            }
            base.OnProcess(p, node);
        }
    }
}
