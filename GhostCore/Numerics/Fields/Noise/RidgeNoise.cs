using SysMath = System.Math;

namespace GhostCore.Numerics
{
    /// <summary>
    /// This generator adds samples with weight decreasing with frequency, like Perlin noise; however, each signal is taken as absolute value, and weighted by previous (i.e. lower-frequency) signal,
    /// creating a sort of feedback loop. Resulting noise has sharp ridges, somewhat resembling cliffs. This is useful for terrain generation.
    /// </summary>
    public class RidgeNoiseField : FractalNoiseField
    {
        #region Fields

        private float _exponent = 1;
        private float[] _spectralWeights;
        private float _weight;

        #endregion

        #region Properties

        /// <summary>
        /// Exponent defines how fast weights decrease with frequency. The higher the exponent, the less weight is given to high frequencies. 
        /// Default value is 1
        /// </summary>
        public float Exponent
        {
            get { return _exponent; }
            set { _exponent = value; OnParamsChanged(); }
        }

        /// <summary>
        /// Offset is applied to signal at every step. Default value is 1
        /// </summary>
        public float Offset { get; set; } = 1;

        /// <summary>
        /// Gain is the weight factor for previous-step signal. Higher gain means more feedback and noisier ridges. 
        /// Default value is 2.
        /// </summary>
        public float Gain { get; set; } = 2;

        #endregion

        #region Constructors

        ///<summary>
        /// Create new ridge generator using seed (seed is used to create a <see cref="GradientNoiseField"/> source)
        ///</summary>
        ///<param name="seed">seed value</param>
        public RidgeNoiseField(int seed)
            : base(seed)
        {
        }

        ///<summary>
        /// Create new ridge generator with user-supplied source.
        ///</summary>
        ///<param name="source">noise source</param>
        public RidgeNoiseField(Field source)
            : base(source)
        {
        }

        #endregion

        /// <inheritdoc/>
        protected override float CombineOctave(int curOctave, float signal, float value)
        {
            if (curOctave == 0)
                _weight = 1;

            // Make the ridges.
            signal = Offset - SysMath.Abs(signal);

            // Square the signal to increase the sharpness of the ridges.
            signal *= signal;

            // The weighting from the previous octave is applied to the signal.
            // Larger values have higher weights, producing sharp points along the
            // ridges.
            signal *= _weight;

            // Weight successive contributions by the previous signal.
            _weight = signal * Gain;
            if (_weight > 1)
                _weight = 1;
            
            if (_weight < 0)
                _weight = 0;

            // Add the signal to the output value.
            return value + (signal * _spectralWeights[curOctave]);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// In this implementation, we pre-calculate the spectral weights used to multiply the ridge signal
        /// </remarks>
        protected override void OnParamsChanged()
        {
            float frequency = 1;
            _spectralWeights = new float[_octaves];
            for (int i = 0; i < _octaves; i++)
            {
                // Compute weight for each frequency.
                _spectralWeights[i] = (float)SysMath.Pow(frequency, -Exponent);
                frequency *= Lacunarity;
            }
        }
    }

}