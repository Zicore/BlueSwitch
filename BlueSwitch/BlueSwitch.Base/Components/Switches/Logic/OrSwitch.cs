using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Logic
{
    public class OrSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            ActivateInputAdd(new PinDescription(typeof(bool), UIType.CheckBox), 2);

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
            bool result = false;
            for (int i = 0; i < TotalInputs; i++)
            {
                result = result || GetDataValueOrDefault<bool>(i);
            }

            SetData(0, new DataContainer(result));

            base.OnProcess(p, node);
        }
    }
}
