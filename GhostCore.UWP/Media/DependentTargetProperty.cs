/*
    This file is part of Project Emilie.
    Public redistribution is not permitted.
*/

namespace GhostCore.UWP.Media
{
    /// <summary>
    /// Common strings used to create property paths for CPU bound animate-able properties
    /// </summary>
    public static class DependentTargetProperty
    {
        public static string Margin = "(FrameworkElement.Margin)";
        public static string Padding = "Padding";
        public static string Height = "(FrameworkElement.Height)";
        public static string Width = "(FrameworkElement.Width)";
        public static string CanvasTop = "(Canvas.Top)";
        public static string CanvasLeft = "(Canvas.Left)";
    }
}
