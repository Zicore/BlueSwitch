using BlueSwitch.Base.Components.Switches.Base;
using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.SwitchMath.Double
{
    class MinSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Math;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            DisplayName = "Double.Min";
            UniqueName = "BlueSwitch.Base.Components.Switches.SwitchMath.Double.MinSwitch";
            Description = "Calculates the minimum of the given decimals.";

            ActivateInputAdd(new PinDescription(typeof(double), UIType.TextEdit) { IsDecimalNumber = true, IsNumeric = true }, 2);

            AddOutput(typeof(double));
        }

        protected override void OnInitializeMetaInformation(Engine engine)
        {
            engine.SearchService.AddTags(this, new string[] { "Math", "Calculation", "Double", "Decimal", "Min", "Minimum" });
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcessData<T>(p, node);

            double result = double.MaxValue;
            for (int i = 0; i < TotalInputs; i++)
            {
                double value = GetDataValueOrDefault<double>(i);
                result = Math.Min(value, result);
            }

            SetData(0, new DataContainer(result));
        }
    }
}
