using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.TerminationConditions
{
    /// <summary>
    /// A termination condition based on a creature generation limit.
    /// </summary>
    public class GenerationThreshold : ITerminationCondition
    {
        private readonly Population _population;
        private readonly int _generationThreshold;

        /// <summary>
        /// Constructs a new instance of the <see cref="GenerationThreshold"/> class.
        /// </summary>
        /// <param name="population">The creature population.</param>
        /// <param name="generationThreshold">The generation threshold.</param>
        internal GenerationThreshold(Population population, int generationThreshold)
        {
            _population = population;
            _generationThreshold = generationThreshold;
        }

        /// <summary>
        /// Checks the termination condition.
        /// </summary>
        /// <returns>A value indicating if evolution should terminate.</returns>
        public bool Check()
        {
            return _population.Generation >= _generationThreshold;
        }
    }
}
