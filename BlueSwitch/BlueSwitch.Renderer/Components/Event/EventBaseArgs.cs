using System;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Event
{
    public class EventBaseArgs : EventArgs
    {
        public EventBaseArgs(ProcessingTree<SwitchBase> tree)
        {
            this.Tree = tree;
        }

        public ProcessingTree<SwitchBase> Tree { get; set; }
    }
}
