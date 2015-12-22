using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Variables
{
    public class Int32Switch : TextEditBaseSwitch
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine); // Wichtig
            AddOutput(typeof(int));
            Name = "Int32";
            TextEdit.AllowDecimalPoint = false;
            TextEdit.NumberMode = true;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            SetData(0, new DataContainer(Convert.ToInt32(TextEdit.NumberValue)));
            base.OnProcessData(p, node);
        }
    }
}
