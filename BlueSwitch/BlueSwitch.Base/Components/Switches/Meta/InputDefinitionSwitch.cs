using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;

namespace BlueSwitch.Base.Components.Switches.Meta
{
    public class InputDefinitionSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Meta;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            HasVariableOutputs = true;
            ExtraRows = 1;

            AddOutput(new ActionSignature());

            ActivateOutputAdd(new PinDescription(new AnySignature(), UIType.None), 2);

            UniqueName = "BlueSwitch.Base.Components.Switches.Meta.InputDefinition";
            DisplayName = "Input";
            Description = "Defines the start of a prefabrication";
        }
    }
}
