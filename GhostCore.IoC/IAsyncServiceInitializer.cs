
using System.Threading.Tasks;

namespace GhostCore.IoC
{
    public interface IAsyncServiceInitializer
    {
        Task<ISafeTaskResult> Initialize(params object[] args);
    }
}
