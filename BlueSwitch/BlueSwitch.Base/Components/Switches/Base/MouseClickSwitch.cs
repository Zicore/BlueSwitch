using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Event;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Trigger.Types;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class MouseClickSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Trigger;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            Name = "MouseClickSwitch";
            Description = "MouseClickSwitch";
            AddOutput(new OutputBase(new ActionSignature()));
            IsStart = true;
        }

        public override void OnRegisterEvents(ProcessingTree<SwitchBase> tree, Engine renderingEngine)
        {
            renderingEngine.EventManager.Register(EventTypeBase.MouseClick, new EventBase(tree));
        }

        protected virtual void OnClickTrigger(Engine renderingEngine, TriggerEventArgs e)
        {
            renderingEngine.EventManager.Run(EventTypeBase.MouseClick);
        }

        public override void UpdateMouseUp(Engine renderingEngine, DrawableBase parent, DrawableBase previous)
        {
            if (this.IsMouseOver)
            {
                OnClickTrigger(renderingEngine, new TriggerEventArgs(this, GetOutputData(0)));
            }
            base.UpdateMouseUp(renderingEngine, parent, previous);
        }
    }
}
