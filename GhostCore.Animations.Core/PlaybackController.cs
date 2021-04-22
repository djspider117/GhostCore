using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace GhostCore.Animations.Core
{
    public class PlaybackController : IDisposable
    {
        public event EventHandler<float> FrameDispatched;

        private ITimeline _timeline;
        private IScene _scene;
        private Timer _timer;

        private volatile float _curTime;

        public float CurrentTime
        {
            get { return _curTime; }
            set { _curTime = value; }
        }

        public double RelativeTime => (double)(CurrentTime * 1000 / _timeline.Duration);

        public PlaybackController(IScene scene, ITimeline timeline)
        {
            _timeline = timeline;
            _scene = scene;
            _timer = new Timer
            {
                Interval = 1000 / _scene.RenderInfo.FrameRate,
            };
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _curTime += ((float)_timer.Interval) / 1000;
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

            FrameDispatched?.Invoke(this, _curTime);
        }

    }

    public enum SeekBehaviour
    {
        ContinuePlaying,
        PauseRendering
    }
}
