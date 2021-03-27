using System;

namespace GhostCore.MVVM.Messaging
{
    public class BusEvent : BusEvent<object>
    {
        public TType DataAs<TType>()
        {
            if (_isDisposed)
                throw new ObjectDisposedException($"The current object of type {nameof(BusEvent)} has been disposed");

            return (TType)DataObject;
        }
    }
}
