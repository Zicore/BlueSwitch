using System;
using System.IO;
using System.Text;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.FileSystem
{
    public class CombinePathSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            ActivateInputAdd(new PinDescription(typeof(string), UIType.TextEdit) {}, 2);

            AddOutput(typeof(string));
            UniqueName = "BlueSwitch.Base.Components.Switches.FileSystem.CombinePath.Base";
            DisplayName = "Combine Path";
            Description = "Combine Path";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.FileSystem;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            string currentPath = GetDataValueOrDefault<string>(0);
            for (int i = 1; i < TotalInputs; i++)
            {
                currentPath = Path.Combine(currentPath, GetDataValueOrDefault<string>(i));
            }

            SetData(0, new DataContainer(currentPath));
        }
    }
}
