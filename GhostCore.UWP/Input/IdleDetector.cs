using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace GhostCore.UWP.Input
{
    public class IdleDetector
    {
        #region Singleton

        private static volatile IdleDetector _instance;
        private static readonly object _syncRoot = new object();

        public static IdleDetector Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new IdleDetector();
                    }
                }

                return _instance;
            }
        }
        private IdleDetector()
        {
        }

        #endregion

        #region Events

        public event EventHandler Idled;
        public event EventHandler Resumed;
       
        #endregion

        #region Fields

        private CoreWindow _window;
        private int? _timeoutMinutes;
        private DispatcherTimer _timer;
        private int _timesTicked;

        #endregion

        #region Properties

        public bool IsInitialized { get; set; }
        public bool IsStarted { get; set; }

        #endregion

        #region Initialization

        public void Initialize(CoreWindow wnd)
        {
            IsInitialized = true;
            _window = wnd;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };

            _timer.Tick += Timer_Tick;
        }

        public void SetTimeout(int? timeoutMinutes)
        {
            _timeoutMinutes = timeoutMinutes;
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

                _timesTicked = 0;

                // only start if there is a timeout set
                if (_timeoutMinutes != null)
                {
                    IsStarted = true;
                    _timer.Start();
                }
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
                IsStarted = false;
            }
        }

        public void Reset()
        {
            if (IsStarted)
            {
                Resumed?.Invoke(this, EventArgs.Empty);

                _timer.Stop();
                _timer.Start();
                _timesTicked = 0;
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
            _timesTicked++;

            if (_timeoutMinutes == null)
            {
                _timer.Stop();
            }

            if (_timesTicked >= (_timeoutMinutes ?? 1))
            {
                Idled?.Invoke(this, EventArgs.Empty);
                _timer.Stop();
            }
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
