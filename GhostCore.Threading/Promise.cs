using Internal;
using System;
using System.Collections.Generic;

namespace GhostCore.Threading
{
    public class Promise<T> : IDisposable, IDisposing
    {
        public event EventHandler Disposing;

        private List<Action<T>> _handlers;

        public int Id { get; set; }
        public int FutureObjectId { get; set; }
        public T Result { get; private set; }

        //TODO: add expiration date, and culling in the service

        public Promise(int futureObjectId, Action<T> handler = null)
        {
            Id = __Incrementor.GetIncrementedValue();
            FutureObjectId = futureObjectId;
            _handlers = new List<Action<T>>();
            if (handler != null)
                _handlers.Add(handler);
        }

        public void Fulfill(T result)
        {
            Result = result;
            foreach (var handler in _handlers)
            {
                handler(result);
                Dispose();
            }
        }

        public void AddHandler(Action<T> handler)
        {
            if (handler != null)
                _handlers.Add(handler);
        }

        public void Dispose()
        {
            Disposing?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Promise : Promise<object>
    {
        public Promise(int futureObjectId, Action<object> handler = null)
            : base(futureObjectId, handler)
        {
        }
    }
}
