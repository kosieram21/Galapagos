using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Binary
{
    /// <summary>
    /// Boundary mutation operator.
    /// </summary>
    internal class BoundaryMutation : IMutation
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

            return Stochastic.FlipCoin() ? 
                new BinaryChromosome(uint.MaxValue, binChromosome.BitCount) : 
                new BinaryChromosome(0, binChromosome.BitCount);
        }
    }
}
