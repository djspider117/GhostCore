/*
    This file is part of Project Emilie.
    Public redistribution is not permitted.
*/

using System;

namespace GhostCore.UWP.Media
{
    /// <summary>
    /// A collection of common PropertyPath's used by XAML Storyboards
    /// for creating independently animate-able animations. Hence the word
    /// "independent". Don't add CPU bound properties here please. I don't
    /// care for them.
    /// </summary>
    public static class TargetProperty
    {
        //------------------------------------------------------
        //
        // UI Element
        //
        //------------------------------------------------------

        // UIElement Opacity
        public static String Opacity = "(UIElement.Opacity)";
        // UIElement Visibility
        public static String Visiblity = "(UIElement.Visibility)";





        //------------------------------------------------------
        //
        // Composite Transform (Render Transform)
        //
        //------------------------------------------------------

        public class CompositeTransform
        {
            public static string Identifier { get; } = "(UIElement.RenderTransform).(CompositeTransform.";

            // Render Transform Composite Transform X-Axis Translation
            public static String TranslateX { get; } = "(UIElement.RenderTransform).(CompositeTransform.TranslateX)";
            // Render Transform Composite Transform Y-Axis Translation
            public static String TranslateY { get; } = "(UIElement.RenderTransform).(CompositeTransform.TranslateY)";
            // Render Transform Composite Transform X-Axis Scale
            public static String ScaleX { get; } = "(UIElement.RenderTransform).(CompositeTransform.ScaleX)";
            // Render Transform Composite Transform Y-Axis Scale
            public static String ScaleY { get; } = "(UIElement.RenderTransform).(CompositeTransform.ScaleY)";
            // Render Transform Composite Transform X-Scale Skew
            public static String SkewX { get; } = "(UIElement.RenderTransform).(CompositeTransform.SkewX)";
            // Render Transform Composite Transform Y-Scale Skew
            public static String SkewY { get; } = "(UIElement.RenderTransform).(CompositeTransform.SkewY)";
            // Render Transform Composite Transform Rotation
            public static String Rotation { get; } = "(UIElement.RenderTransform).(CompositeTransform.Rotation)";
        }







        //------------------------------------------------------
        //
        //  Plane Projection
        //
        //------------------------------------------------------

        public class PlaneProjection
        {
            public static string Identifier { get; } = "(UIElement.Projection).(PlaneProjection.";

            // Plane Projection X-Axis Rotation
            public static String RotationX { get; } = "(UIElement.Projection).(PlaneProjection.RotationX)";
            // Plane Projection Y-Axis Rotation
            public static String RotationY { get; } = "(UIElement.Projection).(PlaneProjection.RotationY)";
            // Plane Projection Z-Axis Rotation
            public static String RotationZ { get; } = "(UIElement.Projection).(PlaneProjection.RotationZ)";

            public static String GlobalOffsetX { get; } = "(UIElement.Projection).(PlaneProjection.GlobalOffsetX)";
            public static String GlobalOffsetY { get; } = "(UIElement.Projection).(PlaneProjection.GlobalOffsetY)";
            public static String GlobalOffsetZ { get; } = "(UIElement.Projection).(PlaneProjection.GlobalOffsetZ)";

            public static String LocalOffsetX { get; } = "(UIElement.Projection).(PlaneProjection.LocalOffsetX)";
            public static String LocalOffsetY { get; } = "(UIElement.Projection).(PlaneProjection.LocalOffsetY)";
            public static String LocalOffsetZ { get; } = "(UIElement.Projection).(PlaneProjection.LocalOffsetZ)";

            public static String CenterOfRotationX { get; } = "(UIElement.Projection).(PlaneProjection.CenterOfRotationX)";
            public static String CenterOfRotationY { get; } = "(UIElement.Projection).(PlaneProjection.CenterOfRotationY)";
            public static String CenterOfRotationZ { get; } = "(UIElement.Projection).(PlaneProjection.CenterOfRotationZ)";
        }





        //------------------------------------------------------
        //
        //  Composite Transform 3D (Transform 3D)
        //
        //------------------------------------------------------

        public class CompositeTransform3D
        {
            public static string Identifier { get; } = "(UIElement.Transform3D).(CompositeTransform3D.";

            public static String TranslateX { get; } = "(UIElement.Transform3D).(CompositeTransform3D.TranslateX)";
            public static String TranslateY { get; } = "(UIElement.Transform3D).(CompositeTransform3D.TranslateY)";
            public static String TranslateZ { get; } = "(UIElement.Transform3D).(CompositeTransform3D.TranslateZ)";

            public static String RotationX { get; } = "(UIElement.Transform3D).(CompositeTransform3D.RotationX)";
            public static String RotationY { get; } = "(UIElement.Transform3D).(CompositeTransform3D.RotationY)";
            public static String RotationZ { get; } = "(UIElement.Transform3D).(CompositeTransform3D.RotationZ)";

            public static String ScaleX { get; } = "(UIElement.Transform3D).(CompositeTransform3D.ScaleX)";
            public static String ScaleY { get; } = "(UIElement.Transform3D).(CompositeTransform3D.ScaleY)";
            public static String ScaleZ { get; } = "(UIElement.Transform3D).(CompositeTransform3D.ScaleZ)";

            public static String CenterX { get; } = "(UIElement.Transform3D).(CompositeTransform3D.CenterX)";
            public static String CenterY { get; } = "(UIElement.Transform3D).(CompositeTransform3D.CenterY)";
            public static String CenterZ { get; } = "(UIElement.Transform3D).(CompositeTransform3D.CenterZ)";
        }
    }
}
