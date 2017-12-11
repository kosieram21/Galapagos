using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;
using Galapagos.API;

namespace Galapagos.CrossoverOperators
{
    /// <summary>
    /// Crossover base class.
    /// </summary>
    /// <typeparam name="TChromosome">The chromosome type of this crossover.</typeparam>
    public abstract class Crossover<TChromosome> : ICrossover
        where TChromosome : class, IChromosome
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="Crossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        protected Crossover(double weigth = 1)
        {
            Weight = weigth;
        }

        /// <summary>
        /// Gets the crossover weight.
        /// </summary>
        public double Weight { get; private set; }

        /// <summary>
        /// Invokes the crossover operator.
        /// </summary>
        /// <param name="x">The mother chromosome.</param>
        /// <param name="y">The father chromosome.</param>
        /// <returns>The new DNA.</returns>
        public IChromosome Invoke(IChromosome x, IChromosome y)
        {
            if (!(x is TChromosome) || !(y is TChromosome))
                throw new ArgumentException("Error! Incompatible chromosomes.");

            return InternalInvoke((TChromosome)x, (TChromosome)y);
        }

        /// <summary>
        /// Internal invocation of the crossover operator.
        /// </summary>
        /// <param name="x">The mother chromosome.</param>
        /// <param name="y">The father chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected abstract IChromosome InternalInvoke(TChromosome x, TChromosome y);
    }
}
