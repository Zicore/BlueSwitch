using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.SwitchMath
{
    public class AddSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            ActivateInputAdd(new PinDescription(typeof(double), UIType.TextEdit) { IsNumeric = true, IsDecimalNumber = true}, 2);

            AddOutput(typeof(double));
            Name = "Add";
            Description = "Add";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Math;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            double sum = 0;
            for (int i = 0; i < TotalInputs; i++)
            {
                sum += GetDataValueOrDefault<double>(i);
            }

            SetData(0, new DataContainer(sum));
        }
    }
}
