/*
    This file is part of Project Emilie.
    Public redistribution is not permitted.
*/

using GhostCore.Numerics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Shapes;

namespace GhostCore.UWP.Media
{

    /// <summary>
    /// Helpful extensions methods to enable you to write fluent Composition animations
    /// </summary>
    public static class Composition
    {
        /*
         * NOTE
         * 
         * Type constraints on extension methods do not form part of the method
         * signature used for choosing a correct method. Therefore two extensions
         * with the same parameters but different type constraints will conflict
         * with each other.
         * 
         * Due to this, some methods here use type constraints whereas other that
         * conflict with the XAML storyboarding extensions use explicit type
         * extensions. When adding methods, please keep in mind whether it's 
         * possible some other toolkit might have a similar signature for extensions
         * to form your plan of attack
         */

        #region Fundamentals

        public static T SafeDispose<T>(T disposable) where T : IDisposable
        {
            disposable?.Dispose();
            return default;
        }

        public static Compositor Compositor { get; set; } = Window.Current.Compositor;

        /// <summary>
        /// Starts a Composition Storyboard that cleans itself upon completion
        /// </summary>
        /// <param name="storyboard"></param>
        public static void Begin(this CompositionTimeline storyboard)
        {
            storyboard.Visual.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation,
                (batch) => { storyboard?.Start(); },
                (batch) => { storyboard?.Dispose(); });
        }

        public static Task<bool> BeginAsync(this CompositionTimeline storyboard)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            storyboard.Visual.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation,
                (batch) => { storyboard?.Start(); },
                (batch) => { storyboard?.Dispose(); tcs.SetResult(true); });

            return tcs.Task;
        }

        /// <summary>
        /// Starts a Composition Storyboard Collection that cleans itself up on completion
        /// </summary>
        /// <param name="storyboard"></param>
        /// <param name="compositor"></param>
        public static void Begin(this CompositionStoryboard storyboard)
        {
            if (storyboard.Storyboards.Count > 0)
            {
                storyboard.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation,
                    (batch) => { storyboard?.Start(); },
                    (batch) => { storyboard?.Dispose(); });
            }
        }

        public static Task<bool> BeginAsync(this CompositionStoryboard storyboard)
        {
            if (storyboard.Storyboards.Count == 0)
                return Task.FromResult(false);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            storyboard.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation,
                (batch) => { storyboard?.Start(); },
                (batch) => { storyboard?.Dispose(); tcs.SetResult(true); });

            return tcs.Task;
        }

        public static void CreateScopedBatch(this Compositor compositor,
            CompositionBatchTypes batchType,
            Action<CompositionScopedBatch> action,
            Action<CompositionScopedBatch> onCompleteAction = null)
        {
            if (action == null)
                throw
                  new ArgumentException("Cannot create a scoped batch on an action with null value!",
                  nameof(action));

            // Create ScopedBatch
            var batch = compositor.CreateScopedBatch(batchType);

            //// Handler for the Completed Event
            void handler(object s, CompositionBatchCompletedEventArgs ea)
            {
                // Unsubscribe the handler from the Completed Event
                ((CompositionScopedBatch)s).Completed -= handler;

                try
                {
                    // Invoke the post action
                    onCompleteAction?.Invoke(batch);
                }
                finally
                {
                    ((CompositionScopedBatch)s).Dispose();
                }
            }

            batch.Completed += handler;

            // Invoke the action
            action(batch);


            // End Batch
            batch.End();
        }

        #endregion


        #region Element / Base Extensions

        public static IEnumerable<Visual> GetVisuals<T>(this IEnumerable<T> elements) where T : UIElement
        {
            foreach (var item in elements)
                yield return item.GetVisual();
        }

        /// <summary>
        /// Returns the Composition Hand-off Visual for this framework element
        /// </summary>
        /// <param name="element"></param>
        /// <returns>Composition Hand-off Visual</returns>
        public static Visual GetVisual(this UIElement element) => element == null ? null : ElementCompositionPreview.GetElementVisual(element);

        public static ContainerVisual GetContainerVisual(this UIElement element, bool linkSize = true)
        {
            if (element == null)
                return null;

            if (ElementCompositionPreview.GetElementChildVisual(element) is ContainerVisual container)
                return container;

            // Create a new container visual, link it's size to the element's and then set
            // the container as the child visual of the element.
            container = element.GetVisual().Compositor.CreateContainerVisual();
            CompositionFactory.LinkSize(container, element.GetVisual());
            element.SetChildVisual(container);

            return container;
        }

        public static CompositionPropertySet GetScrollManipulationPropertySet(this ScrollViewer scrollViewer)
        {
            return ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scrollViewer);
        }

        public static void SetShowAnimation(this UIElement element, ICompositionAnimationBase animation)
        {
            ElementCompositionPreview.SetImplicitShowAnimation(element, animation);
        }

        public static void SetHideAnimation(this UIElement element, ICompositionAnimationBase animation)
        {
            ElementCompositionPreview.SetImplicitHideAnimation(element, animation);
        }

        public static void SetChildVisual(this UIElement element, Visual visual)
        {
            ElementCompositionPreview.SetElementChildVisual(element, visual);
        }

        public static bool SupportsAlphaMask(UIElement element)
        {
            switch (element)
            {
                case TextBlock _:
                case Shape _:
                case Image _:
                    return true;

                default:
                    return element is ISupportsAlphaMask;
            }
        }

        /// <summary>
        /// Attempts to get the AlphaMask from supported UI elements.
        /// Returns null if the element cannot create an AlphaMask.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static CompositionBrush GetAlphaMask(UIElement element)
        {
            switch (element)
            {
                case TextBlock t:
                    return t.GetAlphaMask();

                case Shape s:
                    return s.GetAlphaMask();

                case Image i:
                    return i.GetAlphaMask();

                default:
                    break;
            }

            if (element is ISupportsAlphaMask mask)
                return mask.GetAlphaMask();

            return null;
        }

        #endregion


        #region Translation

        public static IEnumerable<T> EnableCompositionTranslation<T>(this IEnumerable<T> elements) where T : UIElement
        {
            foreach (var element in elements)
            {
                element.EnableCompositionTranslation();
                yield return element;
            }
        }

        public static UIElement EnableCompositionTranslation(this UIElement element)
        {
            return EnableCompositionTranslation(element, null);
        }

        public static UIElement EnableCompositionTranslation(this UIElement element, float x, float y, float z)
        {
            return EnableCompositionTranslation(element, new Vector3(x, y, z));
        }

        public static UIElement EnableCompositionTranslation(this UIElement element, Vector3? defaultTranslation)
        {
            Visual visual = element.GetVisual();
            if (visual.Properties.TryGetVector3(CompositionProperty.Translation, out _) == CompositionGetValueStatus.NotFound)
            {
                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
                if (defaultTranslation.HasValue)
                    visual.Properties.InsertVector3(CompositionProperty.Translation, defaultTranslation.Value);
                else
                    visual.Properties.InsertVector3(CompositionProperty.Translation, new Vector3());
            }

            return element;
        }

        public static Vector3 GetTranslation(this Visual visual)
        {
            visual.Properties.TryGetVector3(CompositionProperty.Translation, out Vector3 translation);
            return translation;
        }

        public static Visual SetTranslation(this Visual visual, float x, float y, float z)
        {
            return SetTranslation(visual, new Vector3(x, y, z));
        }

        public static Visual SetTranslation(this Visual visual, Vector3 translation)
        {
            visual.Properties.InsertVector3(CompositionProperty.Translation, translation);
            return visual;
        }

        /// <summary>
        /// Sets the axis to rotate the visual around.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static Visual SetRotationAxis(this Visual visual, Vector3 axis)
        {
            visual.RotationAxis = axis;
            return visual;
        }

        /// <summary>
        /// Sets the axis to rotate the visual around.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Visual SetRotationAxis(this Visual visual, float x, float y, float z)
        {
            visual.RotationAxis = new Vector3(x, y, z);
            return visual;
        }

        public static Visual SetCenterPoint(this Visual visual, float x, float y, float z)
        {
            return SetCenterPoint(visual, new Vector3(x, y, z));
        }

        public static Visual SetCenterPoint(this Visual visual, Vector3 vector)
        {
            visual.CenterPoint = vector;
            return visual;
        }

        /// <summary>
        /// Sets the centre point of a visual to its current cartesian centre (relative 0.5f, 0.5f).
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static Visual SetCenterPoint(this Visual visual)
        {
            return SetCenterPoint(visual, new Vector3(visual.Size / 2f, 0f));
        }

        #endregion


        #region SetTarget

        public static T SetTarget<T>(this T animation, string target) where T : CompositionAnimation
        {
            animation.Target = target;
            return animation;
        }

        private static T SetSafeTarget<T>(this T animation, string target) where T : KeyFrameAnimation
        {
            if (!String.IsNullOrEmpty(target))
                animation.Target = target;

            return animation;
        }

        #endregion


        #region SetDelayTime

        /// <summary>
        /// Sets the delay time in seconds
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="animation"></param>
        /// <param name="delayTime">Delay Time in seconds</param>
        /// <returns></returns>
        public static T SetDelayTime<T>(this T animation, double delayTime)
            where T : KeyFrameAnimation
        {
            animation.DelayTime = TimeSpan.FromSeconds(delayTime);
            return animation;
        }

        public static T SetDelayTime<T>(this T animation, TimeSpan delayTime)
            where T : KeyFrameAnimation
        {
            animation.DelayTime = delayTime;
            return animation;
        }

        #endregion


        #region SetDuration

        /// <summary>
        /// Sets the duration
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="duration">Duration in seconds</param>
        /// <returns></returns>
        public static T SetDuration<T>(this T animation, double duration) where T : KeyFrameAnimation
        {
            return SetDuration(animation, TimeSpan.FromSeconds(duration));
        }

        public static T SetDuration<T>(this T animation, TimeSpan duration) where T : KeyFrameAnimation
        {
            animation.Duration = duration;
            return animation;
        }

        public static SpringVector3NaturalMotionAnimation SetPeriod(this SpringVector3NaturalMotionAnimation animation, double duration)
        {
            return SetPeriod(animation, TimeSpan.FromSeconds(duration));
        }

        public static SpringVector3NaturalMotionAnimation SetPeriod(this SpringVector3NaturalMotionAnimation animation, TimeSpan duration)
        {
            animation.Period = duration;
            return animation;
        }

        #endregion


        #region StopBehaviour

        public static T SetStopBehavior<T>(this T animation, AnimationStopBehavior stopBehavior) where T : KeyFrameAnimation
        {
            animation.StopBehavior = stopBehavior;
            return animation;
        }

        #endregion


        #region Direction

        public static T SetDirection<T>(this T animation, AnimationDirection direction) where T : KeyFrameAnimation
        {
            animation.Direction = direction;
            return animation;
        }

        #endregion


        #region Comment

        public static T SetComment<T>(this T obj, string comment) where T : CompositionObject
        {
            obj.Comment = comment;
            return obj;
        }

        #endregion


        #region IterationBehavior

        public static T SetIterationBehavior<T>(this T animation, AnimationIterationBehavior iterationBehavior) where T : KeyFrameAnimation
        {
            animation.IterationBehavior = iterationBehavior;
            return animation;
        }

        #endregion


        #region AddKeyFrame Builders

        public static T SetFinalValue<T>(this T animation, Vector3 finalValue) where T : Vector3NaturalMotionAnimation
        {
            animation.FinalValue = finalValue;
            return animation;
        }

        public static T SetFinalValue<T>(this T animation, float x, float y, float z) where T : Vector3NaturalMotionAnimation
        {
            animation.FinalValue = new Vector3(x, y, z);
            return animation;
        }

        public static T AddExpressionKeyFrame<T>(this T animation, float normalizedProgressKey, string value, CubicBezierControlPoints ease) where T : KeyFrameAnimation
        {
            animation.InsertExpressionKeyFrame(normalizedProgressKey, value, animation.Compositor.CreateCubicBezierEasingFunction(ease));
            return animation;
        }

        public static T AddExpressionKeyFrame<T>(this T animation, float normalizedProgressKey, string value, CompositionEasingFunction ease = null) where T : KeyFrameAnimation
        {
            animation.InsertExpressionKeyFrame(normalizedProgressKey, value, ease);
            return animation;
        }

        public static ScalarKeyFrameAnimation AddKeyFrame(this ScalarKeyFrameAnimation animation, float normalizedProgressKey, float value, CompositionEasingFunction ease = null)
        {
            animation.InsertKeyFrame(normalizedProgressKey, value, ease);
            return animation;
        }

        public static ScalarKeyFrameAnimation AddKeyFrame(this ScalarKeyFrameAnimation animation, float normalizedProgressKey, float value, CubicBezierControlPoints ease)
        {
            animation.InsertKeyFrame(normalizedProgressKey, value, animation.Compositor.CreateCubicBezierEasingFunction(ease));
            return animation;
        }

        public static ColorKeyFrameAnimation AddKeyFrame(this ColorKeyFrameAnimation animation, float normalizedProgressKey, Color value, CompositionEasingFunction ease = null)
        {
            animation.InsertKeyFrame(normalizedProgressKey, value, ease);
            return animation;
        }

        public static Vector2KeyFrameAnimation AddKeyFrame(this Vector2KeyFrameAnimation animation, float normalizedProgressKey, Vector2 value, CompositionEasingFunction ease = null)
        {
            animation.InsertKeyFrame(normalizedProgressKey, value, ease);
            return animation;
        }

        public static Vector3KeyFrameAnimation AddKeyFrame(this Vector3KeyFrameAnimation animation, float normalizedProgressKey, Vector3 value, CompositionEasingFunction ease = null)
        {
            animation.InsertKeyFrame(normalizedProgressKey, value, ease);
            return animation;
        }

        /// <summary>
        /// Adds a Vector3KeyFrame where the X & Y components are set to the input value and the Z component defaults to 0f.
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="normalizedProgressKey"></param>
        /// <param name="value"></param>
        /// <param name="ease"></param>
        /// <returns></returns>
        public static Vector3KeyFrameAnimation AddKeyFrame(this Vector3KeyFrameAnimation animation, float normalizedProgressKey, float value, CompositionEasingFunction ease = null)
        {
            animation.InsertKeyFrame(normalizedProgressKey, new Vector3(value, value, 0f), ease);
            return animation;
        }

        /// <summary>
        /// Adds a Vector3KeyFrame where the X & Y components are set to the input value and the Z component defaults to 1f.
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="normalizedProgressKey"></param>
        /// <param name="value"></param>
        /// <param name="ease"></param>
        /// <returns></returns>
        public static Vector3KeyFrameAnimation AddScaleKeyFrame(this Vector3KeyFrameAnimation animation, float normalizedProgressKey, float value, CompositionEasingFunction ease = null)
        {
            animation.InsertKeyFrame(normalizedProgressKey, new Vector3(value, value, 1f), ease);
            return animation;
        }

        /// <summary>
        /// Adds a Vector3KeyFrame where the X & Y components are set to the input value and the Z component defaults to 1f.
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="normalizedProgressKey"></param>
        /// <param name="value"></param>
        /// <param name="ease"></param>
        /// <returns></returns>
        public static Vector3KeyFrameAnimation AddScaleKeyFrame(this Vector3KeyFrameAnimation animation, float normalizedProgressKey, float value, CubicBezierControlPoints ease)
        {
            animation.InsertKeyFrame(normalizedProgressKey, new Vector3(value, value, 1f), animation.Compositor.CreateCubicBezierEasingFunction(ease));
            return animation;
        }

        /// <summary>
        /// Adds a Vector3KeyFrame using the X & Y components. The Z component defaults to 0f.
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="normalizedProgressKey"></param>
        /// <param name="x">X Component of the Vector3</param>
        /// <param name="y">Y Component of the Vector3</param>
        /// <param name="ease">Optional ease</param>
        /// <returns></returns>
        public static Vector3KeyFrameAnimation AddKeyFrame(this Vector3KeyFrameAnimation animation, float normalizedProgressKey, float x, float y, CompositionEasingFunction ease = null)
        {
            animation.InsertKeyFrame(normalizedProgressKey, new Vector3(x, y, 0f), ease);
            return animation;
        }

        public static Vector3KeyFrameAnimation AddKeyFrame(this Vector3KeyFrameAnimation animation, float normalizedProgressKey, float x, float y, CubicBezierControlPoints ease)
        {
            animation.InsertKeyFrame(normalizedProgressKey, new Vector3(x, y, 0f), animation.Compositor.CreateCubicBezierEasingFunction(ease));
            return animation;
        }

        /// <summary>
        /// Adds a Vector3KeyFrame with X Y & Z components.
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="normalizedProgressKey"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="ease"></param>
        /// <returns></returns>
        public static Vector3KeyFrameAnimation AddKeyFrame(this Vector3KeyFrameAnimation animation, float normalizedProgressKey, float x, float y, float z, CompositionEasingFunction ease = null)
        {
            animation.InsertKeyFrame(normalizedProgressKey, new Vector3(x, y, z), ease);
            return animation;
        }

        public static Vector4KeyFrameAnimation AddKeyFrame(this Vector4KeyFrameAnimation animation, float normalizedProgressKey, Vector4 value, CompositionEasingFunction ease = null)
        {
            animation.InsertKeyFrame(normalizedProgressKey, value, ease);
            return animation;
        }

        public static QuaternionKeyFrameAnimation AddKeyFrame(this QuaternionKeyFrameAnimation animation, float normalizedProgressKey, Quaternion value, CompositionEasingFunction ease = null)
        {
            animation.InsertKeyFrame(normalizedProgressKey, value, ease);
            return animation;
        }

        #endregion


        #region Compositor Create Builders

        public static ColorKeyFrameAnimation CreateColorKeyFrameAnimation(this CompositionObject visual, string targetProperty = null)
        {
            return visual.Compositor.CreateColorKeyFrameAnimation().SetSafeTarget(targetProperty);
        }

        public static ScalarKeyFrameAnimation CreateScalarKeyFrameAnimation(this CompositionObject visual, string targetProperty = null)
        {
            return visual.Compositor.CreateScalarKeyFrameAnimation().SetSafeTarget(targetProperty);
        }

        public static Vector2KeyFrameAnimation CreateVector2KeyFrameAnimation(this CompositionObject visual, string targetProperty = null)
        {
            return visual.Compositor.CreateVector2KeyFrameAnimation().SetSafeTarget(targetProperty);
        }

        public static Vector3KeyFrameAnimation CreateVector3KeyFrameAnimation(this CompositionObject visual, string targetProperty = null)
        {
            return visual.Compositor.CreateVector3KeyFrameAnimation().SetSafeTarget(targetProperty);
        }

        public static Vector4KeyFrameAnimation CreateVector4KeyFrameAnimation(this CompositionObject visual, string targetProperty = null)
        {
            return visual.Compositor.CreateVector4KeyFrameAnimation().SetSafeTarget(targetProperty);
        }

        public static QuaternionKeyFrameAnimation CreateQuaternionKeyFrameAnimation(this CompositionObject visual, string targetProperty = null)
        {
            return visual.Compositor.CreateQuaternionKeyFrameAnimation().SetSafeTarget(targetProperty);
        }

        public static ExpressionAnimation CreateExpressionAnimation(this CompositionObject visual)
        {
            return visual.Compositor.CreateExpressionAnimation();
        }

        public static ExpressionAnimation CreateExpressionAnimation(this CompositionObject visual, string targetProperty)
        {
            return visual.Compositor.CreateExpressionAnimation().SetTarget(targetProperty);
        }

        public static CubicBezierEasingFunction CreateCubicBezierEasingFunction(this Compositor compositor, float x1, float y1, float x2, float y2)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(x1, y1), new Vector2(x2, y2));
        }

        public static CubicBezierEasingFunction CreateCubicBezierEasingFunction(this Compositor compositor, CubicBezierControlPoints controlPoints)
        {
            return compositor.CreateCubicBezierEasingFunction(controlPoints.ControlPoint1, controlPoints.ControlPoint2);
        }

        public static CubicBezierEasingFunction CreateCubicBezierEasingFunction(this Compositor compositor, Windows.UI.Xaml.Media.Animation.KeySpline spline)
        {
            return compositor.CreateCubicBezierEasingFunction(spline.ToBezierPoints());
        }

        #endregion


        #region SetExpression

        public static ExpressionAnimation SetExpression(this ExpressionAnimation animation, string expression)
        {
            animation.Expression = expression;
            return animation;
        }

        #endregion


        #region SetParameter Builders

        public static T SetParameter<T>(this T animation, string key, UIElement parameter) where T : CompositionAnimation
        {
            animation.SetReferenceParameter(key, parameter.GetVisual());
            return animation;
        }

        public static T SetParameter<T>(this T animation, string key, CompositionObject parameter) where T : CompositionAnimation
        {
            animation.SetReferenceParameter(key, parameter);
            return animation;
        }

        public static T SetParameter<T>(this T animation, string key, float parameter) where T : CompositionAnimation
        {
            animation.SetScalarParameter(key, parameter);
            return animation;
        }

        public static T SetParameter<T>(this T animation, string key, Vector2 parameter) where T : CompositionAnimation
        {
            animation.SetVector2Parameter(key, parameter);
            return animation;
        }

        public static T SetParameter<T>(this T animation, string key, Vector3 parameter) where T : CompositionAnimation
        {
            animation.SetVector3Parameter(key, parameter);
            return animation;
        }

        public static T SetParameter<T>(this T animation, string key, Vector4 parameter) where T : CompositionAnimation
        {
            animation.SetVector4Parameter(key, parameter);
            return animation;
        }

        #endregion


        #region PropertySet Builders

        public static CompositionPropertySet SetValue(this CompositionPropertySet set, string name, float value)
        {
            set.InsertScalar(name, value);
            return set;
        }

        public static CompositionPropertySet SetValue(this CompositionPropertySet set, string name, bool value)
        {
            set.InsertBoolean(name, value);
            return set;
        }

        public static CompositionPropertySet SetValue(this CompositionPropertySet set, string name, Vector2 value)
        {
            set.InsertVector2(name, value);
            return set;
        }

        public static CompositionPropertySet SetValue(this CompositionPropertySet set, string name, Vector3 value)
        {
            set.InsertVector3(name, value);
            return set;
        }

        public static CompositionPropertySet SetValue(this CompositionPropertySet set, string name, Vector4 value)
        {
            set.InsertVector4(name, value);
            return set;
        }

        public static CompositionPropertySet SetValue(this CompositionPropertySet set, string name, Color value)
        {
            set.InsertColor(name, value);
            return set;
        }

        public static CompositionPropertySet SetValue(this CompositionPropertySet set, string name, Matrix3x2 value)
        {
            set.InsertMatrix3x2(name, value);
            return set;
        }

        public static CompositionPropertySet SetValue(this CompositionPropertySet set, string name, Matrix4x4 value)
        {
            set.InsertMatrix4x4(name, value);
            return set;
        }

        public static CompositionPropertySet SetValue(this CompositionPropertySet set, string name, Quaternion value)
        {
            set.InsertQuaternion(name, value);
            return set;
        }

        #endregion


        #region Animation Start / Stop

        public static void StartAnimation(this CompositionObject compositionObject, CompositionAnimation animation)
        {
            if (string.IsNullOrWhiteSpace(animation.Target))
                throw new ArgumentNullException("Animation has no target");

            compositionObject.StartAnimation(animation.Target, animation);
        }

        public static void StartAnimation(this CompositionObject compositionObject, CompositionAnimationGroup animation)
        {
            compositionObject.StartAnimationGroup(animation);
        }

        public static void StopAnimation(this CompositionObject compositionObject, CompositionAnimation animation)
        {
            if (string.IsNullOrWhiteSpace(animation.Target))
                throw new ArgumentNullException("Animation has no target");

            compositionObject.StopAnimation(animation.Target);
        }

        #endregion


        #region AsStoryboard

        public static CompositionTimeline AsStoryboard<T>(this T animation, Visual target) where T : CompositionAnimation
        {
            return new CompositionTimeline(target, animation);
        }

        #endregion


        #region Extras

        public static T SetImplicitAnimation<T>(this T composition, string path, ICompositionAnimationBase animation)
            where T : CompositionObject
        {
            if (composition.ImplicitAnimations == null)
            {
                composition.ImplicitAnimations = composition.Compositor.CreateImplicitAnimationCollection();
            }

            composition.ImplicitAnimations[path] = animation;
            return composition;
        }

        public static FrameworkElement SetImplicitAnimation(this FrameworkElement element, string path, ICompositionAnimationBase animation)
        {
            CompositionObject composition = element.GetVisual();

            if (composition.ImplicitAnimations == null)
            {
                composition.ImplicitAnimations = composition.Compositor.CreateImplicitAnimationCollection();
            }

            composition.ImplicitAnimations[path] = animation;
            return element;
        }

        #endregion
    }
}
