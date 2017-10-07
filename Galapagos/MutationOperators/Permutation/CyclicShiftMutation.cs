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
    internal class CyclicShiftMutation : Mutation<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="CyclicShiftMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public CyclicShiftMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(PermutationChromosome chromosome)
        {
            var permutation = chromosome.Permutation;

            var first = permutation[0];
            for (var i = 1; i < chromosome.N; i++)
                permutation[i - 1] = permutation[i];
            permutation[chromosome.N - 1] = first;

            return new PermutationChromosome(permutation);
        }
    }
}
