using System;

namespace GhostCore.ComponentModel
{
    public interface INotifyPropertyChanging
    {
        event EventHandler<PropertyChangingEventArgs> PropertyChanging;
    }
}
