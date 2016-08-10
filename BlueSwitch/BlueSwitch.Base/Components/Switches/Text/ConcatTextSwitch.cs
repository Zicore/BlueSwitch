using System.Text;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Text
{
    public class ConcatTextSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            ActivateInputAdd(new PinDescription(typeof(string), UIType.TextEdit) {}, 2);

            AddOutput(typeof(string));
            UniqueName = "BlueSwitch.Base.Components.Switches.Text.Concat";
            DisplayName = "Concat";
            Description = "Concatenate strings together";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Text;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < TotalInputs; i++)
            {
                sb.Append(GetDataValueOrDefault<string>(i));
            }

            SetData(0, new DataContainer(sb.ToString()));
        }
    }
}
