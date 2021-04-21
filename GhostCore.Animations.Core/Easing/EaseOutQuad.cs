namespace GhostCore.Animations.Core.Easing
{
    public class EaseOutQuad : EasingFunction
    {
        protected override float EvaluateInternal(float t, Keyframe k0, Keyframe k1, float dt)
        {
            var c = EndValue - StartValue;
            return -c * (t /= Duration) * (t - 2) + StartValue;
        }
    }
}
