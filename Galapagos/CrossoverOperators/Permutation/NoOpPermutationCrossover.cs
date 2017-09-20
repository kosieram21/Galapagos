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
        private static readonly Random _rng = new Random(DateTime.Now.Millisecond);

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

            var flip = (double)(_rng.Next() % 100) / 100;
            return flip < 0.5 ? permChromosomeX : permChromosomeY;
        }
    }
}
