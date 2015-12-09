﻿using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Trigger.Types
{
    public class EventBase
    {
        public EventBase(ProcessingTree<SwitchBase> tree)
        {
            Tree = tree;
        }
        
        public ProcessingTree<SwitchBase> Tree { get; private set; }

        public void AssignTree(ProcessingTree<SwitchBase> tree)
        {
            Tree = tree;
        }

        public void Run(RenderingEngine engine, ProcessorCompiler compiler)
        {
            compiler.Run(engine, Tree);
        }
    }
}
