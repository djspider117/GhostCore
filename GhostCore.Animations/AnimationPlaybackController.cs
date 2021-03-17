using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GhostCore.Animations
{
    public class AnimationPlaybackController : IDisposable
    {
        public event EventHandler FrameDispatched;

        private FoundationComposition _comp;
        private Timer _timer;

        public float CurrentTime { get; private set; }

        public AnimationPlaybackController(FoundationComposition comp)
        {
            _comp = comp;
            _timer = new Timer
            {
                Interval = 1000 / _comp.FrameRate,
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
            if (_comp.Duration != 0 && CurrentTime >= _comp.Duration)
            {
                switch (_comp.WrapMode)
                {
                    case CompositionWrapMode.PlayAndStop:
                        _timer.Stop();
                        break;
                    case CompositionWrapMode.PlayAndReset:
                        _timer.Stop();
                        CurrentTime = 0;
                        break;
                    case CompositionWrapMode.Loop:
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
