using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.CrossoverOperators.Permutation
{
    /// <summary>
    /// Order crossover operator.
    /// </summary>
    internal class OrderCrossover : ICrossover
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

            if(permChromosomeX.N != permChromosomeY.N)
                throw new ArgumentException("Error! Incompatible chromosomes.");

            var seen = new bool[permChromosomeX.N];
            var midPoint = permChromosomeX.N / 2;
            var permutation = new uint[permChromosomeX.N];

            for (var i = 0; i < midPoint; i++)
            {
                var number = permChromosomeX.Permutation[i];
                permutation[i] = number;
                seen[number] = true;
            }

            var index = midPoint;
            for(var i = 0; i < permChromosomeX.N; i++)
            {
                var number = permChromosomeY.Permutation[i];
                if(!seen[number])
                {
                    permutation[index] = number;
                    index++;
                }
            }

            return new PermutationChromosome(permutation);
        }
    }
}
