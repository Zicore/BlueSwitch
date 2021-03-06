﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Debug
{
    public class ExceptionSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Debug;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            UniqueName = "BlueSwitch.Base.Components.Switches.Debug.Exception";
            DisplayName = "Exception";
            Description = "Throws an exception";

            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p, node);

            throw new InvalidOperationException("Exception Switch");
        }
    }
}
