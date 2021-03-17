using System;

namespace GhostCore.Animations
{
    public struct Keyframe
    {
        public float TangentIn;
        public float TangentOut;
        public float WeightIn;
        public float WeightOut;
        public float Time;
        public float Value;
        public WeightedMode WeightedMode;
    }

    internal class KeyframeSerializationHelper
    {
        public float TangentIn { get; set; }
        public float TangentOut { get; set; }
        public float WeightIn { get; set; }
        public float WeightOut { get; set; }
        public float Time { get; set; }
        public float Value { get; set; }
        public WeightedMode WeightedMode { get; set; }

        internal Keyframe ToKeyFrame()
        {
            return new Keyframe
            {
                TangentIn = TangentIn,
                TangentOut = TangentOut,
                Time = Time,
                Value = Value,
                WeightedMode = WeightedMode,
                WeightIn = WeightIn,
                WeightOut = WeightOut
            };
        }
    }

    public struct TextKeyframe
    {
        public float Time;
        public string Value;
    }

    public class TextKeyframeSerializationHelper
    {
        public float Time { get; set; }
        public string Value { get; set; }

        internal TextKeyframe ToKeyFrame()
        {
            return new TextKeyframe()
            {
                Time = Time,
                Value = Value
            };
        }
    }
}
