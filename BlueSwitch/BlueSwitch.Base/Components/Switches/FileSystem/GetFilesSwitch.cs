using System.Collections;
using System.IO;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.FileSystem
{
    public class GetFilesSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.FileSystem;
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
