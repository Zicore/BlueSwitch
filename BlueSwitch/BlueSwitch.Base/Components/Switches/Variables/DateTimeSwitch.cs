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

            AddInput(typeof (string), new TextEdit());
            AddOutput(typeof(string));
            Name = "DateTime";
            ColumnWidth = 120;
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.IO;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var format = GetDataValueOrDefault<string>(0);
            var result = DateTime.Now.ToString(format);
            SetData(0, new DataContainer {Value = result });
        }
    }
}
