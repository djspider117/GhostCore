/*
    This file is part of Project Emilie.
    Public redistribution is not permitted.
*/

using GhostCore.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace GhostCore.UWP.Media
{
    public static class CompositionFactory
    {
        private static Dictionary<Compositor, CompositionBackdropBrush> _backdropCache { get; } = new Dictionary<Compositor, CompositionBackdropBrush>();
        private static Dictionary<Compositor, ExpressionAnimation> _centeringCache { get; } = new Dictionary<Compositor, ExpressionAnimation>();

        public static bool AreEffectsSupported => CompositionCapabilities.GetForCurrentView().AreEffectsSupported();




        //------------------------------------------------------
        //
        //  Factories
        //
        //------------------------------------------------------

        private static CanvasDevice _sharedDevice = null;
        public static CanvasDevice SharedDevice
            => _sharedDevice ?? (_sharedDevice = CanvasDevice.GetSharedDevice());

        private static CompositionGraphicsDevice _compDevice = null;
        public static CompositionGraphicsDevice CompositionGraphicsDevice
            => _compDevice ?? (_compDevice = CanvasComposition.CreateCompositionGraphicsDevice(Composition.Compositor, SharedDevice));

        public static CompositionBackdropBrush GetSharedBackdropBrush(Compositor compositor)
        {
            if (_backdropCache.TryGetValue(compositor, out var brush))
                return brush;

            var newBrush = compositor.CreateBackdropBrush();
            _backdropCache[compositor] = newBrush;
            return newBrush;
        }




        //------------------------------------------------------
        //
        //  Easing
        //
        //------------------------------------------------------

        #region Easing

        public static CubicBezierEasingFunction GetEasingFunction(this Visual visual, PennerType type, PennerVariation variation, bool cache = true)
        {
            return CompositionEasings.GetEasingFunction(visual.Compositor, type, variation, cache);
        }

        public static CubicBezierEasingFunction GetEasingFunction(this Compositor compositor, PennerType type, PennerVariation variation, bool cache = true)
        {
            return CompositionEasings.GetEasingFunction(compositor, type, variation, cache);
        }

        #endregion




        //------------------------------------------------------
        //
        //  Expression Animation : PERSPECTIVE
        //
        //------------------------------------------------------

        #region Perspective Expression

        // Creates a basic 4x4 Matrix for use in perspective matrix multiplication. 
        // Depth of the matrix is automatically bound to the width of the visual.
        static string PERSPECTIVE_AUTO_DEPTH_MATRIX { get; } =
            $"Matrix4x4.CreateTranslation(-this.Target.Size.X /2f, -this.Target.Size.Y /2f, 0f) " +
            $"* Matrix4x4(1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, -1.0f / this.Target.Size.X, 0.0f, 0.0f, 0.0f, 1.0f) " +
            $"* Matrix4x4.CreateTranslation(this.Target.Size.X /2f, this.Target.Size.Y /2f, 0f)";

        // Creates a basic 4x4 Matrix for use in perspective matrix multiplication.
        // To match the default matrix created by XAML PerspectiveTransform3D, set depth to 1000f
        static string PERSPECTIVE_DEPTH_MATRIX(float depth) =>
            $"Matrix4x4.CreateTranslation(-this.Target.Size.X /2f, -this.Target.Size.Y /2f, 0f) " +
            $"* Matrix4x4(1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, -1.0f / {depth}f, 0.0f, 0.0f, 0.0f, 1.0f)" +
            $"* Matrix4x4.CreateTranslation(this.Target.Size.X /2f, this.Target.Size.Y /2f, 0f)";

        /// <summary>
        /// Creates a perspective matrix animation whose depth matches that of the Target
        /// Visual's width.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static ExpressionAnimation CreateAutoPerspectiveAnimation(Visual visual)
        {
            return visual.Compositor.CreateExpressionAnimation()
                .SetExpression(PERSPECTIVE_AUTO_DEPTH_MATRIX)
                .SetTarget(nameof(Visual.TransformMatrix));
        }

        /// <summary>
        /// Creates a perspective matrix animation with the specified depth.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static ExpressionAnimation CreatePerspectiveAnimation(Visual visual, float depth = 1000f)
        {
            return visual.Compositor.CreateExpressionAnimation()
                .SetExpression(PERSPECTIVE_DEPTH_MATRIX(depth))
                .SetTarget(nameof(Visual.TransformMatrix));
        }

        /// <summary>
        /// Applies a perspective transform matrix allowing CompositeTransform3D style 
        /// faux 3D rotation with a depth that is bound to the width of the object.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static CompositionAnimation EnableAutoPerspectiveMatrix(Visual visual)
        {
            var ani = CreateAutoPerspectiveAnimation(visual);
            visual.StartAnimation(ani);
            return ani;
        }

        public static CompositionAnimation EnableAutoPerspectiveMatrix(UIElement element)
        {
            var visual = element.GetVisual();
            var ani = CreateAutoPerspectiveAnimation(visual);
            visual.StartAnimation(ani);
            return ani;
        }

        /// <summary>
        /// Applies a perspective transform matrix allowing CompositeTransform3D style 
        /// faux 3D rotation. The default depth makes this comparable to the default
        /// matrix created by <see cref="PerspectiveTransform3D"/>
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="depth">Depth of the animation. 1000 is the default. Should be greater than 0.</param>
        /// <returns></returns>
        public static CompositionAnimation EnablePerspectiveMatrix(Visual visual, float depth = 1000f)
        {
            var ani = CreatePerspectiveAnimation(visual, depth);
            visual.StartAnimation(ani);
            return ani;
        }

        #endregion




        //------------------------------------------------------
        //
        //  Expression Animation : CENTERING
        //
        //------------------------------------------------------

        #region Centering Expression

        /*
         * An expression used to centre the "Offset" property of a visual relative
         * too itself
         */

        private static string CENTRE_EXPRESSION(float x, float y) =>
            $"({nameof(Vector3)}({CompositionProperty.Target}.{nameof(Visual.Size)}.{nameof(Vector2.X)} * {x}f, " +
            $"{CompositionProperty.Target}.{nameof(Visual.Size)}.{nameof(Vector2.Y)} * {y}f, 0f))";


        /// <summary>
        /// Warning: returns a cache expression
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static ExpressionAnimation CreateCenteringExpression(
            double x = 0.5,
            double y = 0.5)
        {
            return Composition.Compositor.CreateExpressionAnimation()
                    .SetExpression(CENTRE_EXPRESSION((float)x, (float)y))
                    .SetTarget(nameof(Visual.CenterPoint));
        }

        public static ExpressionAnimation GetCenteringExpression(Compositor compositor)
        {
            if (_centeringCache.TryGetValue(compositor, out ExpressionAnimation ani))
                return ani;

            ani = compositor.CreateExpressionAnimation()
                     .SetExpression(CENTRE_EXPRESSION(0.5f, 0.5f))
                     .SetTarget(nameof(Visual.CenterPoint));

            _centeringCache.Add(compositor, ani);
            return ani;
        }

        public static CompositionAnimation StartCentreing(FrameworkElement element)
        {
            if (element == null)
                return null;

            return StartCentering(element.GetVisual());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visual"></param>
        /// <returns>A shared, cached expression animation used for centring a visual</returns>
        public static ExpressionAnimation StartCentering(Visual visual)
        {
            if (visual == null)
                return null;

            var ani = GetCenteringExpression(visual.Compositor);
            visual.StartAnimation(ani);
            return ani;
        }

        #endregion




        //------------------------------------------------------
        //
        // Expression Animation : SIZE LINKING
        //
        //------------------------------------------------------

        #region Linked Size Expression

        /*
         * An expression that matches the size of a visual to another.
         * Useful for keeping shadows etc. in size sync with their target.
         */

        static string LINKED_SIZE_EXPRESSION { get; } = $"{nameof(Visual)}.{nameof(Visual.Size)}";

        public static ExpressionAnimation CreateLinkedSizeExpression(FrameworkElement sourceElement)
        {
            return CreateLinkedSizeExpression(sourceElement.GetVisual());
        }


        public static ExpressionAnimation CreateLinkedSizeExpression(Visual sourceVisual)
        {
            return sourceVisual.CreateExpressionAnimation(nameof(Visual.Size))
                               .SetParameter(nameof(Visual), sourceVisual)
                               .SetExpression(LINKED_SIZE_EXPRESSION);
        }

        /// <summary>
        /// Starts an Expression Animation that links the size of <paramref name="sourceVisual"/> to the <paramref name="targetVisual"/>
        /// </summary>
        /// <param name="targetVisual">Element whose size you want to automatically change</param>
        /// <param name="sourceVisual"></param>
        /// <returns></returns>
        public static T LinkSize<T>(this T targetVisual, Visual sourceVisual) where T : Visual
        {
            targetVisual.StartAnimation(CreateLinkedSizeExpression(sourceVisual));
            return targetVisual;
        }

        /// <summary>
        /// Starts an Expression Animation that links the size of <paramref name="element"/> to the <paramref name="targetVisual"/>
        /// </summary>
        /// <param name="targetVisual">Element whose size you want to automatically change</param>
        /// <param name="element">Element whose size will change <paramref name="targetVisual"/>s size</param>
        /// <returns></returns>
        public static T LinkSize<T>(this T targetVisual, FrameworkElement element) where T : Visual
        {
            targetVisual.StartAnimation(CreateLinkedSizeExpression(element.GetVisual()));
            return targetVisual;
        }

        #endregion




        //------------------------------------------------------
        //
        //  Misc. Animations
        //
        //------------------------------------------------------

        public static void SetStandardRepositionAnimation(FrameworkElement element, double duration = 0.3)
        {
            if (element == null)
                return;

            var vis = element.GetVisual();
            if (vis.ImplicitAnimations == null)
                vis.ImplicitAnimations = vis.Compositor.CreateImplicitAnimationCollection();

            vis.ImplicitAnimations[nameof(Visual.Offset)] =
                vis.CreateVector3KeyFrameAnimation(nameof(Visual.Offset))
                    .AddExpressionKeyFrame(1, CompositionProperty.FinalValue)
                    .SetDuration(duration);
        }


        /// <summary>
        /// Creates a DropShadow on a SpriteVisual from the <paramref name="sourceElement"/>, and sets it as
        /// the overlay visual of the <paramref name="shadowHost"/>
        /// </summary>
        /// <param name="element"></param>
        /// <param name="blurRadius">Default radius of the shadow</param>
        /// <param name="color">Default color of the shadow</param>
        /// <returns></returns>
        public static (SpriteVisual ContainerVisual, DropShadow Shadow) CreateDropShadow(
            FrameworkElement sourceElement,
            FrameworkElement shadowHost,
            double blurRadius,
            Color color,
            float opacity = 1f,
            float offsetX = 0f,
            float offsetY = 0f,
            float offsetZ = 0f)
        {
            Visual vis = sourceElement.GetVisual();
            Compositor c = vis.Compositor;

            SpriteVisual sprite = c.CreateSpriteVisual();

            DropShadow shadow = c.CreateDropShadow();
            shadow.Color = color;
            shadow.BlurRadius = (float)blurRadius;
            shadow.Opacity = 1f;
            sprite.Shadow = shadow;
            shadow.Offset = new Vector3(offsetX, offsetY, offsetZ);

            if (Composition.GetAlphaMask(sourceElement) is CompositionBrush mask)
            {
                shadow.Mask = mask;
                shadow.SourcePolicy = CompositionDropShadowSourcePolicy.Default;
            }

            if (shadowHost != null)
            {
                // Insert shadow sprites into container visuals rather than directly into an element - 
                // this allows you to put *multiple* shadows on an element if you wish.
                if (!(ElementCompositionPreview.GetElementChildVisual(shadowHost) is ContainerVisual container))
                {
                    container = c.CreateContainerVisual();
                    CompositionFactory.LinkSize(container, shadowHost);
                    ElementCompositionPreview.SetElementChildVisual(shadowHost, container);
                }

                container.Children.InsertAtTop(sprite);
                CompositionFactory.LinkSize(sprite, shadowHost);
            }

            return (sprite, shadow);
        }




        /// <summary>
        /// Creates a "Fade" animation using Composition
        /// </summary>
        /// <param name="element"></param>
        /// <param name="duration"></param>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        public static CompositionTimeline CreateFade(FrameworkElement element, TimeSpan duration, float to, float? from = null, TimeSpan? delay = null, CompositionEasingFunction easing = null)
        {
            return CreateFade(element.GetVisual(), duration, to, from, delay, easing);
        }

        /// <summary>
        /// Creates a fade animation using composition
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="duration"></param>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        public static CompositionTimeline CreateFade(Visual visual, TimeSpan duration, float to, float? from = null, TimeSpan? delay = null, CompositionEasingFunction easing = null)
        {
            var op = visual.CreateScalarKeyFrameAnimation(nameof(Visual.Opacity))
                .SetDuration(duration);

            if (from != null && from.HasValue)
            {
                if (easing == null)
                    op.InsertKeyFrame(0, from.Value);
                else
                    op.InsertKeyFrame(0, from.Value, easing);
            }
            else
            {
                if (easing == null)
                    op.InsertExpressionKeyFrame(0, CompositionProperty.StartingValue);
                else
                    op.InsertExpressionKeyFrame(0, CompositionProperty.StartingValue, easing);
            }

            if (easing == null)
                op.InsertKeyFrame(1, to);
            else
                op.InsertKeyFrame(1, to, easing);


            if (delay != null && delay.HasValue)
                op.DelayTime = delay.Value;

            return new CompositionTimeline(visual, op);
        }

        public static CompositionTimeline CreateSlide(FrameworkElement element, Vector2 position)
        {
            var vis = element.GetVisual();
            var vec = new Vector3(position, 0);

            var t = vis.CreateVector3KeyFrameAnimation(nameof(Visual.Offset))
                .SetDuration(TimeSpan.FromMilliseconds(300))
                .AddExpressionKeyFrame(0, CompositionProperty.StartingValue)
                .AddKeyFrame(1, vec);

            return new CompositionTimeline(vis, t);
        }


        public static CompositionTimeline CreateSlideWithTranslate(FrameworkElement element, Vector2 position, TimeSpan? duration = null, CompositionEasingFunction easing = null)
        {
            ElementCompositionPreview.SetIsTranslationEnabled(element, true);
            var vis = element.GetVisual();
            var vec = new Vector3(position, 0);

            var translate = vis
                .CreateVector3KeyFrameAnimation(CompositionProperty.Translation)
                .SetDuration(duration ?? TimeSpan.FromMilliseconds(300))
                .AddExpressionKeyFrame(0, CompositionProperty.StartingValue, easing)
                .AddKeyFrame(1, vec, easing);

            return new CompositionTimeline(vis, translate);
        }

        public static CompositionTimeline CreateSlide(FrameworkElement element, Vector2 from, Vector2? to = null, TimeSpan? duration = null)
        {
            var vis = element.GetVisual();
            var vec = to.HasValue ? new Vector3(to.Value, 0) : vis.Offset;

            vis.Offset = new Vector3(from, 0);

            var t = vis.CreateVector3KeyFrameAnimation(nameof(Visual.Offset))
                .SetDuration(duration ?? TimeSpan.FromMilliseconds(300))
                .AddExpressionKeyFrame(0, CompositionProperty.StartingValue)
                .AddKeyFrame(1, vec);

            return new CompositionTimeline(vis, t);
        }

        /// <summary>
        /// An analogue of <see cref="AnimationFactory.CreateEntranceTheme(IEnumerable{FrameworkElement}, double, double, TimeSpan?)"/>, although
        /// does not filter items list. You should do this before had. 
        /// WARNING: Requires UI thread access due to use of <see cref="Animation.AddedDelayProperty"/>
        /// </summary>
        public static CompositionStoryboard CreateEntranceTheme<T>(
           IList<T> items,
           float verticalOffset = 0,
           float horizontalOffset = 0,
           TimeSpan? customStagger = null,
           TimeSpan? customStartDelay = null,
           TimeSpan? customDuration = null,
           float? endOpacity = null) where T : FrameworkElement
        {
            // 0. Prepare ourselves a nice list of visible framework elements to animate
            List<T> _items = items.ToList();

            List<double> _opacitys = new List<double>();

            // 1. Hide them all to prevent flickering
            foreach (var element in _items)
            {
                Visual v = element.GetVisual();

                // 1.1. Record the original opacity value - we'll animate to this value later
                _opacitys.Add(v.Opacity);

                // 1.2. Hide the item
                v.Opacity = 0;
            }

            // 2. Create the return storyboard, and the properties we're going to use
            CompositionStoryboard storyboard = new CompositionStoryboard();
            if (_items.Count == 0)
                return storyboard;

            TimeSpan startOffset = customStartDelay ?? TimeSpan.FromSeconds(0);
            TimeSpan staggerTime = customStagger ?? TimeSpan.FromMilliseconds(83);
            TimeSpan duration = customDuration ?? TimeSpan.FromMilliseconds(1000);
            TimeSpan durationOpacity = TimeSpan.FromMilliseconds(duration.TotalMilliseconds * 0.3);

            var ease = items[0].GetVisual().Compositor.CreateCubicBezierEasingFunction(KeySplines.EntranceTheme.ToBezierPoints());

            // 3. Now let's build the storyboard!
            for (int i = 0; i < _items.Count; i++)
            {
                // 3.0. Get the item and it's opacity 
                FrameworkElement item = _items[i];
                float _originalOpacity = endOpacity ?? (float)_opacitys[i];

                Visual visual = item.GetVisual();
                item.EnableCompositionTranslation();

                // 3.1. Check AddedDelay
                startOffset = startOffset.Add(Properties.GetAddedDelay(item));

                // 3.2. Animate the opacity
                var group = visual.Compositor.CreateAnimationGroup();

                group.Add(
                    visual.CreateScalarKeyFrameAnimation(nameof(Visual.Opacity))
                    .SetDelayTime(startOffset)
                    .SetDuration(durationOpacity)
                    .AddKeyFrame(0, 0)
                    .AddKeyFrame(1, _originalOpacity, ease));

                var dur = duration.TotalMilliseconds + startOffset.TotalMilliseconds;

                group.Add(
                    visual.CreateVector3KeyFrameAnimation(CompositionProperty.Translation)
                    .SetDuration(TimeSpan.FromMilliseconds(dur))
                    .AddKeyFrame(0, horizontalOffset, verticalOffset)
                    .AddKeyFrame((float)startOffset.TotalMilliseconds / (float)dur, horizontalOffset, verticalOffset)
                    .AddKeyFrame(1, 0, ease));

                // 3.4. Increment start offset
                startOffset = startOffset.Add(staggerTime);

                storyboard.Add(new CompositionTimeline(visual, group));
            }

            return storyboard;
        }

    }
}