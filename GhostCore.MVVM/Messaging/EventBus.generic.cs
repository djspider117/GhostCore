using System;

namespace GhostCore.MVVM.Messaging
{
    public delegate void BusEventHandler<T>(BusEvent<T> e);

    public class EventBus<T> : IDisposable
    {
        public event BusEventHandler<T> EventBroadcasted;

        internal bool _isDisposed;

        public string Name { get; internal set; }
        public BusEvent<T> LastEvent { get; protected set; }
        public bool CacheLastEvent { get; set; }

        public EventBus(string name)
        {
            Name = name;
        }

        public void Publish(T data) => Publish(null, data, null);
        public void Publish(T data, object originalSource) => Publish(null, data, originalSource);

        public void Publish(object source, params T[] data)
        {
            foreach (var item in data)
            {
                Publish(item, source);
            }
        }

        public virtual void Publish(string evtFilter, T data, object originalSource)
        {
            var busEvent = new BusEvent<T>()
            {
                EventFilter = evtFilter,
                DataObject = data,
                OriginalSource = originalSource
            };

            if (CacheLastEvent)
                LastEvent = busEvent;

            OnEventBroadcasted(busEvent);
        }

        protected void OnEventBroadcasted(BusEvent<T> evt) => EventBroadcasted?.Invoke(evt);

        public void Dispose()
        {
            _isDisposed = true;
            LastEvent?.Dispose();
            LastEvent = null;
            Name = null;
        }
    }
}
