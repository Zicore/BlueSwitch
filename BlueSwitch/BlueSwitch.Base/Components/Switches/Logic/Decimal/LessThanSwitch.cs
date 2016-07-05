using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Logic.Decimal
{
    public class LessThanSwitch : SwitchBase
    {
        public LessThanSwitch()
        {
            
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            AddInput(typeof(decimal));
            AddInput(typeof(decimal));
            AddOutput(typeof(bool));

            UniqueName = "Decimal.LessThan";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.LogicDecimal;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var a = GetData(0);
            var b = GetData(1);

            if (a != null && b != null)
            {
                SetData(0, new DataContainer((decimal)a.Value < (decimal)b.Value));
            }

            base.OnProcess(p, node);
        }
    }
}
