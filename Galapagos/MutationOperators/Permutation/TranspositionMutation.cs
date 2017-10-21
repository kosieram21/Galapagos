using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Permutation
{
    /// <summary>
    /// Transposition mutation operator.
    /// </summary>
    public class TranspositionMutation : Mutation<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="TranspositionMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal TranspositionMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(PermutationChromosome chromosome)
        {
            var permutation = chromosome.Permutation;
            long i = 0;
            long j = 0;

            do
            {
                i = Stochastic.Next(chromosome.N);
                j = Stochastic.Next(chromosome.N);
            } while (i == j);

            var temp = permutation[j];
            permutation[j] = permutation[i];
            permutation[i] = temp;

            return new PermutationChromosome(permutation);
        }
    }
}
