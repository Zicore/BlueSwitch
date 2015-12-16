using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;

namespace BlueSwitch.Base.Components.Event
{
    public class TriggerEventArgs : EventArgs
    {
        public SwitchBase Origin { get; set; }
        public DataContainer Data { get; set; }

        public TriggerEventArgs(SwitchBase origin, DataContainer data)
        {
            Origin = origin;
            Data = data;
        }
    }
}
