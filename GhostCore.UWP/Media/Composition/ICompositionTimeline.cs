/*
    This file is part of Project Emilie.
    Public redistribution is not permitted.
*/

using System;
using Windows.UI.Composition;

namespace GhostCore.UWP.Media
{
    public interface ICompositionTimeline : IDisposable
    {
        void Start();
        void Stop();
        Compositor Compositor { get; }
    }
}
