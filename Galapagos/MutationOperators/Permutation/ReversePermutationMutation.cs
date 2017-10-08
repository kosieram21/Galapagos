using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.MutationOperators.Shared;

namespace Galapagos.MutationOperators.Permutation
{
    /// <summary>
    /// Reverse mutation operator.
    /// </summary>
    internal class ReversePermutationMutation : ReverseMutation<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ReversePermutationMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public ReversePermutationMutation(uint weigth = 1)
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
