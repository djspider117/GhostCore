namespace GhostCore.Animations
{
    public abstract class EasingFunction : AnimationCurve
    {
        public float Duration { get; set; }
        public float StartValue { get; set; }
        public float EndValue { get; set; }

    }
}
