using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Variables;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class DateTimeSwitch : VariableSwitch
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);
            AddOutput(typeof(int));
            Name = "DateTime";
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.IO;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var t = DateTime.Now;
            SetData(0, new DataContainer {Value = t});
        }
    }
}
