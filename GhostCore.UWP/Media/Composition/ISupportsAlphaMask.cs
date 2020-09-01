/*
    This file is part of Project Emilie.
    Public redistribution is not permitted.
*/

using Windows.UI.Composition;

namespace GhostCore.UWP.Media
{
    public interface ISupportsAlphaMask
    {
        CompositionBrush GetAlphaMask();
    }
}
