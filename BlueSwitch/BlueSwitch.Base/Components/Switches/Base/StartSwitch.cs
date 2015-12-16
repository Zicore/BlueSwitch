using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Event;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Trigger.Types;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class StartSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return GroupBase.Trigger;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            Name = "Start";
            Description = "Start";
            AddOutput(new OutputBase(new ActionSignature()));
            IsStart = true;

        }

        public override void OnRegisterEvents(ProcessingTree<SwitchBase> tree, Engine renderingEngine)
        {
            renderingEngine.EventManager.Register(EventTypeBase.Start, new EventBase(tree));
        }

        protected virtual void OnClickTrigger(TriggerEventArgs e)
        {
            
        }
    }
}
