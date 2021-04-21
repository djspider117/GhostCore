using Microsoft.Graphics.Canvas;
using System;

namespace GhostCore.Animations.Rendering
{
    public class FileLoadEventArgs : EventArgs
    {
        public string Source { get; internal set; }
        public ICanvasResourceCreator ResourceCreator { get; internal set; }
    }
}
