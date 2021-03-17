using GhostCore.Animations.Layers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostCore.Animations
{
    public interface IRuntimeFoundationComposition
    {
        IList<ILayer> ExplodedLayers { get; set; }
        Task<IList<ILayer>> ExplodePrecomps();
    }
}
