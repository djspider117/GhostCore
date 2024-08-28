namespace GhostCore.Math
{
    /// <summary>
    /// A variation of Perlin noise, this generator creates billowy shapes useful for cloud generation. It uses the same formula as Perlin noise, but adds 
    /// absolute values of signal
    /// </summary>
    public class BillowNoiseField : PerlinNoiseField
    {
        ///<summary>
        /// Create new billow generator using seed (seed is used to create a <see cref="GradientNoiseField"/> source)
        ///</summary>
        ///<param name="seed">seed value</param>
        public BillowNoiseField(int seed)
            : base(seed)
        {
        }
        ///<summary>
        /// Create new billow generator with user-supplied source. 
        ///</summary>
        ///<param name="source">noise source</param>
        public BillowNoiseField(Field source)
            : base(source)
        {
        }

        /// <inheritdoc/>
        protected override float CombineOctave(int curOctave, float signal, float value)
        {
            if (curOctave == 0)
                _curPersistence = 1;
            value += (2 * System.Math.Abs(signal) - 1) * _curPersistence;
            _curPersistence *= Persistence;
            return value;
        }
    }
}