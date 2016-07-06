using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Variables
{
    public abstract class TextEditBaseSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "Variable";

            TextEdit = new TextEdit();
            TextEdit.AutoResize = true;
        }

        protected TextEdit TextEdit;

        public override GroupBase OnSetGroup()
        {
            return Groups.Variable;
        }
    }
}
