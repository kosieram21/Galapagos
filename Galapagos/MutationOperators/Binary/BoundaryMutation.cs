using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Binary
{
    /// <summary>
    /// Boundary mutation operator.
    /// </summary>
    internal class BoundaryMutation : Mutation<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="BoundaryMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public BoundaryMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(BinaryChromosome chromosome)
        {
            return Stochastic.FlipCoin() ? 
                new BinaryChromosome(uint.MaxValue, chromosome.BitCount) : 
                new BinaryChromosome(0, chromosome.BitCount);
        }
    }
}
