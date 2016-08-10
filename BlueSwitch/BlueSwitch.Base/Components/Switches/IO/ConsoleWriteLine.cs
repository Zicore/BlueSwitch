﻿using System;
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
            return Groups.IO;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.IO.ConsoleWriteLine";
            DisplayName = "Console.WriteLine";
            Description = "Writes a line into the console.";

            AddInput(new ActionSignature());

            AddInput(typeof(string));

            AddOutput(new ActionSignature());

            IsCompact = true;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetDataValueOrDefault<string>(1);
            Console.WriteLine(data);
            base.OnProcess(p, node);
        }
    }
}
