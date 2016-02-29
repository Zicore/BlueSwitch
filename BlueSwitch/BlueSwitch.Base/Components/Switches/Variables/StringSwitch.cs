using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Variables
{
    public class StringSwitch : TextEditBaseSwitch
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine); // Wichtig
            AddOutput(typeof(string));
            UniqueName = "String";
            ColumnWidth = 120;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            SetData(0, new DataContainer(TextEdit.Text));
            base.OnProcessData(p, node);
        }
    }
}
