using System;
using System.Drawing;

namespace GhostCore.Math
{
    public class Vector2D : ICloneable, IEquatable<Vector2D>
    {
        #region Properties

        public double X { get; set; }
        public double Y { get; set; }

        public bool IsNan
        {
            get
            {
                return double.IsNaN(X) || double.IsNaN(Y);
            }
        }

        #endregion

        #region Constructors

        public Vector2D()
        {
        }

        public Vector2D(double x = 0, double y = 0)
        {
            X = x;
            Y = y;
        }
        public Vector2D(Vector2D pt) : this(pt.X, pt.Y)
        {
        }

        #endregion

        #region APIs

        public void Offset(Vector2D pt)
        {
            Offset(pt.X, pt.Y);
        }
        public void Offset(double x, double y)
        {
            X += x;
            Y += y;
        }

        public void Replace(double x, double y)
        {
            X = x;
            Y = y;
        }
        public void Reset()
        {
            X = Y = 0;
        }

        public Vector2D Clone()
        {
            return new Vector2D(this);
        }
        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region Math

        public double Distance(Vector2D target)
        {
            return MathUtils.Distance(this, target);
        }
        public double SquaredDistance(Vector2D target)
        {
            return MathUtils.SquaredDistance(this, target);
        }

        public Vector2D ProjectOnLine(Vector2D v1, Vector2D v2, bool checkOnlyInsideBoundingBox = false)
        {
            return MathUtils.GetProjectedPointOnLine(v1, v2, this, checkOnlyInsideBoundingBox);
        }

        public Vector2D Normalize()
        {
            return MathUtils.NormalizeVector(this);
        }

        #endregion

        #region Overrides

        public bool Equals(Vector2D other)
        {
            if (other == null)
                return false;

            return MathUtils.EpsilonEquals(other.X, X) && MathUtils.EpsilonEquals(other.Y, Y);
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", X, Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this))
                return true;

            if (obj is Vector2D p)
            {
                return p.Equals(this);
            }

            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Binary Operators

        public static Vector2D operator +(Vector2D c1, Vector2D c2)
        {
            return new Vector2D(c1.X + c2.X, c1.Y + c2.Y);
        }
        public static Vector2D operator +(Vector2D c1, double a)
        {
            return new Vector2D(c1.X + a, c1.Y + a);
        }
        public static Vector2D operator -(Vector2D c1, Vector2D c2)
        {
            return new Vector2D(c1.X - c2.X, c1.Y - c2.Y);
        }
        public static Vector2D operator -(Vector2D c1, double a)
        {
            return new Vector2D(c1.X - a, c1.Y - a);
        }
        public static Vector2D operator *(Vector2D c1, Vector2D c2)
        {
            return new Vector2D(c1.X * c2.X, c1.Y * c2.Y);
        }
        public static Vector2D operator *(Vector2D c1, double scalar)
        {
            return new Vector2D(c1.X * scalar, c1.Y * scalar);
        }
        public static Vector2D operator *(double scalar, Vector2D c1)
        {
            return new Vector2D(c1.X * scalar, c1.Y * scalar);
        }

        public static Vector2D operator /(Vector2D c1, double scalar)
        {
            return new Vector2D(c1.X / scalar, c1.Y / scalar);
        }
        public static Vector2D operator /(Vector2D c1, Vector2D c2)
        {
            return new Vector2D(c1.X / c2.X, c1.Y / c2.Y);
        }

        public static Vector2D operator %(Vector2D c1, int scalar)
        {
            return new Vector2D(c1.X % scalar, c1.Y % scalar);
        }

        #endregion

        #region Logic Operators

        public static bool operator ==(Vector2D c1, Vector2D c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null))
                return true;
            if (ReferenceEquals(c1, null))
                return false;

            return c1.Equals(c2);
        }
        public static bool operator !=(Vector2D c1, Vector2D c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null))
                return false;

            if (ReferenceEquals(c1, null))
                return true;

            return !c1.Equals(c2);
        }
        public static bool operator <(Vector2D c1, Vector2D c2)
        {
            var origin = new Vector2D();
            var d1 = MathUtils.Distance(c1, origin);
            var d2 = MathUtils.Distance(c2, origin);
            return d1 < d2;
        }
        public static bool operator >(Vector2D c1, Vector2D c2)
        {
            var origin = new Vector2D();
            var d1 = MathUtils.Distance(c1, origin);
            var d2 = MathUtils.Distance(c2, origin);
            return d1 > d2;
        }

        #endregion

        #region Implicit Operators

        public static implicit operator Vector2D(double d)
        {
            return new Vector2D(d, d);
        }
        public static implicit operator PointF(Vector2D v)
        {
            return new PointF((float)v.X, (float)v.Y);
        }
        public static implicit operator Point(Vector2D v)
        {
            return new Point((int)v.X, (int)v.Y);
        }
        public static implicit operator Vector2D(PointF v)
        {
            return new Vector2D(v.X, v.Y);
        }
        public static implicit operator Vector2D(Point v)
        {
            return new Vector2D(v.X, v.Y);
        }

        #endregion
    }
}
