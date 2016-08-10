using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.IO
{
    public class ConsoleReadLine : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.IO;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.IO.ConsoleReadLine";
            DisplayName = "ConsoleReadLine";
            Description = "Reads a line from the console.";

            AddInput(new ActionSignature());
            AddInput(typeof(string));

            AddOutput(new ActionSignature());
            AddOutput(typeof(string));
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetData(1);
            if (data != null)
            {
                Console.WriteLine(data.Value);
            }
            SetData(1, new DataContainer(Console.ReadLine()));

            base.OnProcess(p, node);
        }
    }
}
