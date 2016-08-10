using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Logic.Int32
{
    public class LessThanSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            AddInput(typeof(int), new TextEdit { NumberMode = true, AllowDecimalPoint = false });
            AddInput(typeof(int), new TextEdit { NumberMode = true, AllowDecimalPoint = false });
            AddOutput(typeof(bool));

            UniqueName = "BlueSwitch.Base.Components.Switches.Logic.Int32.Int32.LessThan";
            DisplayName = "Int32.LessThan";
            Description = "Checks if Int32(x) is less than Int32(y)";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.LogicInt32;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var a = GetData<int>(0);
            var b = GetData<int>(1);

            if (a != null && b != null)
            {
                SetData(0, new DataContainer((int)a.Value < (int)b.Value));
            }

            base.OnProcess(p, node);
        }
    }
}
