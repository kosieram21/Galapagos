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
    internal class ReverseMutation : Mutation<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ReverseMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public ReverseMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(PermutationChromosome chromosome)
        {
            var permutation = chromosome.Permutation;

            for(var i = 0; i < chromosome.N / 2; i++)
            {
                permutation[i] = permutation[i] + permutation[chromosome.N - 1 - i];
                permutation[chromosome.N - 1 - i] = permutation[i] - permutation[chromosome.N - 1 - i];
                permutation[i] = permutation[i] - permutation[chromosome.N - 1 - i];
            }

            return new PermutationChromosome(permutation);
        }
    }
}
