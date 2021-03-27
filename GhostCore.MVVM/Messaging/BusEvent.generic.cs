using System;

namespace GhostCore.MVVM.Messaging
{
    public class BusEvent<T> : IDisposable
    {
        internal bool _isDisposed;

        public string EventFilter { get; set; }
        public T DataObject { get; set; }
        public object OriginalSource { get; set; }

        public void Dispose()
        {
            EventFilter = null;
            DataObject = default;
            OriginalSource = null;
            _isDisposed = true;
        }
    }
}
