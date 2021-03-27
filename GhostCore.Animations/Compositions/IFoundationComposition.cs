using GhostCore.Animations.Layers;
using System.Collections.Generic;
using Windows.UI;

namespace GhostCore.Animations
{
    public interface IFoundationComposition
    {
        IList<ILayer> Layers { get; set; }
        Color BackgroundColor { get; set; }

        IEnumerable<ImageLayer> GetAllImageLayers();
    }
}
