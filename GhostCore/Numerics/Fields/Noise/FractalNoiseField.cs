using System.Numerics;

namespace GhostCore.Numerics
{
    ///<summary>
    /// Base class for fractal noise fields. 
    ///</summary>
    /// <remarks>
    /// Fractal generators use a source noise, that is sampled at several frequencies. 
    /// These sampled values are then combined into a result using some algorithm determined in inherited classes.
    /// 
    /// The default generator field is a <see cref="GradientNoiseField"/>
    ///</remarks>
    public abstract class FractalNoiseField : Field
    {
        #region Fields

        private static readonly Quaternion _rotation = Quaternion.CreateFromYawPitchRoll(47, 29, 55); // random rotation used in octave conjucation
        private readonly Field _noiseField; // source noise field

        /// <summary>
        /// Initial frequency.
        /// </summary>
        protected float _freq;

        /// <summary>
        /// Frequency coefficient. "How spread out the noise is"
        /// </summary>
        protected float _lacunarity;

        /// <summary>
        /// Number of octaves to sample. Default is 6
        /// </summary>
        protected int _octaves;

        #endregion

        #region Properties

        ///<summary>
        /// Frequency coefficient. Sampling frequency is multiplied by lacunarity value with each octave.
        /// Default value is 2, so that every octave doubles sampling frequency
        ///</summary>
        public float Lacunarity
        {
            get { return _lacunarity; }
            set { _lacunarity = value; OnParamsChanged(); }
        }

        /// <summary>
        /// Number of octaves to sample. Default is 6.
        /// </summary>
        public int Octaves
        {
            get { return _octaves; }
            set { _octaves = value; OnParamsChanged(); }
        }

        /// <summary>
        /// Initial frequency.
        /// </summary>
        public float Frequency
        {
            get { return _freq; }
            set { _freq = value; OnParamsChanged(); }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new fractal noise using default source: gradient noise seeded by supplied seed value
        /// </summary>
        /// <param name="seed">seed value</param>
        protected FractalNoiseField(int seed) 
            : this(new GradientNoiseField(seed))
        {
        }

        /// <summary>
        /// Creates a new fractal noise, supplying your own source generator
        /// </summary>
        /// <param name="source">source noise</param>
        protected FractalNoiseField(Field source)
        {
            _noiseField = source;
            Lacunarity = 2.17f;
            Octaves = 6;
            Frequency = 1;
        }

        #endregion

        /// <inheritdoc />
        ///   /// <remarks>
        /// We override the default behaviour for getting a field value to leverage SIMD accelerated types in .NET
        /// </remarks>
        public override float GetValue(Vector3 fieldPos)
        {
            var value = 0f;

            for (int curOctave = 0; curOctave < Octaves; curOctave++)
            {
                // Get the coherent-noise value from the input value and add it to the final result.
                var signal = _noiseField.GetValue(fieldPos);
                value = CombineOctave(curOctave, signal, value);

                // Prepare the next octave.
                // scale coords to increase frequency, then rotate to break up lattice pattern
                fieldPos = Vector3.Transform(fieldPos * Lacunarity, _rotation);
            }

            return value;
        }

        /// <inheritdoc />
        /// <remarks>
        /// We override the default behaviour for getting a field value to leverage SIMD accelerated types in .NET
        /// </remarks>
        public override float GetValue(float x, float y, float z) => GetValue(new Vector3(x, y, z));

        /// <summary>
        /// Returns new resulting noise value after source noise is sampled. 
        /// </summary>
        /// <param name="curOctave">Octave at which source is sampled (this always starts with 0</param>
        /// <param name="signal">Sampled value</param>
        /// <param name="value">Resulting value from previous step</param>
        /// <returns>Resulting value adjusted for this sample</returns>
        protected abstract float CombineOctave(int curOctave, float signal, float value);

        /// <summary>
		/// This method is called whenever any generator's parameter is changed (i.e. Lacunarity, Frequency or OctaveCount). 
        /// Override it to precalculate any values used in generation.
		/// </summary>
		protected virtual void OnParamsChanged() { }
    }

}