using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.MutationOperators.Binary
{
    public class RandomizationBinaryMutation : Mutation<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="RandomizationBinaryMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal RandomizationBinaryMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(BinaryChromosome chromosome)
        {
            return new BinaryChromosome(chromosome.BitCount);
        }
    }
}
