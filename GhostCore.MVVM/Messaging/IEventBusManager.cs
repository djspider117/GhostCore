namespace GhostCore.MVVM.Messaging
{
    public interface IEventBusManager
    {
        EventBus CreateBus();
        EventBus CreateBus(string busName);
        EventBus<T> CreateBus<T>();
        EventBus<T> CreateBus<T>(string busName);

        EventBus GetBus(string busName);
        EventBus<T> GetBus<T>(string busName);


        EventBus GetOrCreateBus(string busName);
        EventBus<T> GetOrCreateBus<T>(string busName);

        EventBus GetDefaultBus();

        void DestroyBus(string busName);
    }
}