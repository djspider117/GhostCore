namespace GhostCore.Math.Interpolation
{
    ///<summary>
    /// Quintic interpolation is the most smooth, guarateeing continuinty of second-order derivatives. it is slow, however.
    ///</summary>
    public class QuinticSCurve : SCurve
    {
        /// <inheritdoc/>
        public override float GetValue(float t)
        {
            var t3 = t * t * t;
            var t4 = t3 * t;
            var t5 = t4 * t;
            return 6 * t5 - 15 * t4 + 10 * t3;
        }
    }
}
