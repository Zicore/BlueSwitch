using System;
using System.Drawing;
using BlueSwitch.Base.Services;

namespace BlueSwitch.Base
{
    public class ContextActionEventArgs : EventArgs
    {
        public InputOutputSelector Selector { get; set; }
        public Point Location { get; set; }
    }
}
