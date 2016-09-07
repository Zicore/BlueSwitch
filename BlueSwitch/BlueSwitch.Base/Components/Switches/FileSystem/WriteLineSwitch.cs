using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.FileSystem
{
    public class WriteLineSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.FileSystem;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.FileSystem.WriteLine";
            DisplayName = "Write Line";
            Description = "Writes a line to a file.";

            AddInput(new ActionSignature());
            AddInput(typeof(string));
            AddInput(typeof(string));
            AddOutput(new ActionSignature());

            IsCompact = true;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            string filePath = GetDataValueOrDefault<string>(1);
            using (StreamWriter sw = File.AppendText(filePath))
            {
                var line = GetDataValueOrDefault<string>(2);
                sw.WriteLine(line);
            }
            base.OnProcess(p, node);
        }
    }
}
