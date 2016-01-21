﻿using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class RestartSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.CodeFlow;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            Name = "Restart";
            Description = "Marks for restart";

            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            p.MarkForRestart();
            base.OnProcess(p, node);
        }
    }
}
