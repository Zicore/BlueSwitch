using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Variables
{
    public class DoubleSwitch : TextEditBaseSwitch
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine); // Wichtig
            AddOutput(typeof(double), TextEdit);
            UniqueName = "BlueSwitch.Base.Components.Switches.Variables.Double";
            DisplayName = "Double";
            Description = "A double variable";
            TextEdit.AllowDecimalPoint = true;
            TextEdit.NumberMode = true;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            SetData(0, new DataContainer(Convert.ToDouble(TextEdit.NumberValue)));
            base.OnProcessData(p, node);
        }
    }
}
