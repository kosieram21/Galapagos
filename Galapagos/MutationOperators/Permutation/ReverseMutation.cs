using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Permutation
{
    /// <summary>
    /// Reverse mutation operator.
    /// </summary>
    internal class ReverseMutation : IMutation
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
            var permutation = permChromosome.Permutation;

            for(var i = 0; i < permChromosome.N / 2; i++)
            {
                permutation[i] = permutation[i] + permutation[permChromosome.N - 1 - i];
                permutation[permChromosome.N - 1 - i] = permutation[i] - permutation[permChromosome.N - 1 - i];
                permutation[i] = permutation[i] - permutation[permChromosome.N - 1 - i];
            }

            return new PermutationChromosome(permutation);
        }
    }
}
