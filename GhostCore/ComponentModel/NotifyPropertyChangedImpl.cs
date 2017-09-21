using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.ComponentModel
{
    public class NotifyPropertyChangedImpl : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool DisableINPC { get; set; }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null || DisableINPC)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
