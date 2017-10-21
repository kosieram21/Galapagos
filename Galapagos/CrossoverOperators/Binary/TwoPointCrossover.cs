using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.CrossoverOperators.Binary
{
    /// <summary>
    /// Two point crossover operator.
    /// </summary>
    public class TwoPointCrossover : Crossover<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="TwoPointCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal TwoPointCrossover(uint weigth = 1)
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

            var bits = new bool[x.BitCount];
            Array.Copy(x.Bits, bits, x.BitCount);

            var start = Stochastic.Next(x.BitCount - 1);
            var end = Stochastic.Next(start + 1, x.BitCount);

            for (var i = start; i < end; i++)
                bits[i] = y.Bits[i];

            return new BinaryChromosome(bits);
        }
    }
}
