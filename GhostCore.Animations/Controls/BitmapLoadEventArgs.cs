using Microsoft.Graphics.Canvas;
using System;
using System.Threading.Tasks;

namespace GhostCore.Animations.Controls
{
    public class BitmapLoadEventArgs : FileLoadEventArgs
    {
        public CanvasBitmap Bitmap { get; set; }
    }
}
