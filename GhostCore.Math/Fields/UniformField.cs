namespace GhostCore.Math
{
    /// <inheritdoc />
    /// <remarks>
    /// This field type is uniform in all direction and will have the same value regardless of the input position specified.
    /// Use this to create constant fields.
    /// </remarks>
    public class UniformField<T> : Field<T> where T : struct
    {
        public T FieldValue { get; protected set; }

        public UniformField(T uniformFieldValue)
        {
            FieldValue = uniformFieldValue;
        }

        /// <inheritdoc />
        /// <remarks>
        /// The implementation of <see cref="IField{T}.GetValue(float, float, float)"/> here will ignore any position and just return the internal field value.
        /// The <see cref="FieldValue"/> property will be returned
        /// </remarks>
        public override T GetValue(float x, float y, float z)
        {
            return FieldValue;
        }
    }
}