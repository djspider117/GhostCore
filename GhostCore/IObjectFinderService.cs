using System.Threading.Tasks;

namespace GhostCore
{
    public interface IObjectFinderService
    {
        IIdentifiable GetById(int id);
    }

}
