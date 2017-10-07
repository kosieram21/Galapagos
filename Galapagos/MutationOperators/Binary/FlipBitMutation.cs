using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Binary
{
    /// <summary>
    /// Flip bit mutation operator.
    /// </summary>
    internal class FlipBitMutation : Mutation<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="FlipBitMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public FlipBitMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(BinaryChromosome chromosome)
        {
            var bits = chromosome.Bits;
            return new BinaryChromosome(uint.MaxValue - bits, chromosome.BitCount);
        }
    }
}
