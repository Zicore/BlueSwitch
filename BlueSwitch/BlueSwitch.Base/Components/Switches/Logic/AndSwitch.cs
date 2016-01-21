using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Logic
{
    public class AndSwitch : SwitchBase
    {
        public AndSwitch()
        {
            
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            HasVariableInputs = true;
            ExtraRows = 1;
            
            AddOutput(typeof(bool));

            ActivateInputAdd(new PinDescription(typeof(bool), UIType.CheckBox), 2);

            Name = "And";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Logic;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            bool result = true;
            for (int i = 0; i < TotalInputs; i++)
            {
                result = result && GetDataValueOrDefault<bool>(i);
            }

            SetData(0, new DataContainer(result));

            base.OnProcess(p, node);
        }
    }
}
