using System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace GhostCore.UWP.Input
{
    public class IdleDetector
    {
        #region Singleton

        public static IdleDetector Instance { get; } = new IdleDetector();

        #endregion

        #region Events

        public event EventHandler Idled;
        public event EventHandler Resumed;

        #endregion

        #region Fields

        private CoreWindow _window;
        private DispatcherTimer _timer;

        #endregion

        #region Properties

        public bool IsInitialized { get; set; }
        public bool IsStarted => _timer.IsEnabled;

        #endregion

        #region Initialization

        public void Initialize(CoreWindow wnd, TimeSpan? timeout = null)
        {
            IsInitialized = true;
            _window = wnd;

            SetTimeout(timeout);

            _timer.Tick += Timer_Tick;
        }

        public void SetTimeout(TimeSpan? timeout = null)
        {
            _timer = new DispatcherTimer
            {
                Interval = timeout ?? TimeSpan.FromMinutes(1)
            };

            _timer.Tick += Timer_Tick;
        }

        #endregion

        #region API

        public void Start()
        {
            CheckInitialized();

            if (!IsStarted)
            {
                _window.PointerPressed += Window_PointerPressed;
                _window.PointerReleased += Window_PointerReleased;
                //_window.PointerMoved += window_PointerMoved;
                _window.KeyDown += Window_KeyDown; // use KeyDown for soft keys
                _window.CharacterReceived += Window_CharacterReceived; // text suggestions or chords

                _timer.Start();
            }
        }

        public void Stop()
        {
            CheckInitialized();

            if (IsStarted)
            {
                _window.PointerPressed -= Window_PointerPressed;
                _window.PointerReleased -= Window_PointerReleased;
                //_window.PointerMoved -= window_PointerMoved;
                _window.KeyDown -= Window_KeyDown;
                _window.CharacterReceived -= Window_CharacterReceived;

                _timer.Stop();
            }
        }

        public void Reset()
        {
            if (IsStarted)
            {
                Resumed?.Invoke(this, EventArgs.Empty);

                _timer.Stop();
                _timer.Start();
            }
        }

        public void ForceIdle()
        {
            Idled?.Invoke(this, EventArgs.Empty);
            _timer.Stop();
        }

        #endregion

        #region Event handlers

        private void Timer_Tick(object sender, object e)
        {
            Idled?.Invoke(this, EventArgs.Empty);
            _timer.Stop();
        }

        private void Window_PointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            Reset();
        }
        private void Window_PointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            Reset();
        }
        private void Window_PointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            Reset();
        }
        private void Window_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            Reset();
        }
        private void Window_CharacterReceived(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            Reset();
        }

        #endregion

        private void CheckInitialized()
        {
            if (!IsInitialized)
                throw new Exception("GlobalWindowEventsManager has not been initialized yet ! Please initialize.");
        }
    }
}
