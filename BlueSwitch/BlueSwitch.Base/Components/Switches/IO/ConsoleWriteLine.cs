using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.IO
{
    public class ConsoleWriteLineSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return GroupBase.IO;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            Name = "ConsoleWriteLine";
            ColumnWidth = 100;
            AddInput(new ActionSignature());

            AddInput(typeof(string));

            AddOutput(new ActionSignature());
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetDataValueOrDefault<string>(1);
            Console.WriteLine(data);
            base.OnProcess(p, node);
        }
    }
}
