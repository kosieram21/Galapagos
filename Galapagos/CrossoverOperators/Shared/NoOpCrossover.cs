using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.CrossoverOperators.Shared
{
    /// <summary>
    /// NoOp crossover.
    /// </summary>
    /// <typeparam name="TChromosome">The chromosome type of this crossover.</typeparam>
    public abstract class NoOpCrossover<TChromosome> : Crossover<TChromosome>
        where TChromosome : class, IChromosome
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="NoOpCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        protected NoOpCrossover(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation of the crossover operator.
        /// </summary>
        /// <param name="x">The mother chromosome.</param>
        /// <param name="y">The father chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(TChromosome x, TChromosome y)
        {
            return Stochastic.FlipCoin() ? x : y;
        }
    }
}
