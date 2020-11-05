using System.Threading.Tasks;

namespace GhostCore
{
    /// <summary>
    /// Supports cloning asynchronously, which creates a new instance of a class with the same value as an existing instance.
    /// </summary>
    public interface IAsyncCloneable
    {
        Task<T> CloneAsync<T>();

        Task<object> CloneAsync();
    }
}