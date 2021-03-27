using SysMath = System.Math;

namespace GhostCore.Math.Interpolation
{
    ///<summary>
    /// Cosine interpolation uses cosine function instead of power curve, resulting in somewhat smoother noise than cubic interpolation, but still only achieving first-order continuity.
    /// Depending on target machine, it may be faster than quintic interpolation.
    ///</summary>
    public class CosineSCurve : SCurve
    {
        /// <inheritdoc/>
        public override float GetValue(float t)
        {
            return (float)((1 - SysMath.Cos(t * SysMath.PI)) * .5);
        }
    }
}
