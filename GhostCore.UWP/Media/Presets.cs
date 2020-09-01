/*
    This file is part of Project Emilie.
    Public redistribution is not permitted.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Media3D;

namespace GhostCore.UWP.Media
{
    public static class Presets
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static Storyboard EntranceTheme(
            IEnumerable<FrameworkElement> items,
            double verticalOffset = 0,
            double horizontalOffset = 0,
            TimeSpan? customStagger = null)
        {
            // 0. Prepare ourselves a nice list of visible framework elements to animate
            List<FrameworkElement> _items = items.Where(i => i.Visibility == Visibility.Visible && Properties.GetShouldAnimate(i)).ToList();
            List<double> _opacitys = new List<double>();

            // 1. Hide them all to prevent flickering
            foreach (var item in _items)
            {
                // 1.1. Record the original opacity value - we'll animate to this value later
                _opacitys.Add(item.Opacity);

                // 1.2. Hide the item
                item.Opacity = 0;
            }

            // 2. Create the return storyboard, and the properties we're going to use
            Storyboard sb = new Storyboard();
            TimeSpan startOffset = TimeSpan.FromSeconds(0);
            TimeSpan staggerTime = customStagger != null && customStagger.HasValue ? customStagger.Value : TimeSpan.FromMilliseconds(83);
            TimeSpan duration = TimeSpan.FromMilliseconds(1000);
            TimeSpan durationOpacity = TimeSpan.FromMilliseconds(300);

            // 3. Now let's build the storyboard!
            for (int i = 0; i < _items.Count; i++)
            {
                // 3.0. Get the item and it's opacity 
                FrameworkElement item = _items[i];
                Double _originalOpacity = _opacitys[i];

                // 3.1. Check AddedDelay
                startOffset = startOffset.Add(Properties.GetAddedDelay(item));

                // 3.2. Animate the opacity
                sb.CreateTimeline<DoubleAnimationUsingKeyFrames>(item, TargetProperty.Opacity)
                    .AddKeyFrame(TimeSpan.Zero, 0)
                    .AddKeyFrame(startOffset, 0)
                    .AddKeyFrame(startOffset.Add(durationOpacity), _originalOpacity, KeySplines.EntranceTheme);

                // 3.3. Animate the X-Translation
                if (horizontalOffset != 0)
                {
                    sb.CreateTimeline<DoubleAnimationUsingKeyFrames>(item, TargetProperty.CompositeTransform.TranslateX)
                        .AddKeyFrame(TimeSpan.Zero, horizontalOffset)
                        .AddKeyFrame(startOffset, horizontalOffset)
                        .AddKeyFrame(startOffset.Add(duration), 0, KeySplines.EntranceTheme);
                }

                // 3.3. Animate the Y-Translation
                if (verticalOffset != 0)
                {
                    sb.CreateTimeline<DoubleAnimationUsingKeyFrames>(item, TargetProperty.CompositeTransform.TranslateY)
                        .AddKeyFrame(TimeSpan.Zero, verticalOffset)
                        .AddKeyFrame(startOffset, verticalOffset)
                        .AddKeyFrame(startOffset.Add(duration), 0, KeySplines.EntranceTheme);
                }

                // 3.4. Increment start offset
                startOffset = startOffset.Add(staggerTime);
            }

            return sb;
        }


        public static Storyboard CreateDepth3DIn(
            IEnumerable<FrameworkElement> items,
            FrameworkElement container = null,
            double startDepth = -300,
            TimeSpan? customStagger = null)
        {
            // 0. Prepare ourselves a nice list of visible framework elements to animate
            List<FrameworkElement> _items = items.Where(i => i.Visibility == Visibility.Visible && Properties.GetShouldAnimate(i)).ToList();
            List<double> _opacitys = new List<double>();
            container?.GetPerspectiveTransform3D();


            // 1. Hide them all to prevent flickering
            //foreach (var item in _items)
            //{
            //    if (_opacitys == null)
            //        _opacitys = new List<double>();

            //    // 1.1. Record the original opacity value - we'll animate to this value later
            //    _opacitys.Add(item.Opacity);

            //    // 1.2. Hide the item
            //    item.Opacity = 0;
            //}

            // 2. Create the return storyboard, and the properties we're going to use
            Storyboard sb = new Storyboard();
            TimeSpan startOffset = TimeSpan.FromSeconds(0);
            TimeSpan staggerTime = customStagger != null && customStagger.HasValue ? customStagger.Value : TimeSpan.FromMilliseconds(60);
            TimeSpan duration = TimeSpan.FromMilliseconds(500);
            TimeSpan durationOpacity = TimeSpan.FromMilliseconds(300);

            // 3. Now let's build the storyboard!
            for (int i = 0; i < _items.Count; i++)
            {
                // 3.0. Get the item and it's opacity 
                FrameworkElement item = _items[i];
                //Double _originalOpacity = _opacitys[i];
                item.GetCompositeTransform3D();

                // 3.1. Check AddedDelay
                startOffset = startOffset.Add(Properties.GetAddedDelay(item));

                // 3.2. Animate the opacity
                sb.CreateTimeline<DoubleAnimationUsingKeyFrames>(item, TargetProperty.Opacity)
                    .AddKeyFrame(TimeSpan.Zero, 0)
                    .AddKeyFrame(startOffset, 0)
                    .AddKeyFrame(startOffset.Add(durationOpacity), 1, KeySplines.DepthZoomOpacity);

                // 3.3. Animate the 3D depth translation
                if (startDepth != 0)
                {
                    sb.CreateTimeline<DoubleAnimationUsingKeyFrames>(item, TargetProperty.CompositeTransform3D.TranslateZ)
                        .AddKeyFrame(TimeSpan.Zero, startDepth)
                        .AddKeyFrame(startOffset, startDepth)
                        .AddKeyFrame(startOffset.Add(duration), 0, KeySplines.EntranceTheme);
                }

                // 3.4. Increment start offset
                startOffset = startOffset.Add(staggerTime);
            }

            return sb;
        }


        public static Storyboard CreateDepth3DOut(
           IEnumerable<FrameworkElement> items,
           FrameworkElement container = null,
           double endDepth = 300,
           TimeSpan? customStagger = null,
           TimeSpan? customDuration = null
           )
        {
            // 0. Prepare ourselves a nice list of visible framework elements to animate
            List<FrameworkElement> _items = items.Where(i => i.Visibility == Visibility.Visible && Properties.GetShouldAnimate(i) && Properties.GetIsDepthTarget(i)).ToList();
            List<double> _opacitys = new List<double>();
            container?.GetPerspectiveTransform3D();


            //// 1. Hide them all to prevent flickering
            //foreach (var item in _items)
            //{
            //    // 1.1. Record the original opacity value - we'll animate to this value later
            //    _opacitys.Add(item.Opacity);

            //    // 1.2. Hide the item
            //    item.Opacity = 0;
            //}

            // 2. Create the return storyboard, and the properties we're going to use
            Storyboard sb = new Storyboard();
            TimeSpan startOffset = TimeSpan.FromSeconds(0);
            TimeSpan staggerTime = customStagger != null && customStagger.HasValue ? customStagger.Value : TimeSpan.FromMilliseconds(25);
            TimeSpan duration = customDuration != null && customDuration.HasValue ? customDuration.Value : TimeSpan.FromMilliseconds(500);
            TimeSpan durationOpacity = TimeSpan.FromMilliseconds(150);

            EasingFunctionBase ease = new ExponentialEase { EasingMode = EasingMode.EaseIn, Exponent = 6 };

            // 3. Now let's build the storyboard!
            for (int i = 0; i < _items.Count; i++)
            {
                // 3.0. Get the item and it's opacity 
                FrameworkElement item = _items[i];
                CompositeTransform3D trans = item.GetCompositeTransform3D();


                // 3.1. Check AddedDelay
                startOffset = startOffset.Add(Properties.GetAddedDelay(item));

                // 3.2. Animate the opacity
                sb.CreateTimeline<DoubleAnimationUsingKeyFrames>(item, TargetProperty.Opacity)
                    .AddKeyFrame(TimeSpan.Zero, item.Opacity)
                    .AddKeyFrame(startOffset, item.Opacity)
                    .AddKeyFrame(startOffset.Add(durationOpacity), 0, KeySplines.DepthZoomOpacity);

                // 3.3. Animate the 3D depth translation
                if (endDepth != 0)
                {
                    var dbX = Animation.CreateTimeline<DoubleAnimationUsingKeyFrames>(item, TargetProperty.CompositeTransform3D.TranslateZ, sb);
                    dbX.AddKeyFrame(TimeSpan.Zero, trans.TranslateZ);
                    dbX.AddKeyFrame(startOffset, trans.TranslateZ);
                    dbX.AddKeyFrame(startOffset.Add(duration), endDepth, KeySplines.EntranceTheme);
                }

                // 3.4. Increment start offset
                startOffset = startOffset.Add(staggerTime);
            }

            return sb;
        }
    }

}
