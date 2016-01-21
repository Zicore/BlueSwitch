using System;

namespace BlueSwitch.Base.Trigger.Types
{
    public class EventTypeBase
    {
        private String _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        protected EventTypeBase()
        {
            
        }

        public static readonly EventTypeBase Start = new EventTypeBase { Name = "Start"};
        public static readonly EventTypeBase MouseClick = new EventTypeBase { Name = "MouseClick" };
        public static readonly EventTypeBase StartSingle = new EventTypeBase { Name = "StartSingle" };
        public static readonly EventTypeBase TimerTick = new EventTypeBase { Name = "TimerTick" };
    }
}
