using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace GhostCore.Animations.Core
{
    public class PlaybackController : IDisposable
    {
        public event EventHandler FrameDispatched;

        private ITimeline _timeline;
        private IScene _scene;
        private Timer _timer;

        public float CurrentTime { get; private set; }

        public PlaybackController(IScene scene, ITimeline timeline)
        {
            _timeline = timeline;
            _scene = scene;
            _timer = new Timer
            {
                Interval = 1000 / _scene.RenderInfo.FrameRate,
                AutoReset = true
            };
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CurrentTime += (float)_timer.Interval;
            TriggerRender();
        }

        public void Play()
        {
            _timer.Start();
        }

        public void Pause()
        {
            _timer.Stop();
        }

        public void Seek(float t, SeekBehaviour seekBehaviour = SeekBehaviour.PauseRendering)
        {
            CurrentTime = t;
            TriggerRender();

            if (seekBehaviour == SeekBehaviour.PauseRendering)
            {
                _timer.Stop();
            }
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Elapsed -= Timer_Elapsed;
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }


        private void TriggerRender()
        {
            if (_timeline.Duration != 0 && CurrentTime >= _timeline.Duration)
            {
                switch (_timeline.WrapMode)
                {
                    case PlayableWrapMode.PlayOnce:
                        _timer.Stop();
                        break;
                    case PlayableWrapMode.PlayOnceAndReset:
                        _timer.Stop();
                        CurrentTime = 0;
                        break;
                    case PlayableWrapMode.Loop:
                        CurrentTime = 0;
                        break;
                    default:
                        break;
                }
            }

            FrameDispatched?.Invoke(this, EventArgs.Empty);
        }

    }

    public enum SeekBehaviour
    {
        ContinuePlaying,
        PauseRendering
    }
}
