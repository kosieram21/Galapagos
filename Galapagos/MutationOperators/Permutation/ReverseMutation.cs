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

            var start = Stochastic.Next(chromosome.N - 1);
            var end = Stochastic.Next(start + 1, chromosome.N);

            for(var i = 0; i < (end - start) / 2; i++)
            {
                var temp = permutation[start + i];
                permutation[start + i] = permutation[end - i];
                permutation[end - i] = temp;
            }

            return new PermutationChromosome(permutation);
        }
    }
}
