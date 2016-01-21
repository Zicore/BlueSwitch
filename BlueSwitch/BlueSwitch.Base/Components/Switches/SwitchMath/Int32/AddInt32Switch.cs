using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.SwitchMath.Int32
{
    public class AddInt32Switch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            ActivateInputAdd(new PinDescription(typeof(int), UIType.TextEdit) { IsNumeric = true, IsDecimalNumber = false, IsAutoStoreValue = true}, 2);

            AddOutput(typeof(int));
            Name = "Add.Int32";
            Description = "Add.Int32";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Math;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            int sum = 0;
            for (int i = 0; i < TotalInputs; i++)
            {
                sum += GetDataValueOrDefault<int>(i);
            }

            SetData(0, new DataContainer(sum));
        }
    }
}
