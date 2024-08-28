using System;
using System.Collections.Generic;
using System.Linq;

using SysMath = System.Math;

namespace GhostCore.Math
{
    public static class MathUtils
    {
        public const double Epsilon = 9.0E-7;

        public static double Lerp(this double t, double start, double end)
        {
            return start + t * (end - start);
        }

        public static float Lerp(this float t, float start, float end)
        {
            return start + t * (end - start);
        }
        /// <summary>
        /// Calculates a percentage of a number within a range
        /// </summary>
        /// <param name="value">Value to calculate</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>A decimal between 0 and 1 representing the percentage of the value in the range</returns>
        public static double InverseLerp(this double value, double min, double max)
        {
            return (value - min) / (max - min);
        }
        public static double Average(params double[] value)
        {
            return value.Average();
        }
        public static double AsRadian(this double angle)
        {
            return angle * SysMath.PI / 180;
        }
        public static double AsAngle(this double rad)
        {
            return rad * 180 / SysMath.PI;
        }

        public static float AsRadian(this float angle)
        {
            return angle * (float)SysMath.PI / 180;
        }
        public static float AsAngle(this float rad)
        {
            return rad * 180 / (float)SysMath.PI;
        }
        public static double Clamp(this double value, double min, double max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }
        public static void Clamp(ref double value, double min, double max)
        {
            value = Clamp(value, min, max);
        }

        public static bool IsInInterval(this double value, double min, double max, bool inclusiveMin = true, bool inclusiveMax = false)
        {
            if (inclusiveMin && inclusiveMax)
                return value >= min && value <= max;

            if (!inclusiveMin && !inclusiveMax)
                return value > min && value < max;

            if (inclusiveMin && !inclusiveMax)
                return value >= min && value < max;

            if (!inclusiveMin && inclusiveMax)
                return value > min && value <= max;

            return false;
        }
        public static bool IsInInterval(this int value, double min, double max, bool inclusiveMin = true, bool inclusiveMax = false)
        {
            if (inclusiveMin && inclusiveMax)
                return value >= min && value <= max;

            if (!inclusiveMin && !inclusiveMax)
                return value > min && value < max;

            if (inclusiveMin && !inclusiveMax)
                return value >= min && value < max;

            if (!inclusiveMin && inclusiveMax)
                return value > min && value <= max;

            return false;
        }
        public static bool IsInInterval(this float value, double min, double max, bool inclusiveMin = true, bool inclusiveMax = false)
        {
            if (inclusiveMin && inclusiveMax)
                return value >= min && value <= max;

            if (!inclusiveMin && !inclusiveMax)
                return value > min && value < max;

            if (inclusiveMin && !inclusiveMax)
                return value >= min && value < max;

            if (!inclusiveMin && inclusiveMax)
                return value > min && value <= max;

            return false;
        }
        public static bool IsInInterval(this decimal value, decimal min, decimal max, bool inclusiveMin = true, bool inclusiveMax = false)
        {
            if (inclusiveMin && inclusiveMax)
                return value >= min && value <= max;

            if (!inclusiveMin && !inclusiveMax)
                return value > min && value < max;

            if (inclusiveMin && !inclusiveMax)
                return value >= min && value < max;

            if (!inclusiveMin && inclusiveMax)
                return value > min && value <= max;

            return false;
        }

        public static bool EpsilonEquals(this double a, double b)
        {
            return SysMath.Abs(a - b) <= Epsilon;
        }
        public static bool EpsilonTest(this double a, double b, BinaryOp op)
        {
            switch (op)
            {
                case BinaryOp.LT:
                    return (a - b) < Epsilon;
                case BinaryOp.GT:
                    return (a - b) > Epsilon;
                case BinaryOp.LTE:
                    return (a - b) <= Epsilon;
                case BinaryOp.GTE:
                    return (a - b) <= Epsilon;
                default:
                    return false;
            }
        }

        public static double GetAngle(Vector2D p1, Vector2D p2, bool asRadian = true)
        {
            double xDiff = p2.X - p1.X;
            double yDiff = p2.Y - p1.Y;

            if (asRadian)
                return SysMath.Atan2(yDiff, xDiff);
            else
                return AsAngle(SysMath.Atan2(yDiff, xDiff));
        }
        public static double GetAngle(Vector2D vector, bool asRadian = true)
        {
            var result = SysMath.Atan2(vector.Y, vector.X);
            if (!asRadian)
                return AsAngle(result);

            return result;
        }
        public static double GetAngle(double x1, double x2, double y1, double y2, bool asRadian = true)
        {
            double xDiff = x2 - x1;
            double yDiff = y2 - y1;
            if (asRadian)
                return SysMath.Atan2(yDiff, xDiff);
            else
                return AsAngle(SysMath.Atan2(yDiff, xDiff));
        }

        public static double AreaOfTriangle(Vector2D A, Vector2D B, Vector2D C)
        {
            return SysMath.Abs((A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y)) / 2);
        }
        public static double AreaOfQuad(Vector2D A, Vector2D B, Vector2D C, Vector2D D)
        {
            var a1 = AreaOfTriangle(A, B, C);
            var a2 = AreaOfTriangle(B, C, D);

            return a1 + a2;
        }

        public static double Distance(Vector2D x, Vector2D y)
        {
            if (x == null || y == null)
                return double.MaxValue;

            return SysMath.Sqrt(SysMath.Pow(y.X - x.X, 2) + SysMath.Pow(y.Y - x.Y, 2));
        }
        public static double SquaredDistance(Vector2D x, Vector2D y)
        {
            if (x == null || y == null)
                return double.MaxValue;

            return SysMath.Pow(y.X - x.X, 2) + SysMath.Pow(y.Y - x.Y, 2);
        }

        public static Vector2D GetProjectedPointOnLine(Vector2D v1, Vector2D v2, Vector2D p, bool disableBBcheck = false)
        {
            Func<Vector2D, Vector2D, double> dotProduct = (a, b) =>
            {
                return a.X * b.X + a.Y * b.Y;
            };

            var e1 = new Vector2D(v2.X - v1.X, v2.Y - v1.Y);
            var e2 = new Vector2D(p.X - v1.X, p.Y - v1.Y);
            double valDp = dotProduct(e1, e2);
            double lenLineE1 = SysMath.Sqrt(e1.X * e1.X + e1.Y * e1.Y);
            double lenLineE2 = SysMath.Sqrt(e2.X * e2.X + e2.Y * e2.Y);
            double cos = valDp / (lenLineE1 * lenLineE2);
            double projLenOfLine = cos * lenLineE2;
            var qp = new Vector2D((v1.X + (projLenOfLine * e1.X) / lenLineE1), (v1.Y + (projLenOfLine * e1.Y) / lenLineE1));

            if (!disableBBcheck)
            {
                if (!IsPointInsideBoundingBox(v1, v2, qp))
                    return null;
            }

            return qp;
        }
        public static Vector2D GetProjectedPointOnBezier(Vector2D v1, Vector2D control, Vector2D v2, Vector2D p)
        {
            throw new NotImplementedException();
        }
        public static Vector2D NormalizeVector(Vector2D vector)
        {
            var distance = SysMath.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            return new Vector2D(vector.X / distance, vector.Y / distance);
        }
        public static Vector2D RotateVector(double angle, Vector2D pt)
        {
            var a = AsRadian(angle);
            float cosa = (float)SysMath.Cos(a);
            float sina = (float)SysMath.Sin(a);
            Vector2D newPoint = new Vector2D((pt.X * cosa - pt.Y * sina), (pt.X * sina + pt.Y * cosa));
            return newPoint;
        }
        public static Vector2D RotateVector(double angle, Vector2D pt, Vector2D centerPoint)
        {
            var qpt = pt - centerPoint;
            return RotateVector(angle, qpt) + centerPoint;
        }
        public static Vector2D ScaleVector(double scale, Vector2D center)
        {
            return center - scale * center;
        }
        public static Vector2D GetPolylineCentroid(IList<Vector2D> vertices)
        {
            Vector2D centroid = new Vector2D() { X = 0.0, Y = 0.0 };
            double signedArea = 0.0;
            double x0 = 0.0;
            double y0 = 0.0;
            double x1 = 0.0;
            double y1 = 0.0;
            double a = 0.0;

            int i = 0;
            for (i = 0; i < vertices.Count - 1; ++i)
            {
                x0 = vertices[i].X;
                y0 = vertices[i].Y;
                x1 = vertices[i + 1].X;
                y1 = vertices[i + 1].Y;
                a = x0 * y1 - x1 * y0;
                signedArea += a;
                centroid.X += (x0 + x1) * a;
                centroid.Y += (y0 + y1) * a;
            }

            // Do last vertex
            x0 = vertices[i].X;
            y0 = vertices[i].Y;
            x1 = vertices[0].X;
            y1 = vertices[0].Y;
            a = x0 * y1 - x1 * y0;
            signedArea += a;
            centroid.X += (x0 + x1) * a;
            centroid.Y += (y0 + y1) * a;

            signedArea *= 0.5;
            centroid.X /= (6 * signedArea);
            centroid.Y /= (6 * signedArea);

            return centroid;
        }

        public static bool IsPointInsideBoundingBox(Vector2D rectStart, Vector2D rectEnd, Vector2D point)
        {
            if (rectStart == null || rectEnd == null || point == null)
                return false;

            var xcheck = point.X >= rectStart.X && point.X <= rectEnd.X; // is inside X range
            var xcheck2 = point.X >= rectEnd.X && point.X <= rectStart.X; // is inside X range inverted

            var ycheck = point.Y >= rectStart.Y && point.Y <= rectEnd.Y; // is inside Y range
            var ycheck2 = point.Y >= rectEnd.Y && point.Y <= rectStart.Y; // is inside Y range inverted

            return (xcheck || xcheck2) && (ycheck || ycheck2);
        }
        public static bool IsLeftSideOfLine(Vector2D test, Vector2D start, Vector2D end)
        {
            return ((start.X - end.X) * (test.Y - end.Y) - (start.Y - end.Y) * (test.X - end.X)) > 0;

        }
        public static bool IsInPolygon(Vector2D[] polygon, Vector2D testPoint)
        {
            bool result = false;
            int j = polygon.Length - 1;
            for (int i = 0; i < polygon.Length; i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
        public static bool ArePolygonsIntersecting(Vector2D[] a, Vector2D[] b)
        {
            foreach (var polygon in new[] { a, b })
            {
                for (int i1 = 0; i1 < polygon.Length; i1++)
                {
                    int i2 = (i1 + 1) % polygon.Length;
                    var p1 = polygon[i1];
                    var p2 = polygon[i2];

                    var normal = new Vector2D(p2.Y - p1.Y, p1.X - p2.X);

                    double? minA = null, maxA = null;
                    foreach (var p in a)
                    {
                        var projected = normal.X * p.X + normal.Y * p.Y;
                        if (minA == null || projected < minA)
                            minA = projected;
                        if (maxA == null || projected > maxA)
                            maxA = projected;
                    }

                    double? minB = null, maxB = null;
                    foreach (var p in b)
                    {
                        var projected = normal.X * p.X + normal.Y * p.Y;
                        if (minB == null || projected < minB)
                            minB = projected;
                        if (maxB == null || projected > maxB)
                            maxB = projected;
                    }

                    if (maxA < minB || maxB < minA)
                        return false;
                }
            }
            return true;
        }


        public static double QuadraticBezierLength(Vector2D p0, Vector2D p1, Vector2D p2)
        {
            Vector2D a = new Vector2D();
            Vector2D b = new Vector2D();

            a.X = p0.X - 2 * p1.X + p2.X;
            a.Y = p0.Y - 2 * p1.Y + p2.Y;
            b.X = 2 * p1.X - 2 * p0.X;
            b.Y = 2 * p1.Y - 2 * p0.Y;
            var A = 4 * (a.X * a.X + a.Y * a.Y);
            var B = 4 * (a.X * b.X + a.Y * b.Y);
            var C = b.X * b.X + b.Y * b.Y;

            var Sabc = 2 * SysMath.Sqrt(A + B + C);
            var A_2 = SysMath.Sqrt(A);
            var A_32 = 2 * A * A_2;
            var C_2 = 2 * SysMath.Sqrt(C);
            var BA = B / A_2;

            return (A_32 * Sabc + A_2 * B * (Sabc - C_2) + (4 * C * A - B * B) * SysMath.Log((2 * A_2 + BA + Sabc) / (BA + C_2))) / (4 * A_32);
        }

    }

    public enum BinaryOp
    {
        LT,
        GT,
        LTE,
        GTE
    }

}
