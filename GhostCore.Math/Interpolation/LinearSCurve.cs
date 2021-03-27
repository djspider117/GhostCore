namespace GhostCore.Math.Interpolation
{
    ///<summary>
    /// Linear interpolator is the fastest and has the lowest quality, only ensuring continuity of noise values, not their derivatives.
    ///</summary>
    public class LinearSCurve : SCurve
    {
        /// <inheritdoc/>
        public override float GetValue(float t)
        {
            return t;
        }
    }
}
