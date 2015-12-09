using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Renderer.Components.Base;
using BlueSwitch.Renderer.Components.Event;
using BlueSwitch.Renderer.Components.Types;

namespace BlueSwitch.Renderer.Components.Switches.Base
{
    public class TriggerBase : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return GroupBase.Trigger;
        }

        protected override void OnInitialize()
        {
            Name = "Trigger";
            Description = "Triggert wenn die Maus über den Switch ist und geklickt wird";
            ColumnWidth = 40;
            AddOutput(new OutputBase(new ActionSignature()));
            IsStart = true;
        }


        public event EventHandler<TriggerEventArgs> ClickTrigger;

        protected virtual void OnClickTrigger(TriggerEventArgs e)
        {
            ClickTrigger?.Invoke(this, e);
        }

        public override void UpdateMouseUp(RenderingEngine e, ComponentBase parent, InputOutputBase previous)
        {
            if (this.IsMouseOver)
            {
                OnClickTrigger(new TriggerEventArgs(this, GetOutputData(0)));
            }
            base.UpdateMouseUp(e, parent, previous);
        }
    }
}
