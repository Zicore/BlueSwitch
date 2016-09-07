using System;
using System.Text;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Text
{
    public class SplitTextSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            AddInput(typeof(string), new TextEdit());
            AddInput(typeof(string), new TextEdit());

            AddOutput(typeof(string[]));

            UniqueName = "BlueSwitch.Base.Components.Switches.Text.Split";
            DisplayName = "Split";
            Description = "Splits strings and returns array";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Text;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var text = GetDataValueOrDefault<string>(0);
            var splitter = GetDataValueOrDefault<string>(1);

            var result = text.Split(new string[] {splitter}, StringSplitOptions.RemoveEmptyEntries);
            SetData(0, result);
        }
    }
}
