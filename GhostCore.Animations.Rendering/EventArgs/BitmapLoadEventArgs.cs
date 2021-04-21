using Microsoft.Graphics.Canvas;

namespace GhostCore.Animations.Rendering
{
    public class BitmapLoadEventArgs : FileLoadEventArgs
    {
        public CanvasBitmap Bitmap { get; set; }
    }
}
