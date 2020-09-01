/*
    This file is part of Project Emilie.
    Public redistribution is not permitted.
*/

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Media
{
    [Bindable]
    public class Properties : DependencyObject
    {
        #region ShouldAnimate

        /// <summary>
        /// Used by Preset Animations. If false, element will not be animated and will not affect stagger timings.
        /// </summary>
        public static bool GetShouldAnimate(DependencyObject obj) => (bool)obj.GetValue(ShouldAnimateProperty);

        /// <summary>
        /// Used by Preset Animations. If false, element will not be animated and will not affect stagger timings.
        /// </summary>
        public static void SetShouldAnimate(DependencyObject obj, bool value)
        {
            obj.SetValue(ShouldAnimateProperty, value);
        }

        /// <summary>
        /// Used by Preset Animations. If false, element will not be animated and will not affect stagger timings.
        /// </summary>
        public static readonly DependencyProperty ShouldAnimateProperty =
            DependencyProperty.RegisterAttached("ShouldAnimate", typeof(bool), typeof(Properties), new PropertyMetadata(true));


        #endregion

        #region AddedDelay

        /// <summary>
        /// Used by Preset Animations. If false, element will not be animated and will not affect stagger timings.
        /// </summary>
        public static TimeSpan GetAddedDelay(DependencyObject obj) => (TimeSpan)obj.GetValue(AddedDelayProperty);

        /// <summary>
        /// Used by Preset Animations. If false, element will not be animated and will not affect stagger timings.
        /// </summary>
        public static void SetAddedDelay(DependencyObject obj, TimeSpan value)
        {
            obj.SetValue(AddedDelayProperty, value);
        }

        /// <summary>
        /// Used by Preset Animations. Allows you to specific an additional stagger delay for a single element
        /// </summary>
        public static readonly DependencyProperty AddedDelayProperty =
            DependencyProperty.RegisterAttached("AddedDelay", typeof(TimeSpan), typeof(Properties), new PropertyMetadata(TimeSpan.Zero));


        #endregion

        #region AnimationGroup

        public static int GetAnimationGroup(DependencyObject obj) => (int)obj.GetValue(AnimationGroupProperty);

        public static void SetAnimationGroup(DependencyObject obj, int value)
        {
            obj.SetValue(AnimationGroupProperty, value);
        }

        public static readonly DependencyProperty AnimationGroupProperty =
            DependencyProperty.RegisterAttached("AnimationGroup", typeof(int), typeof(Properties), new PropertyMetadata(0));

        #endregion

        #region AnimationTargetType

        public static AnimationTargetType GetAnimationTargetType(DependencyObject obj) => (AnimationTargetType)obj.GetValue(AnimationTargetTypeProperty);

        public static void SetAnimationTargetType(DependencyObject obj, AnimationTargetType value)
        {
            obj.SetValue(AnimationTargetTypeProperty, value);
        }

        public static readonly DependencyProperty AnimationTargetTypeProperty =
            DependencyProperty.RegisterAttached("AnimationTargetType", typeof(AnimationTargetType), typeof(Properties), new PropertyMetadata(AnimationTargetType.Undefined));

        #endregion

        public static bool GetIsDepthTarget(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDepthTargetProperty);
        }

        public static void SetIsDepthTarget(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDepthTargetProperty, value);
        }

        public static readonly DependencyProperty IsDepthTargetProperty =
            DependencyProperty.RegisterAttached("IsDepthTarget", typeof(bool), typeof(FrameworkElement), new PropertyMetadata(false));



    }
}
