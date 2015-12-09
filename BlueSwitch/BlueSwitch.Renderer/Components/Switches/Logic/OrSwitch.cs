using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Logic
{
    public class OrSwitch : SwitchBase
    {
        protected override void OnInitialize(RenderingEngine engine)
        {
            AddInput(typeof(bool));
            AddInput(typeof(bool));
            AddOutput(typeof(bool));
            Name = "Or";
            Description = "Or Gate";
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.Logic;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var a = GetData(0);
            var b = GetData(1);

            if (a != null && b != null)
            {
                SetData(0, new DataContainer((bool)a.Value | (bool)b.Value));
            }

            base.OnProcess(p, node);
        }
    }
}
