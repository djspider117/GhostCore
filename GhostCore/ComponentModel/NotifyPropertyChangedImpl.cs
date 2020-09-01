using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.ComponentModel
{
    public class NotifyPropertyChangedImpl : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<PropertyChangingEventArgs> PropertyChanging;

        public bool DisableINPC { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (DisableINPC)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanging(string propertyName, object oldValue, object newValue)
        {
            if (DisableINPC)
                return;

            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(oldValue, newValue, propertyName));
        }

        protected void ExternalAttachPropertyChanged(PropertyChangedEventHandler handler)
        {
            if (PropertyChanged == null)
                PropertyChanged += handler;
        }
        protected void ExternalDetachPropertyChanged(PropertyChangedEventHandler handler)
        {
            PropertyChanged -= handler;
        }

        public void RunWithDisableINPC(Action a)
        {
            DisableINPC = true;
            a();
            DisableINPC = false;
        }
    }
}
