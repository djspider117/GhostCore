using System;
using Windows.Foundation;

namespace GhostCore.UWP
{
    public class SizeEventArgs : EventArgs
    {
        public Size OldSize { get; set; }
        public Size NewSize { get; set; }
        public bool ShouldDelayAnimation { get; set; } = true;
    }
}
