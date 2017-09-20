using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.CrossoverOperators.Permutation
{
    /// <summary>
    /// Alternating position crossover operator.
    /// </summary>
    internal class AlternatingPositionCrossover : ICrossover
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

            if (permChromosomeX.N != permChromosomeY.N)
                throw new ArgumentException("Error! Incompatible chromosomes.");

            var seen = new bool[permChromosomeX.N];
            var permutation = new uint[permChromosomeX.N];

            var j = 0;
            for(var i = 0; i < permChromosomeX.N; i++)
            {
                var numX = permChromosomeX.Permutation[i];
                var numY = permChromosomeY.Permutation[i];

                if(!seen[numX])
                {
                    permutation[j] = numX;
                    seen[numX] = true;
                    j++;
                }

                if(!seen[numY])
                {
                    permutation[j] = numY;
                    seen[numY] = true;
                    j++;
                }

                if (j == permChromosomeX.N)
                    break;
            }

            return new PermutationChromosome(permutation);
        }
    }
}
