using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class RestartSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.CodeFlow;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.Base.Restart";
            DisplayName = "Restart";
            Description = "Marks for restart";

            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());

            IsCompact = true;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            p.MarkForRestart();
            base.OnProcess(p, node);
        }
    }
}
