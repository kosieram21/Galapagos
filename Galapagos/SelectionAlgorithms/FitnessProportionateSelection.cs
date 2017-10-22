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
        protected readonly Creature[] _creatures;
        protected readonly double F = 0;

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        /// <param name="creatures">The creature population.</param>
        internal FitnessProportionateSelection(Creature[] creatures)
        {
            _creatures = creatures.OrderByDescending(creature => creature.Fitness).ToArray();
            foreach (var creature in creatures)
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
        public virtual Creature Invoke()
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
