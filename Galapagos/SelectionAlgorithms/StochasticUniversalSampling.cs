using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.SelectionAlgorithms
{
    /// <summary>
    /// Stochastic universal sampling selection algorithm.
    /// </summary>
    public class StochasticUniversalSampling : FitnessProportionateSelection, ISelectionAlgorithm
    {
        private readonly IList<Creature> _selection = new List<Creature>();
        private readonly uint N = 100;

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        internal StochasticUniversalSampling() 
            : this(null) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="FitnessProportionateSelection"/> class.
        /// </summary>
        /// <param name="n">The number of creatures to select.</param>
        internal StochasticUniversalSampling(uint? n)
        {
            if (n != null) N = (uint)n;
        }

        /// <summary>
        /// Initializes the selection algorithm.
        /// </summary>
        /// <param name="population">The population to select from.</param>
        public override void Initialize(IPopulation population)
        {
            base.Initialize(population);
            ComputeSelection();
        }

        /// <summary>
        /// Invokes the selection algorithm.
        /// </summary>
        /// <returns>The selected creature.</returns>
        public override ICreature Invoke()
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
