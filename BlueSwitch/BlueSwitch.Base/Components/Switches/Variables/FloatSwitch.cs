using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Variables
{
    public class FloatSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            TextEdit = new TextEdit();
            AddOutput(typeof(float), TextEdit);
            UniqueName = "BlueSwitch.Base.Components.Switches.Variables.FloatSwitch";
            DisplayName = "Float";
            Description = "A float variable";
            TextEdit.AllowDecimalPoint = true;
            TextEdit.NumberMode = true;
        }

        protected TextEdit TextEdit;

        public override GroupBase OnSetGroup()
        {
            return Groups.Variable;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            SetData(0, new DataContainer(Convert.ToSingle(TextEdit.NumberValue)));
            base.OnProcessData(p, node);
        }
    }
}
