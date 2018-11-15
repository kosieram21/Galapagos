using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.MutationOperators.Shared;
using Galapagos.Chromosomes;
using Galapagos.API;

namespace Galapagos.MutationOperators.Binary
{
    /// <summary>
    /// Scramble  mutation operator.
    /// </summary>
    public class ScrambleBinaryMutation : ScrambleMutation<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ScrambleBinaryMutation"/> class.
        /// </summary>
        /// <param name="weigth">The mutation weight.</param>
        internal ScrambleBinaryMutation(double weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(BinaryChromosome chromosome)
        {
            var bits = Scramble(chromosome.Bits);

            return new BinaryChromosome(bits);
        }
    }
}
