using GhostCore.Animations.Core;
using Microsoft.Graphics.Canvas;
using System.Threading.Tasks;

namespace GhostCore.Animations.Rendering.Loaders
{
    public interface ILayerSpecificResourceLoader
    {
        Task LoadResourceAsync(ICanvasResourceCreator resourceCreator, IResourceDependentLayer layer);
    }
}
