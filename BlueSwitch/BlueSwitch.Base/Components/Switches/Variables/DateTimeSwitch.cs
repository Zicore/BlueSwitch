using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Variables
{
    public class DateTimeSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);
            
            AddOutput(typeof(DateTime));
            UniqueName = "DateTime";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.IO;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var result = DateTime.Now;
            SetData(0, new DataContainer {Value = result });
        }
    }
}
