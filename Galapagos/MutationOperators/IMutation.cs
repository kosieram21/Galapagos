using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators
{
    public interface IMutation
    {
        /// <summary>
        /// Gets the mutation weight.
        /// </summary>
        uint Weight { get; }

        /// <summary>
        /// Invokes the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        IChromosome Invoke(IChromosome chromosome);
    }
}
