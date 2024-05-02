namespace GhostCore.Math
{
    /// <summary>
    /// Pink noise is a fractal noise that adds together weighted signals sampled at different frequencies, with weight inversely proportional to frequency.
    /// When source noise is <see cref="GradientNoiseField"/>, this becomes Perlin noise.
    /// </summary>
    public class PinkNoiseField : FractalNoiseField
    {
        /// <summary>
        /// The current persistance value
        /// </summary>
        protected float _curPersistence;

        /// <summary>
        /// Persistence value determines how fast signal diminishes with frequency. i-th octave sugnal will be multiplied by presistence to the i-th power.
        /// Note that persistence values >1 are possible, but will not produce interesting noise (lower frequencies will just drown out)
        /// 
        /// Default value is 0.5
        /// </summary>
        public float Persistence { get; set; }

        ///<summary>
        /// Create new pink noise generator using seed. Seed is used to create a <see cref="GradientNoise"/> source. 
        ///</summary>
        ///<param name="seed">seed value</param>
        public PinkNoiseField(int seed) : base(seed)
        {
            Persistence = 0.5f;
        }

        ///<summary>
        /// Create new pink noise generator with user-supplied source. Usually one would use this with <see cref="ValueNoise"/> or gradient noise with less dimensions, but 
        /// some weird effects may be achieved with other generators.
        ///</summary>
        ///<param name="source">noise source</param>
        public PinkNoiseField(Field source) : base(source)
        {
            Persistence = 0.5f;
        }

        /// <inheritdoc/>
        protected override float CombineOctave(int curOctave, float signal, float value)
        {
            if (curOctave == 0)
                _curPersistence = 1;
            value += signal * _curPersistence;
            _curPersistence *= Persistence;
            return value;
        }
    }
}