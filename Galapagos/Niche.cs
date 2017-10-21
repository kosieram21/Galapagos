using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos
{
    /// <summary>
    /// A collection of creatures on the same fitness peak.
    /// </summary>
    internal class Niche : IEnumerable<Creature>
    {
        private Creature _representative;
        private readonly IList<Creature> _creatures = new List<Creature>();

        private readonly uint _distanceThreshold;

        /// <summary>
        /// Constructs a new instance of the <see cref="Niche"/> class.
        /// </summary>
        /// <param name="creature">The genisis creature.</param>
        public Niche(Creature creature, uint distanceThreshold)
        {
            _representative = creature;
            Add(creature);

            _distanceThreshold = distanceThreshold;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Niche"/> class.
        /// </summary>
        /// <remarks>Python friendly constructor.</remarks>
        /// <param name="creature">The genisis creature.</param>
        public Niche(Creature creature, int distanceThreshold)
            : this(creature, (uint)distanceThreshold)
        {
            if (distanceThreshold <= 0)
                throw new ArgumentException("Error! Distance threshold must be a positive value.");
        }

        /// <summary>
        /// Gets the adjusted fitness for the niche.
        /// </summary>
        public double AdjustedFitness
        {
            get
            {
                double fitness = 0;
                foreach (var creature in _creatures)
                    fitness += creature.Fitness;
                return fitness;
            }
        }

        /// <summary>
        /// The size of the niche.
        /// </summary>
        public int Size => _creatures.Count;

        /// <summary>
        /// Accesses a creature from the population.
        /// </summary>
        /// <param name="index">The creature index.</param>
        /// <returns>The creature.</returns>
        public Creature this[int index]
        {
            get
            {
                if (index >= Size)
                    throw new Exception($"Error! {index} is larger than the population size.");
                return _creatures[index];
            }
        }

        /// <summary>
        /// Determines if the given creature is compatible with the niche.
        /// </summary>
        /// <param name="creature">The creature to evaluate.</param>
        /// <returns>A value indicating if the creature is compatible.</returns>
        public bool Compatible(Creature creature)
        {
            var distance = _representative.Distance(creature);
            return distance <= _distanceThreshold;
        }

        /// <summary>
        /// Adds a creature to the niche.
        /// </summary>
        /// <param name="creature">The creature to add to the niche.</param>
        public void Add(Creature creature)
        {
            _creatures.Add(creature);
            creature.RegisterNiche(this);
        }

        /// <summary>
        /// Clears the niche of creatures.
        /// </summary>
        public void Clear()
        {
            var i = Stochastic.Next(Size);
            _representative = _creatures[i];
            _creatures.Clear();
        }

        #region IEnumerable Members

        public IEnumerator<Creature> GetEnumerator()
        {
            return _creatures.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
