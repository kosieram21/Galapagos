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
    internal class NoOpPermutationCrossover : ICrossover
    {
        /// <summary>
        /// Invokes the crossover operator.
        /// </summary>
        /// <param name="x">The mother chromosome.</param>
        /// <param name="y">The father chromosome.</param>
        /// <returns>The new DNA.</returns>
        public IChromosome Invoke(IChromosome x, IChromosome y)
        {
            if (!(x is PermutationChromosome) || !(y is PermutationChromosome))
                throw new ArgumentException("Error! Incompatible chromosomes.");

            var permChromosomeX = x as PermutationChromosome;
            var permChromosomeY = y as PermutationChromosome;

            return Stochastic.FlipCoin() ? permChromosomeX : permChromosomeY;
        }
    }
}
