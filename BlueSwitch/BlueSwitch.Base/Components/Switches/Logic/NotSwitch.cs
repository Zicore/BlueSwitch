using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Logic
{
    public class NotSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            AddInput(typeof (bool), new CheckBox());
            AddOutput(typeof(bool));

            UniqueName = "BlueSwitch.Base.Components.Switches.Logic.Not";
            DisplayName = "Not";
            ColumnWidth = 60;
            IsCompact = true;
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Logic;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var result = !GetDataValueOrDefault<bool>(0);
            SetData(0, new DataContainer(result));

            base.OnProcess(p, node);
        }
    }
}
