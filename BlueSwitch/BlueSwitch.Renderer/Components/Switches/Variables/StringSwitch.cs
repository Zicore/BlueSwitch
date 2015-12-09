using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Variables
{
    public class StringSwitch : VariableSwitch
    {
        protected override void OnInitialize(RenderingEngine engine)
        {
            base.OnInitialize(engine); // Wichtig
            AddOutput(typeof(string));
            Name = "String";
            ColumnWidth = 120;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            SetData(0, new DataContainer(TextEdit.Text));
            base.OnProcessData(p, node);
        }
    }
}
