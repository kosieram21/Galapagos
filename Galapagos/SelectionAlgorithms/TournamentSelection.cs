using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.SelectionAlgorithms
{
    /// <summary>
    /// Tournament selection algorithm.
    /// </summary>
    public class TournamentSelection : ISelectionAlgorithm
    {
        private ICreature[] _creatures;
        private readonly uint K = 2;

        /// <summary>
        /// Constructs a new instance of the <see cref="TournamentSelection"/> class.
        /// </summary>
        internal TournamentSelection()
            : this(null) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="TournamentSelection"/> class.
        /// </summary>
        /// <param name="k">The tournament size.</param>
        internal TournamentSelection(uint? k)
        {
            if (k != null) K = (uint)k;
        }

        /// <summary>
        /// Initializes the selection algorithm.
        /// </summary>
        /// <param name="population">The population to select from.</param>
        public void Initialize(IPopulation population)
        {
            _creatures = population.ToArray();
        }

        /// <summary>
        /// Invokes the selection algorithm.
        /// </summary>
        /// <returns>The selected creature.</returns>
        public ICreature Invoke()
        {
            var size = _creatures.Count();
            ICreature best = null;

            for (var i = 0; i < K; i++)
            {
                var j = Session.Instance.Stochastic.Next(size);
                var creature = _creatures[j];
                if (best == null || creature.Fitness > best.Fitness)
                    best = creature;
            }

            return best;
        }
    }
}
