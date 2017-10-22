using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.CrossoverOperators.Binary
{
    /// <summary>
    /// Uniform crossover operator.
    /// </summary>
    public class UniformCrossover : Crossover<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="UniformCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal UniformCrossover(uint weigth = 1)
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

            for (var i = 0; i < x.BitCount; i++)
                bits[i] = Stochastic.FlipCoin() ?
                    x.Bits[i] : y.Bits[i];

            return new BinaryChromosome(bits);
        }
    }
}
