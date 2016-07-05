using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.FileSystem
{
    public class ReadLineSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.FileSystem;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.FileSystem.ReadLine";
            DisplayName = "Read Line";

            AddInput(new ActionSignature());

            AddInput(typeof(FileHandle));

            AddOutput(new ActionSignature());
            AddOutput(typeof(bool));
            AddOutput(typeof(string));

            IsCompact = true;
        }

        FileReader reader = new FileReader();

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var handle = GetDataValueOrDefault<FileHandle>(1);
            string line = null;
            if (!reader.EndOfFile(handle))
            {
                line = reader.ReadLine(handle);
            }

            var eof = reader.EndOfFile(handle);
            
            SetData(1, eof);
            SetData(2, line);

            base.OnProcess(p, node);
        }
    }
}
