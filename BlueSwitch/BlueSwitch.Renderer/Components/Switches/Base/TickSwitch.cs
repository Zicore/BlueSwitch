using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Event;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Trigger.Types;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class TickSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return GroupBase.Trigger;
        }

        protected override void OnInitialize(RenderingEngine engine)
        {
            Name = "Tick";
            Description = "Tick";
            AddOutput(new OutputBase(new ActionSignature()));
            IsStart = true;

        }

        public override void OnRegisterEvents(ProcessingTree<SwitchBase> tree, RenderingEngine engine)
        {
            engine.EventManager.Register(EventTypeBase.TimerTick, new EventBase(tree));
        }
    }
}
