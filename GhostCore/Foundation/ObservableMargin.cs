using GhostCore.ComponentModel;
using System;
using System.Collections.Generic;

namespace GhostCore.Foundation
{
    public class ObservableMargin : NotifyPropertyChangedImpl, IEquatable<ObservableMargin>
    {
        private double _left;
        private double _top;
        private double _right;
        private double _bottom;

        public double Bottom
        {
            get { return _bottom; }
            set { _bottom = value; OnPropertyChanged(nameof(Bottom)); }
        }
        public double Right
        {
            get { return _right; }
            set { _right = value; OnPropertyChanged(nameof(Right)); }
        }
        public double Top
        {
            get { return _top; }
            set { _top = value; OnPropertyChanged(nameof(Top)); }
        }
        public double Left
        {
            get { return _left; }
            set { _left = value; OnPropertyChanged(nameof(Left)); }
        }

        public ObservableMargin()
            : this(0)
        {

        }

        public ObservableMargin(double uniformSize)
            : this(uniformSize, uniformSize, uniformSize, uniformSize)
        {

        }

        public ObservableMargin(double left, double top, double right, double bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ObservableMargin);
        }

        public bool Equals(ObservableMargin other)
        {
            return other != null &&
                   _left == other._left &&
                   _top == other._top &&
                   _right == other._right &&
                   _bottom == other._bottom;
        }

        public override int GetHashCode()
        {
            var hashCode = -906719803;
            hashCode = hashCode * -1521134295 + _left.GetHashCode();
            hashCode = hashCode * -1521134295 + _top.GetHashCode();
            hashCode = hashCode * -1521134295 + _right.GetHashCode();
            hashCode = hashCode * -1521134295 + _bottom.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ObservableMargin margin1, ObservableMargin margin2)
        {
            return EqualityComparer<ObservableMargin>.Default.Equals(margin1, margin2);
        }

        public static bool operator !=(ObservableMargin margin1, ObservableMargin margin2)
        {
            return !(margin1 == margin2);
        }
    }
}
