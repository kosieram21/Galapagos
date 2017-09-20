using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Permutation
{
    /// <summary>
    /// Cyclic shift mutation operator.
    /// </summary>
    internal class CyclicShiftMutation : IMutation
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

            var first = permutation[0];
            for (var i = 1; i < permChromosome.N; i++)
                permutation[i - 1] = permutation[i];
            permutation[permChromosome.N - 1] = first;

            return new PermutationChromosome(permutation);
        }
    }
}
