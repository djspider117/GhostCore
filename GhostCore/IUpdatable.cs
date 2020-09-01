using System.Threading.Tasks;

namespace GhostCore
{
    public interface IUpdatable
    {
        void Update();
    }

    public interface IAsyncUpdatable
    {
        Task UpdateAsync();
    }
}

