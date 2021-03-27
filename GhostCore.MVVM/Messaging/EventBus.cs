using System;
using System.Text;

namespace GhostCore.MVVM.Messaging
{
    public class EventBus : EventBus<object>
    {
        public EventBus(string name) : base(name)
        {
            Name = name;
        }
    }
}
