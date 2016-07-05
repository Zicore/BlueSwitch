using System.Collections;
using System.IO;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.FileSystem
{
    public class OpenFileSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.FileSystem;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.FileSystem.OpenFile";
            DisplayName = "Open File";

            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());

            AddInput(typeof (string));
            AddOutput(typeof (FileHandle));

            IsCompact = true;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var path = GetDataValueOrDefault<string>(1);

            FileHandle handle = new FileHandle();
            handle.Open(path);

            SetData(1,handle);

            base.OnProcess(p, node);
        }
    }
}
