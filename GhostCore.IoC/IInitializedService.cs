using GhostCore.Foundation;

namespace GhostCore.IoC
{
    public interface IInitializedService
    {
        ISafeTaskResult Initialize(params object[] args);
    }
}
