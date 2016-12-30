using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;

namespace BlueSwitch.Base.Components.Switches.Meta
{
    public class OutputDefinitionSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Meta;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            HasVariableInputs = true;
            ExtraRows = 1;

            AddInput(new ActionSignature());

            ActivateInputAdd(new PinDescription(new AnySignature(), UIType.None), 2);

            UniqueName = "BlueSwitch.Base.Components.Switches.Meta.OutputDefinition";
            DisplayName = "Output";
            Description = "Defines the end of a prefabrication";
        }
    }
}
