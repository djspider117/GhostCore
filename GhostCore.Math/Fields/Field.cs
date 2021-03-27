using System.Numerics;

namespace GhostCore.Math
{
    /// <summary>
    /// The base class for any three-dimensional multi-value field. Can be used to create tensors.
    /// </summary>
    /// <remarks>
    /// <para>
    /// By default this class works with <see cref="float"/> coordinates.
    /// </para>
    /// <see cref="T"/> will represent the value of the field at a particular coordinate. 
    /// It can be a scalar value (byte, short, half, int, float, double, decimal) or any vector 
    /// </remarks>
    /// <typeparam name="T">The scalar of vectorial value of the 3D field. Must be a struct.</typeparam>
    public abstract class Field<T> : IField<T> where T : struct
    {
        #region IField<T>

        /// <inheritdoc />
        public abstract T GetValue(float x, float y, float z);
        /// <inheritdoc />
        /// <remarks>
        /// This is a utility method, it calls <see cref="GetValue(float, float, float)"/>
        /// </remarks>
        public virtual T GetValue(double x = 0, double y = 0, double z = 0) => GetValue((float)x, (float)y, (float)z);
        /// <inheritdoc />
        /// /// <remarks>
        /// This is a utility method, it calls <see cref="GetValue(float, float, float)"/>
        /// </remarks>
        public virtual T GetValue(int x = 0, int y = 0, int z = 0) => GetValue(x, y, z);

        /// <inheritdoc />
        /// <remarks>
        /// This is a utility method, it calls <see cref="GetValue(float, float, float)"/>
        /// </remarks>
        public virtual T GetValue(Vector3 v)
        {
            return GetValue(v.X, v.Y, v.Z);
        }

        #endregion

        #region Operators

        ///<summary>
        /// Overloaded + 
        /// Returns new field that sums these two
        ///</summary>
        public static Field<T> operator +(Field<T> field1, Field<T> field2)
        {
            return new FunctionField<T>((x, y, z) => (dynamic)field1.GetValue(x, y, z) + (dynamic)field2.GetValue(x, y, z));
        }
        ///<summary>
        /// Overloaded + 
        /// Returns new field that adds a constant value
        ///</summary>
        public static Field<T> operator +(Field<T> field1, float f)
        {
            return new FunctionField<T>((x, y, z) => (dynamic)field1.GetValue(x, y, z) + f);
        }


        ///<summary>
        /// Overloaded unary - 
        /// Returns inverse of argument generator
        ///</summary>
        public static Field<T> operator -(Field<T> field1)
        {
            return -1 * (dynamic)field1;
        }
        ///<summary>
        /// Overloaded - 
        /// Returns new field that subtracts second argument from first
        ///</summary>
        public static Field<T> operator -(Field<T> field1, Field<T> field2)
        {
            return new FunctionField<T>((x, y, z) => (dynamic)field1.GetValue(x, y, z) - (dynamic)field2.GetValue(x, y, z)); ;
        }
        ///<summary>
        /// Overloaded - 
        /// Returns new field that subtracts a constant value
        ///</summary>
        public static Field<T> operator -(Field<T> field1, float f)
        {
            return new FunctionField<T>((x, y, z) => (dynamic)field1.GetValue(x, y, z) - f);
        }
        ///<summary>
        /// Overloaded - 
        /// Returns new field that subtracts generator from a constant value
        ///</summary>
        public static Field<T> operator -(float f, Field<T> field1)
        {
            return new FunctionField<T>((x, y, z) => f - (dynamic)field1.GetValue(x, y, z));
        }


        ///<summary>
        /// Overloaded *
        /// Returns new field that multiplies these two
        ///</summary>
        public static Field<T> operator *(Field<T> field1, Field<T> field2)
        {
            return new FunctionField<T>((x, y, z) => (dynamic)field1.GetValue(x, y, z) * (dynamic)field2.GetValue(x, y, z)); ;
        }
        ///<summary>
        /// Overloaded *
        /// Returns new field that multiplies noise by a constant value
        ///</summary>
        public static Field<T> operator *(Field<T> field1, float f)
        {
            return new FunctionField<T>((x, y, z) => (dynamic)field1.GetValue(x, y, z) * f);
        }


        ///<summary>
        /// Overloaded /
        /// Returns new field that divides values of argument generators. Beware of zeroes!
        ///</summary>
        public static Field<T> operator /(Field<T> field1, Field<T> field2)
        {
            return new FunctionField<T>((x, y, z) => (dynamic)field1.GetValue(x, y, z) / (dynamic)field2.GetValue(x, y, z));
        }
        ///<summary>
        /// Overloaded /
        /// Returns new field that divides noise by a constant value
        ///</summary>
        public static Field<T> operator /(Field<T> field1, float f)
        {
            return new FunctionField<T>((x, y, z) => (dynamic)field1.GetValue(x, y, z) / f);
        }
        ///<summary>
        /// Overloaded /
        /// Returns new field that divides constant value by noise values
        ///</summary>
        public static Field<T> operator /(float f, Field<T> field1)
        {
            return new FunctionField<T>((x, y, z) => f / (dynamic)field1.GetValue(x, y, z));
        }


        ///<summary>
        /// Conversion operator. Float values may be implicitly converted to a generator that return this value
        ///</summary>
        ///<param name="value"></param>
        public static implicit operator Field<T>(T value)
        {
            return new UniformField<T>(value);
        }

        #endregion
    }

    public abstract class Field : Field<float> { }
}