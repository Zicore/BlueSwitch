using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Event;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Meta.Help;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Trigger.Types;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class StartSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Trigger;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.Base.Start";
            DisplayName = "Start";
            Description = "Start";
            AddOutput(new OutputBase(new ActionSignature()));
            IsStart = true;
            IsCompact = true;
        }

        protected override void OnInitializeMetaInformation(Engine engine)
        {
            //engine.HelpService.AddOutput(new HelpDescriptionEntry { Title = "Output Trigger"}, this, 0);
            base.OnInitializeMetaInformation(engine);
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
