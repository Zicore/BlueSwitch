using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Variables
{
    public class DoubleSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            TextEdit = new TextEdit();
            AddOutput(typeof(double), TextEdit);
            UniqueName = "BlueSwitch.Base.Components.Switches.Variables.Double";
            DisplayName = "Double";
            Description = "A double variable";
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
            SetData(0, new DataContainer(Convert.ToDouble(TextEdit.NumberValue)));
            base.OnProcessData(p, node);
        }
    }
}
