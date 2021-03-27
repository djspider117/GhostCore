using System;

namespace GhostCore.MVVM.Messaging
{
    public interface IBusEvent<in T>
    {
        T DataObject { get; set; }
        string EventFilter { get; set; }
        object OriginalSource { get; set; }

        void Dispose();
    }

    public class BusEvent<T> : IDisposable, IBusEvent<T>
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
