using GhostCore.Animations.Core;

namespace GhostCore.Animations.Data.Layers
{
    public class ImageLayer : LayerBase, IResourceDependentLayer
    {
        public string Source { get; set; }

        public ImageType Type { get; set; }

        public int SvgViewportWidth { get; set; }
        public int SvgViewportHeight { get; set; }
    }

    public enum ImageType
    {
        Bitmap,
        Svg
    }
}
