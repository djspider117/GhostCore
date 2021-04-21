namespace GhostCore.Animations.Core.Easing
{
    public class EaseOutQuart : EasingFunction
    {
        protected override float EvaluateInternal(float t, Keyframe k0, Keyframe k1, float dt)
        {
            var c = EndValue - StartValue;
            return -c * ((t = t / Duration - 1) * t * t * t - 1) + StartValue;
        }
    }
}
