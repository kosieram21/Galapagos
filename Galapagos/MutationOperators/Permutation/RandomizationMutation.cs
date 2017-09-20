using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Permutation
{
    /// <summary>
    /// Randomization mutation operator.
    /// </summary>
    internal class RandomizationMutation : IMutation
    {
        /// <summary>
        /// Invokes the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        public IChromosome Invoke(IChromosome chromosome)
        {
            if (!(chromosome is PermutationChromosome))
                throw new ArgumentException("Error! Incompatible chromosome.");

            var permChromosome = chromosome as PermutationChromosome;

            return new PermutationChromosome(permChromosome.N);
        }
    }
}
