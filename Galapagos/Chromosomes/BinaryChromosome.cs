using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.Chromosomes
{
    /// <summary>
    /// A binary genetic property of a creature.
    /// </summary>
    public class BinaryChromosome : Chromosome, IBinaryChromosome
    {
        private readonly bool[] _bits;
        private readonly uint _bitCount;

        /// <summary>
        /// Constructs a new instance of the <see cref="BinaryChromosome"/> class.
        /// </summary>
        /// <param name="bitCount">The bit count.</param>
        internal BinaryChromosome(uint bitCount = 32)
        {
            _bitCount = bitCount;
            _bits = new bool[bitCount];
            for (var i = 0; i < bitCount; i++)
                _bits[i] = Stochastic.FlipCoin();
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BinaryChromosome"/> class.
        /// </summary>
        /// <param name="bits">The bits.</param>
        /// <param name="bitCount">The bit count.</param>
        internal BinaryChromosome(bool[] bits)
        {
            _bitCount = (uint)bits.Length;
            _bits = bits;
        }

        /// <summary>
        /// Measures the distance between two chromosome.
        /// </summary>
        /// <remarks>Hamming distance.</remarks>
        /// <param name="other">The other chromosome.</param>
        /// <returns>The distance between the chromosomes.</returns>
        public override double Distance(IChromosome other)
        {
            if(!(other is BinaryChromosome) || (((BinaryChromosome)other).BitCount != BitCount))
                throw new ArgumentException("Error! Incompatible chromosomes.");

            var otherBits = ((BinaryChromosome)other).Bits;

            uint distance = 0;
            for(var i = 0; i < BitCount; i++)
            {
                if (Bits[i] != otherBits[i])
                    distance++;
            }

            return distance;
        }

        /// <summary>
        /// Gets the bits.
        /// </summary>
        public bool[] Bits => _bits;

        /// <summary>
        /// Gets the bitCount.
        /// </summary>
        public uint BitCount => _bitCount;

        /// <summary>
        /// Accesses a gene from the chromosome.
        /// </summary>
        /// <param name="index">The gene index.</param>
        /// <returns>The gene.</returns>
        public bool this[int index]
        {
            get
            {
                if (index >= _bits.Count())
                    throw new Exception($"Error! {index} is larger than the chromosome size.");
                return _bits[index];
            }
        }

        /// <summary>
        /// Converts the binary chromosome to an uint.
        /// </summary>
        /// <param name="lower">The lower bound.</param>
        /// <param name="upper">The upper bound.</param>
        /// <returns>The uint.</returns>
        public uint ToUInt(uint lower = uint.MinValue, uint upper = uint.MaxValue)
        {
            var num = Convert.ToUInt32(GetBinaryData());
            if (num <= lower) return lower;
            else if (num >= upper) return upper;
            else return num;
        }

        /// <summary>
        /// Converts the binary chromosome to an int.
        /// </summary>
        /// <param name="lower">The lower bound.</param>
        /// <param name="upper">The upper bound.</param>
        /// <returns>The int.</returns>
        public int ToInt(int lower = int.MinValue, int upper = int.MaxValue)
        {
            var num = Convert.ToInt32(GetBinaryData());
            if (num <= lower) return lower;
            else if (num >= upper) return upper;
            else return num;
        }

        /// <summary>
        /// Converts the binary chromosome to an double.
        /// </summary>
        /// <param name="lower">The lower bound.</param>
        /// <param name="upper">The upper bound.</param>
        /// <returns>The double.</returns>
        public double ToDouble(double lower = double.MinValue, double upper = double.MaxValue)
        {
            var num = Convert.ToDouble(GetBinaryData());
            if (num <= lower) return lower;
            else if (num >= upper) return upper;
            else return num;
        }

        /// <summary>
        /// Gets the raw binary data from the bits collection.
        /// </summary>
        /// <returns>The raw binary data.</returns>
        private uint GetBinaryData()
        {
            uint bin = 0;

            for (var i = 0; i < 32; i++)
            {
                if (i >= _bitCount)
                    break;

                bin <<= 1;

                if (_bits[i])
                    bin ^= 0x01;
            }

            return bin;
        }

        #region IEnumerable Members

        public IEnumerator<bool> GetEnumerator()
        {
            return _bits.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
