using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.SelectionAlgorithms
{
    /// <summary>
    /// Truncation selection algorithm.
    /// </summary>
    internal class TruncationSelection : ISelectionAlgorithm
    {
        private readonly Random _rng = new Random(DateTime.Now.Millisecond);

        private readonly Creature[] _creatures;
        private readonly List<Creature> _truncation;
        private readonly double _truncationRate = 0.33;

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        /// <param name="creatures">The creature population.</param>
        public TruncationSelection(Creature[] creatures)
            : this(creatures, null) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        /// <param name="creatures">The creature population.</param>
        /// <param name="truncationRate">The truncation rate.</param>
        public TruncationSelection(Creature[] creatures, double? truncationRate)
        {
            if (truncationRate != null) _truncationRate = (double)truncationRate;
            _creatures = creatures;
            _truncation = ComputeTruncation();
        }

        /// <summary>
        /// Invokes the selection algorithm.
        /// </summary>
        /// <returns>The selected creature.</returns>
        public Creature Invoke()
        {
            var size = _creatures.Count();
            var i = _rng.Next(0, (int)(size * _truncationRate) - 1);

            return _truncation[i];
        }

        /// <summary>
        /// Computes a trunctation.
        /// </summary>
        /// <returns>The truncation.</returns>
        private List<Creature> ComputeTruncation()
        {
            var size = _creatures.Count();
            var selectedCreatures = new List<Creature>();
            var sortedCreatures = _creatures.OrderByDescending(creature => creature.Fitness).ToArray();

            for (var i = 0; i < size * _truncationRate; i++)
                selectedCreatures.Add(sortedCreatures[i]);

            return selectedCreatures;
        }
    }
}
