using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.TerminationConditions
{
    /// <summary>
    /// A termination condition based on a minimum fitness threshold.
    /// </summary>
    internal class FitnessThreshold : ITerminationCondition
    {
        private readonly Population _population;
        private readonly uint _fitnessThreshold;

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessThreshold"/> class.
        /// </summary>
        /// <param name="population">The creature population.</param>
        /// <param name="fitnessThreshold">The fitness threshold.</param>
        public FitnessThreshold(Population population, uint fitnessThreshold)
        {
            _population = population;
            _fitnessThreshold = fitnessThreshold;
        }

        /// <summary>
        /// Checks the termination condition.
        /// </summary>
        /// <returns>A value indicating if evolution should terminate.</returns>
        public bool Check()
        {
            return _population.OptimalCreature.Fitness >= _fitnessThreshold;
        }
    }
}
