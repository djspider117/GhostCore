namespace GhostCore.Animations.Layers
{
    public class ImageLayer : LayerBase
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
