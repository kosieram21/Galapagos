using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;
using Galapagos.API;

namespace Galapagos.MutationOperators
{
    /// <summary>
    /// Mutation base class.
    /// </summary>
    /// <typeparam name="TChromosome">The chromosome type of this mutation.</typeparam>
    public abstract class Mutation<TChromosome> : IMutation
        where TChromosome : class, IChromosome
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="Mutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        protected Mutation(uint weigth = 1)
        {
            Weight = weigth;
        }

        /// <summary>
        /// Gets the crossover weight.
        /// </summary>
        public uint Weight { get; private set; }

        /// <summary>
        /// Invokes the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        public IChromosome Invoke(IChromosome chromosome)
        {
            if (!(chromosome is TChromosome))
                throw new ArgumentException("Error! Incompatible chromosome.");

            return InternalInvoke((TChromosome)chromosome);
        }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected abstract IChromosome InternalInvoke(TChromosome chromosome);
    }
}
