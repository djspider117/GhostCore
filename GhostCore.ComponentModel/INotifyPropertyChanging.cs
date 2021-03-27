using System;
using System.Collections.Generic;
using System.Text;

namespace GhostCore.ComponentModel
{
    public interface INotifyPropertyChanging
    {
        event EventHandler<PropertyChangingEventArgs> PropertyChanging;
    }
}
