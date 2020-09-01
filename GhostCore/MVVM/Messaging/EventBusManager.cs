using System;
using System.Collections.Generic;

namespace GhostCore.MVVM.Messaging
{
    public class EventBusManager
    {
        #region Singleton

        private static volatile EventBusManager _instance;
        private static object _syncRoot = new object();


        public static EventBusManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new EventBusManager();
                    }
                }

                return _instance;
            }
        }
        private EventBusManager()
        {
            Initialize();
        }

        #endregion

        #region Fields

        private Dictionary<string, EventBus> _busses;

        #endregion

        private void Initialize()
        {
            _busses = new Dictionary<string, EventBus>
            {
                { "Default", new EventBus("Default") }
            };
        }

        public EventBus CreateBus()
        {
            var guid = Guid.NewGuid();
            return CreateBus(guid.ToString());
        }

        public EventBus CreateBus(string busName)
        {
            if (_busses.ContainsKey(busName))
                throw new ArgumentException("Bus with that name already exists.");

            var evtBus = new EventBus(busName);
            _busses.Add(busName, evtBus);

            return evtBus;
        }

        public EventBus GetOrCreateBus(string busName)
        {
            if (_busses.ContainsKey(busName))
                return _busses[busName];

            return CreateBus(busName);
        }

        public void DestroyBus(string busName)
        {
            if (!_busses.ContainsKey(busName))
                return;

            var bus = _busses[busName];
            bus.LastEvent.DataObject = null;
            bus.LastEvent.EventFilter = null;
            bus.LastEvent.OriginalSource = null;
            bus.LastEvent = null;
            _busses.Remove(busName);
        }

        public EventBus GetBus(string busName)
        {
            if (!_busses.ContainsKey(busName))
                return null;

            return _busses[busName];
        }
        public EventBus GetDefaultBus()
        {
            return _busses["Default"];
        }
    }
}
