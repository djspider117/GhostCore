using System.Numerics;

namespace GhostCore.Math
{
    public interface IField<out T>
    {
        /// <summary>
        /// Returns field value at the point given by the provided coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate</param>
        /// <returns>The value of the field.</returns>
        T GetValue(float x = 0, float y = 0, float z = 0);

        /// <summary>
        /// Returns field value at the point given by the provided coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate</param>
        /// <returns>The value of the field.</returns>
        T GetValue(double x = 0, double y = 0, double z = 0);

        /// <summary>
        /// Returns field value at the point given by the provided coordinates (int values will be cast to float).
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate</param>
        /// <returns>The value of the field.</returns>
        T GetValue(int x = 0, int y = 0, int z = 0);

        /// <summary>
        /// Returns field value at the provided point.
        /// </summary>
        /// <param name="v">The coordinate.</param>
        /// <returns>The value of the field.</returns>
        T GetValue(Vector3 v);
    }
}