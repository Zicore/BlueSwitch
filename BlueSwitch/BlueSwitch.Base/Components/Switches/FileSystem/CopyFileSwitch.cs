using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.FileSystem
{
    public class CopyFileSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "CopyFile";

            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());
            
            AddInput(typeof (string), new TextEdit());
            AddInput(typeof(string), new TextEdit());
            AddInput(typeof (bool), new CheckBox());

            base.OnInitialize(renderingEngine);
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p, node);

            string source = GetDataValueOrDefault<string>(1);
            string destination = GetDataValueOrDefault<string>(2);
            bool overwrite = GetDataValueOrDefault<bool>(3);

            File.Copy(source,destination, overwrite);
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.File;
        }
    }
}
