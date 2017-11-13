using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.SelectionAlgorithms
{
    /// <summary>
    /// Fitness proportionate selection algorithm.
    /// </summary>
    public class FitnessProportionateSelection : ISelectionAlgorithm
    {
        protected ICreature[] _creatures;
        protected double F = 0;

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        internal FitnessProportionateSelection()
        {
        }

        /// <summary>
        /// Initializes the selection algorithm.
        /// </summary>
        /// <param name="population">The population to select from.</param>
        public virtual void Initialize(IPopulation population)
        {
            _creatures = ((Species)population).ToArray()
                .OrderByDescending(creature => creature.Fitness).ToArray();

            F = 0;
            foreach (var creature in _creatures)
            {
                if (!(creature.Fitness > 0))
                    throw new InvalidOperationException("Error! Fitness porprtionate selction requires a positive fitness value");

                F += creature.Fitness;
            }
        }

        /// <summary>
        /// Invokes the selection algorithm.
        /// </summary>
        /// <returns>The selected creature.</returns>
        public virtual ICreature Invoke()
        {
            var value = Stochastic.NextDouble() * F;
            foreach(var creature in _creatures)
            {
                value -= creature.Fitness;
                if (value <= 0) return creature;
            }

            return null;
        }
    }
}
