using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.SelectionAlgorithms
{
    /// <summary>
    /// Tournament selection algorithm.
    /// </summary>
    internal class TournamentSelection : ISelectionAlgorithm
    {
        private readonly Random _rng = new Random(DateTime.Now.Millisecond);

        private readonly Creature[] _creatures;
        private readonly int K = 2;

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        /// <param name="creatures">The creature population.</param>
        public TournamentSelection(Creature[] creatures)
            : this(creatures, null) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        /// <param name="creatures">The creature population.</param>
        /// <param name="k">The tournament size.</param>
        public TournamentSelection(Creature[] creatures, int? k)
        {
            if (k != null) K = (int)k;
            _creatures = creatures;
        }

        /// <summary>
        /// Invokes the selection algorithm.
        /// </summary>
        /// <returns>The selected creature.</returns>
        public Creature Invoke()
        {
            var size = _creatures.Count();
            Creature best = null;

            for (var i = 0; i < K; i++)
            {
                var j = _rng.Next(0, size - 1);
                var creature = _creatures[j];
                if (best == null || creature.Fitness > best.Fitness)
                    best = creature;
            }

            return best;
        }
    }
}
