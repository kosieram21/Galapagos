using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.CrossoverOperators.Permutation
{
    /// <summary>
    /// NoOp permutation crossover.
    /// </summary>
    internal class NoOpPermutationCrossover : Crossover<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="NoOpPermutationCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public NoOpPermutationCrossover(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation of the crossover operator.
        /// </summary>
        /// <param name="x">The mother chromosome.</param>
        /// <param name="y">The father chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(PermutationChromosome x, PermutationChromosome y)
        {
            return Stochastic.FlipCoin() ? x : y;
        }
    }
}
