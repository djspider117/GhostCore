using System;

namespace GhostCore.ComponentModel
{
    public class PropertyChangingEventArgs : EventArgs
    {
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }
        public string PropertyName { get; private set; }

        public PropertyChangingEventArgs(object oldValue, object newValue, string propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
            PropertyName = propertyName;
        }
    }
}
