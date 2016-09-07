using System.Text;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class GetFromArraySwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            AddInput(typeof(string[]));
            AddInput(typeof(int));

            AddOutput(typeof(string));
            UniqueName = "BlueSwitch.Base.Components.Switches.Base";
            DisplayName = "FromArray";
            Description = "FromArray";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Base;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var array = GetDataValueOrDefault<string[]>(0);
            var index = GetDataValueOrDefault<int>(1);

            var entry = array[index];


            SetData(0, new DataContainer(entry));
        }
    }
}
