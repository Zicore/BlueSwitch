using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Variables
{
    public class BooleanSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine); // Wichtig
            _checkBox = new CheckBox();
            AddOutput(typeof(bool));
            Components.Add(_checkBox);
            UniqueName = "Boolean";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Variable;
        }

        CheckBox _checkBox;

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            SetData(0, new DataContainer(_checkBox.Value));
            base.OnProcessData(p, node);
        }
    }
}
