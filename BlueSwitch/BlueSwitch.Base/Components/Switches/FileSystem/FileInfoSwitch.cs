using System.Collections;
using System.IO;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.FileSystem
{
    public class FileInfoSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.FileSystem;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "FileInfo";
            DisplayName = "File Info";

            AddInput(typeof (string), new TextEdit());
            AddOutput(typeof(string));
            AddOutput(typeof(string));
            AddOutput(typeof(string));
            AddOutput(typeof(string));
            AddOutput(typeof(string));
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var path = GetDataValueOrDefault<string>(0);

            FileInfo fi = new FileInfo(path);

            SetData(0,new DataContainer(fi.FullName));
            SetData(1, new DataContainer(fi.Name));
            SetData(2, new DataContainer(fi.Directory.FullName));
            SetData(3, new DataContainer(fi.Directory.Name));
            SetData(4, new DataContainer(fi.Extension));

            base.OnProcess(p, node);
        }
    }
}
