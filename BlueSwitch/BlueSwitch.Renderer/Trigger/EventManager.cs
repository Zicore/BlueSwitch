using System.Collections.Generic;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Trigger.Types;

namespace BlueSwitch.Base.Trigger
{
    public class EventManager
    {
        public RenderingEngine RenderingEngine { get; }

        public EventManager(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
        }
        
        public Dictionary<EventTypeBase,List<EventBase>> Items { get; } = new Dictionary<EventTypeBase, List<EventBase>>();

        public void Register(EventTypeBase eventType, EventBase eventBase)
        {
            if (!Items.ContainsKey(eventType))
            {
                Items[eventType] = new List<EventBase>();
            }

            Items[eventType].Add(eventBase);
        }
        
        public void Run(EventTypeBase type)
        {
            if (Items.ContainsKey(type))
            {
                foreach (var eventBase in Items[type])
                {
                    eventBase.Run(RenderingEngine, RenderingEngine.ProcessorCompiler);
                }
            }
        }
    }
}
