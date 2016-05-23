using System;
using System.Collections;
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

namespace BlueSwitch.Base.Components.Switches.IO
{
    public class GetFilesSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.IO;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "GetFiles";
            
            AddInput(typeof (string), new TextEdit());
            AddOutput(typeof(IEnumerable));
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var path = GetDataValueOrDefault<string>(0);

            var paths = Directory.GetFiles(path);

            SetData(0, new DataContainer(paths));

            base.OnProcess(p, node);
        }
    }
}
