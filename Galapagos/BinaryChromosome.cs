using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos
{
    /// <summary>
    /// A binary genetic property of a creature.
    /// </summary>
    public class BinaryChromosome : IChromosome
    {
        private readonly uint _bits;
        private readonly uint _bitCount;
        private readonly uint _mask;

        /// <summary>
        /// Constructs a new instance of the <see cref="BinaryChromosome"/> class.
        /// </summary>
        /// <param name="bitCount">The bit count.</param>
        internal BinaryChromosome(uint bitCount = 32)
        {
            var bits = (uint)Stochastic.Next();
            _bitCount = bitCount;
            _mask = ComputeMask(_bitCount);
            _bits = bits % (_mask >> 1);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BinaryChromosome"/> class.
        /// </summary>
        /// <param name="bits">The bits.</param>
        /// <param name="bitCount">The bit count.</param>
        internal BinaryChromosome(uint bits, uint bitCount)
        {
            _bitCount = bitCount;
            _mask = ComputeMask(_bitCount);
            _bits = bits & _mask;
        }

        /// <summary>
        /// Computes the mask.
        /// </summary>
        /// <param name="bitCount">The bit count.</param>
        /// <returns>The mask.</returns>
        private uint ComputeMask(uint bitCount)
        {
            uint mask = 0;
            for (var i = 0; i < bitCount; i++)
            {
                mask <<= 1;
                mask++;
            }
            return mask;
        }

        /// <summary>
        /// Gets the bits.
        /// </summary>
        public uint Bits => _bits;

        /// <summary>
        /// Gets the bitCount.
        /// </summary>
        public uint BitCount => _bitCount;

        /// <summary>
        /// Gets the mask.
        /// </summary>
        internal uint Mask => _mask;

        /// <summary>
        /// Converts the binary chromosome to an uint.
        /// </summary>
        /// <param name="lower">The lower bound.</param>
        /// <param name="upper">The upper bound.</param>
        /// <returns>The uint.</returns>
        public uint ToUInt(uint lower = uint.MinValue, uint upper = uint.MaxValue)
        {
            var num = Convert.ToUInt32(_bits);
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
            var num = Convert.ToInt32(_bits);
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
            var num = Convert.ToDouble(_bits);
            if (num <= lower) return lower;
            else if (num >= upper) return upper;
            else return num;
        }
    }
}
