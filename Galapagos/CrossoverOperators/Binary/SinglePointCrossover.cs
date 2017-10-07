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
    internal class SinglePointCrossover : Crossover<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="SinglePointCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public SinglePointCrossover(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation of the crossover operator.
        /// </summary>
        /// <param name="x">The mother chromosome.</param>
        /// <param name="y">The father chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(BinaryChromosome x, BinaryChromosome y)
        {
            if (x.BitCount != y.BitCount)
                throw new ArgumentException("Error! Incompatible chromosomes.");

            var LowerCrossoverRegion = ComputeLowerCrossOverRegion(x);
            var UpperCrossoverRegion = x.Mask ^ LowerCrossoverRegion;

            return new BinaryChromosome((x.Bits & UpperCrossoverRegion) | (y.Bits & LowerCrossoverRegion), x.BitCount);
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
