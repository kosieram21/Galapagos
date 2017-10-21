using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos
{
    public interface IChromosome
    {
        /// <summary>
        /// Measures the distance between two chromosome.
        /// </summary>
        /// <param name="other">The other chromosome.</param>
        /// <returns>The distance between the chromosomes.</returns>
        uint Distance(IChromosome other);
    }
}
