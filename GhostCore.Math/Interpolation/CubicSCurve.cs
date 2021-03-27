namespace GhostCore.Math.Interpolation
{
    ///<summary>
    /// Cubic interpolation is a good compromise between speed and quality. It's slower than linear, but ensures continuity of 1-st order derivatives, making noise smooth.
    ///</summary>
    public class CubicSCurve : SCurve
    {
        /// <inheritdoc />
        public override float GetValue(float t)
        {
            return t * t * (3f - 2f * t);
        }
    }
}
