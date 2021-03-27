using System;
using System.Collections.Generic;

namespace GhostCore.MVVM.Messaging
{
    public class EventBusManager : IEventBusManager
    {
        private const string DEFAULT_BUS_NAME = "Default";

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

        private Dictionary<string, object> _busses;

        #endregion

        private void Initialize()
        {
            _busses = new Dictionary<string, object>
            {
                { DEFAULT_BUS_NAME, new EventBus(DEFAULT_BUS_NAME) }
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

        public EventBus<T> CreateBus<T>()
        {
            var guid = Guid.NewGuid();
            return CreateBus<T>(guid.ToString());
        }
        public EventBus<T> CreateBus<T>(string busName)
        {
            if (_busses.ContainsKey(busName))
                throw new ArgumentException("Bus with that name already exists.");

            var evtBus = new EventBus<T>(busName);
            _busses.Add(busName, evtBus);

            return evtBus;
        }

        public EventBus GetOrCreateBus(string busName)
        {
            if (_busses.ContainsKey(busName))
            {
                var bus = _busses[busName] as EventBus;
                if (bus._isDisposed)
                    throw new ObjectDisposedException(nameof(bus));
            }

            return CreateBus(busName);
        }
        public EventBus<T> GetOrCreateBus<T>(string busName)
        {
            if (_busses.ContainsKey(busName))
            {
                var bus = _busses[busName] as EventBus<T>;
                if (bus._isDisposed)
                    throw new ObjectDisposedException(nameof(bus));
            }

            return CreateBus<T>(busName);
        }

        public EventBus GetBus(string busName)
        {
            if (!_busses.ContainsKey(busName))
                return null;

            var bus = _busses[busName] as EventBus;
            if (bus._isDisposed)
                throw new ObjectDisposedException(nameof(bus));

            return bus;
        }
        public EventBus<T> GetBus<T>(string busName)
        {
            if (!_busses.ContainsKey(busName))
                return null;

            var bus = _busses[busName] as EventBus<T>;
            if (bus._isDisposed)
                throw new ObjectDisposedException(nameof(bus));

            return bus;
        }

        public EventBus GetDefaultBus()
        {
            return _busses[DEFAULT_BUS_NAME] as EventBus;
        }

        public void DestroyBus(string busName)
        {
            if (!_busses.ContainsKey(busName))
                return;

            var bus = _busses[busName];
            bus.Dispose();

            _busses.Remove(busName);
        }
    }
}
