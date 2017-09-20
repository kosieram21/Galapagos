using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.CrossoverOperators.Binary
{
    /// <summary>
    /// Single point crossover operator.
    /// </summary>
    internal class SinglePointCrossover : ICrossover
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

            var binChromosomeX = x as BinaryChromosome;
            var binChromosomeY = y as BinaryChromosome;

            if (binChromosomeX.BitCount != binChromosomeY.BitCount)
                throw new ArgumentException("Error! Incompatible chromosomes.");

            var LowerCrossoverRegion = ComputeLowerCrossOverRegion(binChromosomeX);
            var UpperCrossoverRegion = binChromosomeX.Mask ^ LowerCrossoverRegion;

            return new BinaryChromosome((binChromosomeX.Bits & UpperCrossoverRegion) | (binChromosomeY.Bits & LowerCrossoverRegion), binChromosomeX.BitCount);
        }

        /// <summary>
        /// Computes the lower crossover region.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The lower crossover region.</returns>
        private uint ComputeLowerCrossOverRegion(BinaryChromosome chromosome)
        {
            uint LowerCrossoverRegion = 0;

            var count = chromosome.BitCount - (chromosome.BitCount / 4);
            for (var i = 0; i < count; i++)
            {
                LowerCrossoverRegion <<= 1;
                LowerCrossoverRegion++;
            }

            return LowerCrossoverRegion;
        }
    }
}
