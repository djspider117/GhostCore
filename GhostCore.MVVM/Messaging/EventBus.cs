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
