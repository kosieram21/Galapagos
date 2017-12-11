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
        private readonly uint _generationThreshold;

        /// <summary>
        /// Constructs a new instance of the <see cref="GenerationThreshold"/> class.
        /// </summary>
        /// <param name="generationThreshold">The generation threshold.</param>
        internal GenerationThreshold(uint generationThreshold)
        {
            _generationThreshold = generationThreshold;
        }

        /// <summary>
        /// Checks the termination condition.
        /// </summary>
        /// <param name="population">The population to check against.</param>
        /// <returns>A value indicating if evolution should terminate.</returns>
        public bool Check(IPopulation population)
        {
            return ((Population)population).Generation >= _generationThreshold;
        }
    }
}
