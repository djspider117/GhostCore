/*
    This file is part of Project Emilie.
    Public redistribution is not permitted.
*/

using System;
using Windows.UI.Composition;

namespace GhostCore.UWP.Media
{
    public class CompositionTimeline : ICompositionTimeline
    {
        public CompositionAnimationGroup CompositionAnimationGroup { get; private set; }
        public CompositionAnimation CompositionAnimation { get; private set; }
        public CompositionObject Visual { get; private set; }
        public IDisposable[] Resources { get; private set; }
        public Compositor Compositor => Visual.Compositor;

        public ICompositionAnimationBase ICompositionAnimation
        {
            get { if (CompositionAnimation != null) return CompositionAnimation; else return CompositionAnimationGroup; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="animation"></param>
        /// <param name="resources">Resources used for the animation, like easingFunctions. Anything passed in here will be disposed when Dispose() is called on this object</param>
        public CompositionTimeline(CompositionObject visual, CompositionAnimation animation, params IDisposable[] resources)
        {
            if (animation == null)
                throw new ArgumentNullException(nameof(animation));

            CreateInternal(visual, animation, animation.Target, resources);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="animation"></param>
        /// <param name="targetProperty"></param>
        /// <param name="resources">Resources used for the animation, like easingFunctions. Anything passed in here will be disposed when Dispose() is called on this object</param>
        public CompositionTimeline(CompositionObject visual, CompositionAnimation animation, String targetProperty, params IDisposable[] resources)
        {
            CreateInternal(visual, animation, targetProperty, resources);
        }

        void CreateInternal(CompositionObject visual, CompositionAnimation animation, String targetProperty, params IDisposable[] resources)
        {
            Visual = visual ?? throw new ArgumentNullException(nameof(visual));
            CompositionAnimation = animation ?? throw new ArgumentNullException(nameof(animation));

            if (!String.IsNullOrEmpty(targetProperty))
            {
                CompositionAnimation.Target = targetProperty;
            }
            else if (String.IsNullOrEmpty(CompositionAnimation.Target))
            {
                throw new ArgumentNullException(
                    nameof(targetProperty),
                    $"An animation target must be specified either on the Target property of the \"{animation}\" parameter, or as the \"{targetProperty}\" parameter");
            }

            Resources = resources;
        }

        public static CompositionTimeline Create(CompositionObject visual, CompositionAnimation animation, params IDisposable[] resources)
        {
            return new CompositionTimeline(visual, animation, resources);
        }

        public static CompositionTimeline Create(CompositionObject visual, CompositionAnimationGroup animation, params IDisposable[] resources)
        {
            return new CompositionTimeline(visual, animation, resources);
        }

        public CompositionTimeline(CompositionObject visual, CompositionAnimationGroup animation, params IDisposable[] resources)
        {
            Visual = visual ?? throw new ArgumentNullException(nameof(visual));
            CompositionAnimationGroup = animation ?? throw new ArgumentNullException(nameof(animation));

            Resources = resources;
        }

        public void Start()
        {
            if (CompositionAnimationGroup != null)
            {
                Visual.StartAnimationGroup(CompositionAnimationGroup);
            }
            else if (CompositionAnimation != null)
            {
                Visual.StartAnimation(CompositionAnimation.Target, CompositionAnimation);
            }
        }

        public void Stop()
        {
            if (CompositionAnimationGroup != null)
            {
                Visual.StopAnimationGroup(CompositionAnimationGroup);
            }
            else if (CompositionAnimation != null)
            {
                Visual.StopAnimation(CompositionAnimation.Target);
            }
        }


        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    CompositionAnimation = Composition.SafeDispose(CompositionAnimation);
                    CompositionAnimationGroup = Composition.SafeDispose(CompositionAnimationGroup);

                    if (Resources != null)
                    {
                        for (int i = 0; i < Resources.Length; i++)
                        {
                            var resource = Resources[i];
                            if (resource is ICompositionTimeline csb)
                            {
                                csb.Stop();
                            }

                            Resources[i] = Composition.SafeDispose(resource);
                        }

                        Resources = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CompositionStoryboard() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
