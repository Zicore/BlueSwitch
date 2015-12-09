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
            return GroupBase.Trigger;
        }

        protected override void OnInitialize(RenderingEngine engine)
        {
            Name = "MouseClickSwitch";
            Description = "MouseClickSwitch";
            AddOutput(new OutputBase(new ActionSignature()));
            IsStart = true;
        }

        public override void OnRegisterEvents(ProcessingTree<SwitchBase> tree, RenderingEngine engine)
        {
            engine.EventManager.Register(EventTypeBase.MouseClick, new EventBase(tree));
        }

        protected virtual void OnClickTrigger(RenderingEngine engine, TriggerEventArgs e)
        {
            engine.EventManager.Run(EventTypeBase.MouseClick);
        }

        public override void UpdateMouseUp(RenderingEngine engine, DrawableBase parent, DrawableBase previous)
        {
            if (this.IsMouseOver)
            {
                OnClickTrigger(engine, new TriggerEventArgs(this, GetOutputData(0)));
            }
            base.UpdateMouseUp(engine, parent, previous);
        }
    }
}
