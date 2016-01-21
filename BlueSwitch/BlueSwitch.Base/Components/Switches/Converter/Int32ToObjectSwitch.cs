using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Converter
{
    public class Int32ToObjectSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Converter;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            Name = "Int32.ToObject";
            AddInput(typeof (int));
            AddOutput(typeof (object));
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetData(0);
            SetData(0, new DataContainer(data?.Value));
        }
    }
}
