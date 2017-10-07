using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.CrossoverOperators.Binary
{
    /// <summary>
    /// NoOp binary crossover.
    /// </summary>
    internal class NoOpBinaryCrossover : Crossover<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="NoOpBinaryCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public NoOpBinaryCrossover(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation of the crossover operator.
        /// </summary>
        /// <param name="x">The mother chromosome.</param>
        /// <param name="y">The father chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(BinaryChromosome x, BinaryChromosome y)
        {
            return Stochastic.FlipCoin() ? x : y;
        }
    }
}
