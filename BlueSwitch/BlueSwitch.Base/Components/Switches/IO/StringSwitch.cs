using System;
using System.Drawing;
using BlueSwitch.Renderer.Components.Base;
using BlueSwitch.Renderer.Components.Switches.Base;
using BlueSwitch.Renderer.Components.UI;
using BlueSwitch.Renderer.Processing;

namespace BlueSwitch.Renderer.Components.Switches.IO
{
    public class StringSwitch : SwitchBase
    {
        protected override void OnInitialize(RenderingEngine engine)
        {
            AddOutput(typeof (string));
            Name = "String";

            Value = "#OPDWIN";

            
            Components.Add(new TextEdit());
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.IO;
        }

        public String Value { get; set; } = String.Empty;
        
        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            if (Value != null)
            {
                SetData(0, new DataContainer(Value));
            }
        }
    }
}
