using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.SelectionAlgorithms
{
    /// <summary>
    /// Stochastic universal sampling selection algorithm.
    /// </summary>
    internal class StochasticUniversalSampling : FitnessProportionateSelection, ISelectionAlgorithm
    {
        private readonly IList<Creature> _selection = new List<Creature>();
        private readonly int N = 100;

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        /// <param name="creatures">The creature population.</param>
        public StochasticUniversalSampling(Creature[] creatures) 
            : this(creatures, null) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        /// <param name="creatures">The creature population.</param>
        /// <param name="n">The number of creatures to select.</param>
        public StochasticUniversalSampling(Creature[] creatures, int? n)
            : base(creatures)
        {
            if (n != null) N = (int)n;
            ComputeSelection();
        }

        /// <summary>
        /// Invokes the selection algorithm.
        /// </summary>
        /// <returns>The selected creature.</returns>
        public override Creature Invoke()
        {
            var size = _selection.Count();
            var i = Stochastic.Next(size);

            return _selection[i];
        }

        /// <summary>
        /// Computes the selection.
        /// </summary>
        private void ComputeSelection()
        {
            var P = F / N;
            var start = Stochastic.Next(P);
            var pointers = new List<double>();
            
            for(var i = 0; i < N; i++)
                pointers.Add(start + (i * (int)P));

            foreach (var pointer in pointers)
            {
                var i = 0;
                var sum = _creatures[0].Fitness;
                while(sum < pointer)
                {
                    i++;
                    sum += _creatures[i].Fitness;
                }
                _selection.Add(_creatures[i]);
            }
        }
    }
}
