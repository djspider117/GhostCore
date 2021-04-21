namespace GhostCore.Animations.Core
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

    public struct TextKeyframe
    {
        public float Time;
        public string Value;
    }
}
