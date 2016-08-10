using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Meta.Search;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.SwitchMath.Int32
{
    public class AddSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine engine)
        {
            ActivateInputAdd(new PinDescription(typeof(int), UIType.TextEdit) { IsNumeric = true, IsDecimalNumber = false, IsAutoStoreValue = true}, 2);

            AddOutput(typeof(int));
            UniqueName = "Int32.Add";
            Description = "Calculates the sum of all integer inputs";
            DisplayName = "Int32.Add";
        }

        protected override void OnInitializeMetaInformation(Engine engine)
        {
            engine.SearchService.AddTags(this, new string[] { "Math", "Calculation", "Int32", "Add" });
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
