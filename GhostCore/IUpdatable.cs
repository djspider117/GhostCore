using System.Threading.Tasks;

namespace GhostCore
{
    public interface IUpdatable
    {
        void Update(object param = null);
    }

    public interface IAsyncUpdatable
    {
        Task UpdateAsync(object param = null);
    }
}

