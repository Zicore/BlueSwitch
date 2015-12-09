using System;
using BlueSwitch.Renderer.Components.Base;
using BlueSwitch.Renderer.Components.Switches.Base;
using BlueSwitch.Renderer.Processing;

namespace BlueSwitch.Renderer.Components.Switches.IO
{
    public class Input2 : SwitchBase
    {
        protected override void OnInitialize()
        {
            AddOutput(typeof (string));
            Name = "StaticText";
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.IO;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            SetData(0,new DataContainer("Enter Value:"));
        }
    }
}
