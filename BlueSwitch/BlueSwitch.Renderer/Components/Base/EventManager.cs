using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Renderer.Components.Event;

namespace BlueSwitch.Renderer.Components.Base
{
    public class EventManager
    {
        //public List<EventBase> StartItems { get; } = new List<EventBase>();

        public event EventHandler<EventBaseArgs> Start;

        public virtual void OnStart(EventBaseArgs e)
        {
            Start?.Invoke(this, e);
        }
    }
}
