using GhostCore.MVVM;

namespace GhostCore.Animations.Editor.ViewModels
{
    public class TimelineViewModel : ViewModelBase
    {
        private float _startTime;
        private float _endTime;
        private float _horiOffset;
        private float _vertOffset;
        private float _currentTime;
        private float _maxZoomFactor = 55;

        public float MaxZoomFactor
        {
            get { return _maxZoomFactor; }
            set { _maxZoomFactor = value; OnPropertyChanged(nameof(MaxZoomFactor)); }
        }

        public float StartTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged(nameof(StartTime)); }
        }

        public float EndTime
        {
            get { return _endTime; }
            set { _endTime = value; OnPropertyChanged(nameof(EndTime)); }
        }

        public float HorizontalOffset
        {
            get { return _horiOffset; }
            set { _horiOffset = value; OnPropertyChanged(nameof(HorizontalOffset)); }
        }

        public float VerticalOffset
        {
            get { return _vertOffset; }
            set { _vertOffset = value; OnPropertyChanged(nameof(VerticalOffset)); }
        }

        public float CurrentTime
        {
            get { return _currentTime; }
            set { _currentTime = value; OnPropertyChanged(nameof(CurrentTime)); }
        }

        public float InitialStartTime { get; private set; }
        public float InitialEndTime { get; private set; }

        public TimelineViewModel(float startTime, float endTime)
        {
            InitialStartTime = startTime;
            InitialEndTime = endTime;
            _startTime = startTime;
            _endTime = endTime;
        }
    }

}
