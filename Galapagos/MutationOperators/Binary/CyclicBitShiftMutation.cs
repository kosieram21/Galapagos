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
    internal class CyclicBitShiftMutation : Mutation<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="CyclicBitShiftMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public CyclicBitShiftMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(BinaryChromosome chromosome)
        {
            var bits = chromosome.Bits;
            var lsb = bits & 1;
            bits >>= 1;
            bits |= (lsb == 1 ? 0x80000000 : 0);
            return new BinaryChromosome(bits, chromosome.BitCount);
        }
    }
}
