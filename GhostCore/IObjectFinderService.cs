using GhostCore.ObjectModel;
using System.Threading.Tasks;

namespace GhostCore
{
    public interface IObjectFinderService
    {
        object GetById(int id);
    }

    public interface IIdentifiable
    {
        int Id { get; set; }
    }
}
