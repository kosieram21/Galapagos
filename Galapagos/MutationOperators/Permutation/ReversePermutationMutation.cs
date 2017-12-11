using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.MutationOperators.Shared;
using Galapagos.Chromosomes;
using Galapagos.API;

namespace Galapagos.MutationOperators.Permutation
{
    /// <summary>
    /// Reverse mutation operator.
    /// </summary>
    public class ReversePermutationMutation : ReverseMutation<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ReversePermutationMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal ReversePermutationMutation(double weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(PermutationChromosome chromosome)
        {
            var permutation = Reverse(chromosome.Permutation);

            return new PermutationChromosome(permutation);
        }
    }
}
