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
    /// Scramble  mutation operator.
    /// </summary>
    public class ScramblePermutationMutation : ScrambleMutation<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ScramblePermutationMutation"/> class.
        /// </summary>
        /// <param name="weigth">The mutation weight.</param>
        internal ScramblePermutationMutation(double weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(PermutationChromosome chromosome)
        {
            var permutation = Scramble(chromosome.Permutation);

            return new PermutationChromosome(permutation);
        }
    }
}
