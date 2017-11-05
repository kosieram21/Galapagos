using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.TerminationConditions
{
    /// <summary>
    /// Fitness plateau termination condition.
    /// </summary>
    public class FitnessPlateau : ITerminationCondition
    {
        private readonly uint _plateauLength;

        private double _bestFitness = 0;
        private int _count = 0;

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessPlateau"/> class.
        /// </summary>
        /// <param name="plateauLength">The fitness plateau length.</param>
        internal FitnessPlateau(uint plateauLength)
        {
            _plateauLength = plateauLength;
        }

        /// <summary>
        /// Checks the termination condition.
        /// </summary>
        /// <param name="population">The population to check against.</param>
        /// <returns>A value indicating if evolution should terminate.</returns>
        public bool Check(IPopulation population)
        {
            if (((Population)population).InternalOptimalCreature.TrueFitness > _bestFitness)
            {
                _bestFitness = ((Population)population).InternalOptimalCreature.TrueFitness;
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
