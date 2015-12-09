using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Converter
{
    public class BooleanToStringSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return GroupBase.Converter;
        }

        protected override void OnInitialize(RenderingEngine engine)
        {
            Name = "Boolean.ToString";
            AddInput(typeof (bool));
            AddOutput(typeof (string));
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetData(0);
            SetData(0, new DataContainer(data?.Value?.ToString()));
        }
    }
}
