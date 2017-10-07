using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.CrossoverOperators
{
    public interface ICrossover : IOperator
    {
        /// <summary>
        /// Invokes the crossover operator.
        /// </summary>
        /// <param name="x">The mother chromosome.</param>
        /// <param name="y">The father chromosome.</param>
        /// <returns>The new DNA.</returns>
        IChromosome Invoke(IChromosome x, IChromosome y);
    }
}
