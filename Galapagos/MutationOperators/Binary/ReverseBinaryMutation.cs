using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.MutationOperators.Shared;

namespace Galapagos.MutationOperators.Binary
{
    public class ReverseBinaryMutation : ReverseMutation<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ReverseBinaryMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal ReverseBinaryMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(BinaryChromosome chromosome)
        {
            var bits = Reverse(chromosome.Bits);

            return new BinaryChromosome(bits);
        }
    }
}
