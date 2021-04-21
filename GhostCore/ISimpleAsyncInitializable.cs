using System.Threading.Tasks;

namespace GhostCore
{
    public interface ISimpleAsyncInitializable
    {
        Task InitializeAsync();
    }
}