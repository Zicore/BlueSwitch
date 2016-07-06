using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Variables
{
    public class StringSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            TextEdit = new TextEdit();
            TextEdit.AutoResize = true;
            //TextEdit.AutoStoreValue = true;

            AddOutput(typeof(string), TextEdit);
            UniqueName = "BlueSwitch.Base.Components.Switches.Variables.String";
            DisplayName = "String";
            ColumnWidth = 120;
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Variable;
        }

        protected TextEdit TextEdit;

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            SetData(0, new DataContainer(TextEdit.Text));
            base.OnProcessData(p, node);
        }
    }
}
