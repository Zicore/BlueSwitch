using System;
using BlueSwitch.Renderer.Components.Base;
using BlueSwitch.Renderer.Components.Types;
using BlueSwitch.Renderer.Processing;

namespace BlueSwitch.Renderer.Components.Switches.Base
{
    public class DateTimeCalculatorSwitch : SwitchBase
    {
        protected override void OnInitialize(RenderingEngine engine)
        {
            AddInput(new InputBase(new AnySignature()));
            AddOutput(new OutputBase(new SignatureSingle(typeof(DateTime))));
            AddOutput(new OutputBase(new SignatureSingle(typeof(DateTime))));
            Name = "DateTime";
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.IO;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            Console.WriteLine(Name);

            var t = DateTime.Now;
            Outputs[0].Data = new DataContainer { Value = t };
            Outputs[1].Data = new DataContainer { Value = t};
        }
    }
}
