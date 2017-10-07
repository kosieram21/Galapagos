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
    internal class RandomizationMutation : Mutation<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="RandomizationMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public RandomizationMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(PermutationChromosome chromosome)
        {
            return new PermutationChromosome(chromosome.N);
        }
    }
}
