using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Event;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Trigger.Types;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class NamedStartSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Trigger;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "Named Start";
            Description = "Named Start";

            AddInput(typeof (string), new TextEdit());

            AddOutput(new OutputBase(new ActionSignature()));
            IsStart = true;
        }

        public override void OnRegisterEvents(ProcessingTree<SwitchBase> tree, Engine renderingEngine)
        {
            var name = GetDataValueOrDefault<string>(0);
            renderingEngine.EventManager.Register(EventTypeBase.StartSingle, new EventBase(tree) { Name = name});
        }
    }
}
