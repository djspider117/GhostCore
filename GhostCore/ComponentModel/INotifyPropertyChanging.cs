using System;
using System.Collections.Generic;
using System.Text;

namespace GhostCore.ComponentModel
{
    public interface INotifyPropertyChanging
    {
        event EventHandler<PropertyChangingEventArgs> PropertyChanging;
    }

    public class PropertyChangingEventArgs : EventArgs
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public string PropertyName { get; set; }

        public PropertyChangingEventArgs(object oldValue, object newValue, string propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
            PropertyName = propertyName;
        }
    }
}
