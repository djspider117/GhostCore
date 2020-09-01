using System;
using System.Text;

namespace GhostCore.MVVM.Messaging
{
    public delegate void BusEventHandler(BusEvent e);
    public class EventBus
    {
        public event BusEventHandler EventBroadcasted;

        public string Name { get; set; }
        public BusEvent LastEvent { get; set; }
        public bool CacheLastEvent { get; set; }

        public EventBus(string name)
        {
            Name = name;
        }

        public void Publish(object data, object originalSource)
        {
            Publish(null, data, originalSource);
        }

        public void Publish(string evtFilter, object data, object originalSource)
        {
            var busEvent = new BusEvent() { EventFilter = evtFilter, DataObject = data, OriginalSource = originalSource };
            
            if (CacheLastEvent)
                LastEvent = busEvent;
            
            OnEventBroadcasted(busEvent);
        }

        protected void OnEventBroadcasted(BusEvent evt)
        {
            if (EventBroadcasted == null)
                return;

            EventBroadcasted(evt);
        }
    }

    public class BusEvent
    {
        public string EventFilter { get; set; }
        public object DataObject { get; set; }
        public object OriginalSource { get; set; }

        public T DataAs<T>()
        {
            return (T)DataObject;
        }
    }
}
