using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.TerminationConditions
{
    /// <summary>
    /// Fitness plateau termination condition.
    /// </summary>
    internal class FitnessPlateau : ITerminationCondition
    {
        private readonly Population _population;
        private readonly int _plateauLength;

        private uint _bestFitness = 1;
        private int _count = 0;

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessPlateau"/> class.
        /// </summary>
        /// <param name="population">The creature population.</param>
        /// <param name="plateauLength">The fitness plateau length.</param>
        public FitnessPlateau(Population population, int plateauLength)
        {
            _population = population;
            _plateauLength = plateauLength;
        }

        /// <summary>
        /// Checks the termination condition.
        /// </summary>
        /// <returns>A value indicating if evolution should terminate.</returns>
        public bool Check()
        {
            if (_population.OptimalCreature.Fitness > _bestFitness)
            {
                _bestFitness = _population.OptimalCreature.Fitness;
                _count = 0;
            }
            else
            {
                _count++;
            }

            return _count >= _plateauLength;
        }
    }
}
