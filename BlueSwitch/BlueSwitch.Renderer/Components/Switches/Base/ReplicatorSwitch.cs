using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class ReplicatorSwitch : SwitchBase
    {
        protected override void OnInitialize(RenderingEngine engine)
        {
            AddInput(new InputBase(new AnySignature()));

            AddOutput(new OutputBase(new AnySignature()));
            AddOutput(new OutputBase(new AnySignature()));
            AddOutput(new OutputBase(new AnySignature()));
            AddOutput(new OutputBase(new AnySignature()));

            Name = "Replicator";
            Description = "Setzt Index 0 Input in alle Outputs";
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p,node);
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            foreach (var o in Outputs)
            {
                SetData(o.Index, GetData(0));
            }

            base.OnProcessData(p, node);
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.Base;
        }
    }
}
