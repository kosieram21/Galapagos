using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.SelectionAlgorithms
{
    /// <summary>
    /// Fitness proportionate selection algorithm.
    /// </summary>
    internal class FitnessProportionateSelection : ISelectionAlgorithm
    {
        protected readonly Random _rng = new Random(DateTime.Now.Millisecond);

        protected readonly Creature[] _creatures;
        protected readonly uint F = 0;

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        /// <param name="creatures">The creature population.</param>
        public FitnessProportionateSelection(Creature[] creatures)
        {
            _creatures = creatures.OrderByDescending(creature => creature.Fitness).ToArray();
            foreach (var creature in creatures)
                F += creature.Fitness;
        }

        /// <summary>
        /// Invokes the selection algorithm.
        /// </summary>
        /// <returns>The selected creature.</returns>
        public virtual Creature Invoke()
        {
            var value = _rng.NextDouble() * F;
            foreach(var creature in _creatures)
            {
                value -= creature.Fitness;
                if (value <= 0) return creature;
            }

            return null;
        }
    }
}
