namespace GhostCore.Animations.Core
{
    public class AnimationCurve
    {
        private float _cachedDuration = -1;

        public Keyframe[] Keyframes { get; set; }
        public PlayableWrapMode WrapMode { get; set; }
        public bool IsAnimationEnabled { get; set; }
        public string TargetProperty { get; set; }
        public bool UseLinerInterpolation { get; set; } = true;

        public float CachedDuration
        {
            get
            {
                if (_cachedDuration == -1)
                    _cachedDuration = Keyframes[Keyframes.Length - 1].Time - Keyframes[0].Time;

                return _cachedDuration;
            }
        }

        public float Evaluate(float t, float keyOffset = 0)
        {
            if (keyOffset > t)
                return Keyframes[0].Value;

            t -= keyOffset;
            if (t < 0)
                t = 0;

            if (!IsAnimationEnabled)
            {
                Keyframe? rv = null;
                for (int i = 0; i < Keyframes.Length; i++)
                {
                    var x = Keyframes[i];
                    if (x.Time <= t)
                    {
                        rv = x;
                    }

                    if (x.Time > t)
                    {
                        break;
                    }
                }

                if (rv == null)
                    return Keyframes[0].Value;
                else
                    return rv.Value.Value;
            }

            Keyframe? key0 = null;
            Keyframe? key1 = null;

            switch (WrapMode)
            {
                case PlayableWrapMode.Loop:
                    t -= CachedDuration * (int)(t / CachedDuration);
                    break;
                case PlayableWrapMode.PingPong:
                    var iteration = (int)(t / CachedDuration);
                    t -= CachedDuration * iteration;

                    if (iteration % 2 != 0)
                        t = CachedDuration - t;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < Keyframes.Length; i++)
            {
                var x = Keyframes[i];
                var ct = x.Time;
                if (ct <= t)
                {
                    key0 = x;
                    continue;
                }

                if (ct > t)
                {
                    key1 = x;
                    break;
                }
            }

            if (key0 == null)
            {
                key0 = new Keyframe() { Time = 0, Value = Keyframes[0].Value };
            }

            var k0 = key0.Value;

            if (key1 == null)
                return k0.Value;

            var k1 = key1.Value;

            float dt = k1.Time - k0.Time;

            t = (t - k0.Time) / dt;

            return EvaluateInternal(t, k0, k1, dt);
        }

        protected virtual float EvaluateInternal(float t, Keyframe k0, Keyframe k1, float dt)
        {
            if (UseLinerInterpolation)
                return (1 - t) * k0.Value + k1.Value * t;

            float m0 = k0.TangentOut * dt;
            float m1 = k1.TangentIn * dt;

            float tsq = t * t;
            float tcube = tsq * t;

            float a = 2 * tcube - 3 * tsq + 1;
            float b = tcube - 2 * tsq + t;
            float c = tcube - tsq;
            float d = -2 * tcube + 3 * tsq;

            return a * k0.Value + b * m0 + c * m1 + d * k1.Value;
        }
    }

}
