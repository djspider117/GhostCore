namespace GhostCore
{
    public interface ISafeTaskResult<out T> : ISafeTaskResult
    {
        T ResultValue { get; }
        ISafeTaskResult<K> Cast<K>();
        ISafeTaskResult<K> Convert<K>();
    }
}
