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
    public class CloseFileSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.FileSystem;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.FileSystem.CloseFileSwitch";
            DisplayName = "Close File";

            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());

            AddInput(typeof(FileHandle));

            IsCompact = true;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var handle = GetDataValueOrDefault<FileHandle>(1);

            handle.Close();
            
            base.OnProcess(p, node);
        }
    }
}
