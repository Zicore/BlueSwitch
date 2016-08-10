using BlueSwitch.Base.Components.Switches.Base;
using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.SwitchMath.Int32
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

            DisplayName = "Int32.Min";
            UniqueName = "BlueSwitch.Base.Components.Switches.SwitchMath.Int32.MinSwitch";
            Description = "Calculates the minimum of the given integers";

            ActivateInputAdd(new PinDescription(typeof(int), UIType.TextEdit) { IsDecimalNumber = false, IsNumeric = true },2);

            AddOutput(typeof(int));
        }

        protected override void OnInitializeMetaInformation(Engine engine)
        {
            engine.SearchService.AddTags(this, new string[] { "Math", "Calculation", "Min", "Minimum", "Int32" });
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcessData<T>(p, node);

            int result = int.MaxValue;
            for(int i = 0; i < TotalInputs; i++)
            {
                int value = GetDataValueOrDefault<int>(i);
                result = Math.Min(value, result);
            }

            SetData(0, new DataContainer(result));
        }
    }
}
