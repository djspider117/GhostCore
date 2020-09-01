/*
    This file is part of Project Emilie.
    Public redistribution is not permitted.
*/

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace GhostCore.UWP.Media.Simple
{
    /// <summary>
    /// Provides simple methods to create render transforms.
    /// Can also be used as extensions if you like polluting
    /// your intellisense.
    /// </summary>
    public static class RenderTransform
    {
        #region Factory Methods
        internal static Storyboard GetStoryboard(FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            String property,
            EasingFunctionBase ease = null)
        {
            Storyboard sb = new Storyboard();
            var da = CreateDoubleAnimation(element, to, from, duration, ease);
            sb.AddTimeline(da, element, property);
            return sb;
        }

        public static DoubleAnimation CreateDoubleAnimation(
            FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null,
            String propertyPath = null)
        {
            element.GetCompositeTransform();

            DoubleAnimation db = new DoubleAnimation();
            db.To = to;
            db.From = from;
            db.Duration = new Duration(duration);
            db.EasingFunction = ease;

            if (!String.IsNullOrWhiteSpace(propertyPath))
                Storyboard.SetTargetProperty(db, propertyPath);

            Storyboard.SetTarget(db, element);

            return db;
        }

        #endregion

        #region Translate X

        public static DoubleAnimation CreateTranslateXDoubleAnimation(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return CreateDoubleAnimation(element, to, from, duration, null, TargetProperty.CompositeTransform.TranslateX);
        }

        public static Task TranslateXAsync(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return GetStoryboard(element, to, from, duration, TargetProperty.CompositeTransform.TranslateX, ease).BeginAsync();
        }

        #endregion

        #region Translate Y
        public static DoubleAnimation CreateTranslateYDoubleAnimation(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return CreateDoubleAnimation(element, to, from, duration, null, TargetProperty.CompositeTransform.TranslateY);
        }

        public static Task TranslateYAsync(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return GetStoryboard(element, to, from, duration, TargetProperty.CompositeTransform.TranslateY, ease).BeginAsync();
        }

        #endregion

        #region Skew X

        public static DoubleAnimation GetSkewXDoubleAnimation(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return CreateDoubleAnimation(element, to, from, duration, null, TargetProperty.CompositeTransform.SkewX);
        }

        public static Task SkewXAsync(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return GetStoryboard(element, to, from, duration, TargetProperty.CompositeTransform.SkewX, ease).BeginAsync();
        }

        #endregion

        #region Skew Y

        public static DoubleAnimation CreateSkewYDoubleAnimation(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return CreateDoubleAnimation(element, to, from, duration, null, TargetProperty.CompositeTransform.SkewY);
        }

        public static Task SkewYAsync(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return GetStoryboard(element, to, from, duration, TargetProperty.CompositeTransform.SkewY, ease).BeginAsync();
        }

        #endregion

        #region Scale X

        public static DoubleAnimation CreateScaleXDoubleAnimation(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return CreateDoubleAnimation(element, to, from, duration, null, TargetProperty.CompositeTransform.ScaleX);
        }

        public static async Task ScaleXAsync(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            await GetStoryboard(element, to, from, duration, TargetProperty.CompositeTransform.ScaleX, ease).BeginAsync();
        }

        #endregion

        #region Scale Y

        public static DoubleAnimation CreateScaleYDoubleAnimation(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return CreateDoubleAnimation(element, to, from, duration, null, TargetProperty.CompositeTransform.ScaleY);
        }

        public static Task ScaleYAsync(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return GetStoryboard(element, to, from, duration, TargetProperty.CompositeTransform.ScaleY, ease).BeginAsync();
        }

        #endregion

        #region Rotate

        public static DoubleAnimation GetRotationDoubleAnimation(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return CreateDoubleAnimation(element, to, from, duration, null, TargetProperty.CompositeTransform.Rotation);
        }

        public static Task RotateAsync(this FrameworkElement element,
            Double? to,
            Double? from,
            TimeSpan duration,
            EasingFunctionBase ease = null)
        {
            return GetStoryboard(element, to, from, duration, TargetProperty.CompositeTransform.Rotation, ease).BeginAsync();
        }

        #endregion
    }
}
