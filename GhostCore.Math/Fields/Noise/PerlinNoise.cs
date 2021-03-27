namespace GhostCore.Math
{
    /// <summary>
    /// Pink noise is a fractal noise that adds together weighted signals sampled at different frequencies, with weight inversely proportional to frequency.
    /// When source noise is <see cref="GradientNoiseField"/>, this becomes Perlin noise.
    /// </summary>
    /// </summary>
    public class PerlinNoiseField : PinkNoiseField
    {
        public PerlinNoiseField(int seed) : base(seed)
        {
        }

        public PerlinNoiseField(Field source) : base(source)
        {
        }
    }
}