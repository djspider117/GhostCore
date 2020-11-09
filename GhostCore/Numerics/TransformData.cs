using GhostCore.ComponentModel;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Xml.Serialization;

namespace GhostCore.Numerics
{
    public class TransformData
    {
        [XmlAttribute]
        public double TranslateX { get; set; }

        [XmlAttribute]
        public double TranslateY { get; set; }

        [XmlAttribute]
        public double Rotation { get; set; }

        [XmlAttribute]
        public double ScaleX { get; set; } = 1;

        [XmlAttribute]
        public double ScaleY { get; set; } = 1;
    }

    public class ObservableTransformData : NotifyPropertyChangedImpl, ICloneable
    {
        public event EventHandler<(double, double)> ScaleChanged;

        private double _translateX;
        private double _translateY;
        private double _rotation;
        private double _scaleX = 1;
        private double _scaleY = 1;
        private (double, double) _combinedTranslation;

        public double TranslateX
        {
            get { return _translateX; }
            set
            {
                _translateX = value;
                OnPropertyChanged(nameof(TranslateX));
            }
        }
        public double TranslateY
        {
            get { return _translateY; }
            set
            {
                _translateY = value;
                OnPropertyChanged(nameof(TranslateY));
            }
        }

        public (double, double) CombinedTranslation
        {
            get { return _combinedTranslation; }
            set
            {
                _combinedTranslation = value;
                OnPropertyChanged(nameof(CombinedTranslation));
            }
        }

        public double Rotation
        {
            get { return _rotation; }
            set { _rotation = value; OnPropertyChanged(nameof(Rotation)); }
        }

        public double ScaleX
        {
            get { return _scaleX; }
            set
            {
                _scaleX = value;
                OnPropertyChanged(nameof(ScaleX));
                OnPropertyChanged(nameof(InverseScaleX));
                ScaleChanged?.Invoke(this, (_scaleX, _scaleY));
            }
        }

        public double ScaleY
        {
            get { return _scaleY; }
            set
            {
                _scaleY = value;
                OnPropertyChanged(nameof(ScaleY));
                OnPropertyChanged(nameof(InverseScaleY));
                ScaleChanged?.Invoke(this, (_scaleX, _scaleY));
            }
        }

        public double UniformScale
        {
            get { return _scaleX; }
            set
            {
                _scaleX = value;
                _scaleY = value;
                OnPropertyChanged(nameof(ScaleX));
                OnPropertyChanged(nameof(InverseScaleX));
                OnPropertyChanged(nameof(ScaleY));
                OnPropertyChanged(nameof(InverseScaleY));

                ScaleChanged?.Invoke(this, (_scaleX, _scaleY));
            }
        }

        public double InverseScaleX
        {
            get { return 1 / _scaleX; }
        }

        public double InverseScaleY
        {
            get { return 1 / _scaleY; }
        }

        public object Clone() => new ObservableTransformData()
        {
            Rotation = Rotation,
            ScaleX = ScaleX,
            ScaleY = ScaleY,
            TranslateX = TranslateX,
            TranslateY = TranslateY,
            UniformScale = UniformScale,
        };

        public void Reset()
        {
            Rotation = 0;
            TranslateX = 0;
            TranslateY = 0;
            UniformScale = 1;

        }

        public void RestoreFrom(ObservableTransformData ct)
        {
            if (ct == null)
                return;

            if (ct.ScaleX != ct.ScaleY)
            {
                ScaleX = ct.ScaleX;
                ScaleY = ct.ScaleY;
            }
            else
            {
                UniformScale = ct.UniformScale;
            }

            Rotation = ct.Rotation;
            TranslateX = ct.TranslateX;
            TranslateY = ct.TranslateY;
        }

        public override string ToString()
        {
            if (_scaleX == _scaleY)
                return $"[Transform (x={TranslateX}, y={TranslateY}, s={UniformScale}, R={Rotation}]";

            return $"[Transform (x={TranslateX}, y={TranslateY}, Sx={ScaleX}, Sy={ScaleY}, R={Rotation}]";
        }
    }
}
