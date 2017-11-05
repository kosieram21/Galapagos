using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.SelectionAlgorithms
{
    /// <summary>
    /// Truncation selection algorithm.
    /// </summary>
    public class TruncationSelection : ISelectionAlgorithm
    {
        private readonly Random _rng = new Random(DateTime.Now.Millisecond);

        private Creature[] _creatures;
        private List<Creature> _truncation;
        private readonly double _truncationRate = 0.33;

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        internal TruncationSelection()
            : this(null) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        /// <param name="truncationRate">The truncation rate.</param>
        internal TruncationSelection(double? truncationRate)
        {
            if (truncationRate != null) _truncationRate = (double)truncationRate;
        }

        /// <summary>
        /// Initializes the selection algorithm.
        /// </summary>
        /// <param name="population">The population to select from.</param>
        public void Initialize(IPopulation population)
        {
            _creatures = ((Population)population).Creatures;
            _truncation = ComputeTruncation();
        }

        /// <summary>
        /// Invokes the selection algorithm.
        /// </summary>
        /// <returns>The selected creature.</returns>
        public ICreature Invoke()
        {
            var size = _creatures.Count();
            var i = Stochastic.Next((size * _truncationRate) - 1);

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
