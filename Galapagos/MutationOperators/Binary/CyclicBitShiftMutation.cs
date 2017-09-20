using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Binary
{
    /// <summary>
    /// Cyclic shift mutation operator.
    /// </summary>
    internal class CyclicBitShiftMutation : IMutation
    {
        /// <summary>
        /// Invokes the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        public IChromosome Invoke(IChromosome chromosome)
        {
            if (!(chromosome is BinaryChromosome))
                throw new ArgumentException("Error! Incompatible chromosome.");

            var binChromosome = chromosome as BinaryChromosome;

            var bits = binChromosome.Bits;
            var lsb = bits & 1;
            bits >>= 1;
            bits |= (lsb == 1 ? 0x80000000 : 0);
            return new BinaryChromosome(bits, binChromosome.BitCount);
        }
    }
}
