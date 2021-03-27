using GhostCore.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace GhostCore.Foundation
{
    public class ObservableSize : NotifyPropertyChangedImpl, IEquatable<ObservableSize>
    {
        private double _width;
        private double _height;

        public double Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged(nameof(Width)); }
        }

        public double Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged(nameof(Height)); }
        }

        public ObservableSize()
            : this(uniformSize: 0)
        {

        }

        public ObservableSize(double uniformSize)
            : this(width: 0, height: 0)
        {
        }

        public ObservableSize(double width, double height)
        {
            _width = width;
            _height = height;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ObservableSize);
        }

        public bool Equals(ObservableSize other)
        {
            return other != null &&
                   _width == other._width &&
                   _height == other._height;
        }

        public override int GetHashCode()
        {
            var hashCode = -607065473;
            hashCode = hashCode * -1521134295 + _width.GetHashCode();
            hashCode = hashCode * -1521134295 + _height.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ObservableSize size1, ObservableSize size2)
        {
            return EqualityComparer<ObservableSize>.Default.Equals(size1, size2);
        }

        public static bool operator !=(ObservableSize size1, ObservableSize size2)
        {
            return !(size1 == size2);
        }
    }
}
