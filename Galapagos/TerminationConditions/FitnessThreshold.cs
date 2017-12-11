using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.TerminationConditions
{
    /// <summary>
    /// A termination condition based on a minimum fitness threshold.
    /// </summary>
    public class FitnessThreshold : ITerminationCondition
    {
        private readonly double _fitnessThreshold;

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessThreshold"/> class.
        /// </summary>
        /// <param name="fitnessThreshold">The fitness threshold.</param>
        internal FitnessThreshold(double fitnessThreshold)
        {
            _fitnessThreshold = fitnessThreshold;
        }

        /// <summary>
        /// Checks the termination condition.
        /// </summary>
        /// <param name="population">The population to check against.</param>
        /// <returns>A value indicating if evolution should terminate.</returns>
        public bool Check(IPopulation population)
        {
            return ((Population)population).OptimalSpecies.InternalOptimalCreature.TrueFitness >= _fitnessThreshold;
        }
    }
}
