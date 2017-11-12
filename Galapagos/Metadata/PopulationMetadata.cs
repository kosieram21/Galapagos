using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;
using Galapagos.API.Factory;

namespace Galapagos.Metadata
{
    /// <summary>
    /// Metadata for a creature.
    /// </summary>
    public class PopulationMetadata : IPopulationMetadata
    {
        private const uint DEFAULT_SIZE = 1000;
        private const double DEFAULT_SURVIVAL_RATE = 0;
        private const uint DEFAULT_DISTANCE_THRESHOLD = 0;
        private const SelectionAlgorithm DEFAULT_SELECTION_ALGORITHM = API.SelectionAlgorithm.FitnessProportionate;
        private const TerminationCondition DEFAULT_TERMINATION_CONDITION = TerminationCondition.GenerationThreshold;
        private const uint DEFAULT_GENERATION_THRESHOLD = 1000;

        private readonly List<ITerminationCondition> _terminationConditions = new List<ITerminationCondition>();

        private readonly IList<IChromosomeMetadata> _chromosomeMetadata = new List<IChromosomeMetadata>();

        /// <summary>
        /// Constructs a new instance of the <see cref="ChromosomeMetadata"/> class.
        /// </summary>
        internal PopulationMetadata()
        {
            Size = DEFAULT_SIZE;
            SurvivalRate = DEFAULT_SURVIVAL_RATE;
            DistanceThreshold = DEFAULT_DISTANCE_THRESHOLD;
            SelectionAlgorithm = GeneticFactory.ConstructSelectionAlgorithm(DEFAULT_SELECTION_ALGORITHM);
        }

        /// <summary>
        /// Gets the population size.
        /// </summary>
        public uint Size { get; internal set; }

        /// <summary>
        /// Gets the survival rate used in elitism. A survival rate of 0 disables elitism.
        /// </summary>
        public double SurvivalRate { get; internal set; }

        /// <summary>
        /// Gets the distance threshold for niches. A distance threshold of 0 disable niches.
        /// </summary>
        public uint DistanceThreshold { get; internal set; }

        /// <summary>
        /// Gets the selection algorithm.
        /// </summary>
        public ISelectionAlgorithm SelectionAlgorithm { get; internal set; }

        /// <summary>
        /// Gets the termination conditions.
        /// </summary>
        public IReadOnlyList<ITerminationCondition> TerminationConditions
        {
            get
            {
                if (!_terminationConditions.Any())
                    _terminationConditions.Add(GeneticFactory.ConstructTerminationCondition(DEFAULT_TERMINATION_CONDITION, DEFAULT_GENERATION_THRESHOLD));
                return _terminationConditions;
            }
        }

        /// <summary>
        /// Gets the fitness function.
        /// </summary>
        public Func<ICreature, double> FitnessFunction { get; internal set; }

        /// <summary>
        /// Adds a termination condition to the metadata.
        /// </summary>
        /// <param name="condition"></param>
        internal void AddTerminationCondition(ITerminationCondition condition)
        {
            _terminationConditions.Add(condition);
        }

        /// <summary>
        /// Adds chromosome metadata to the metadata.
        /// </summary>
        /// <param name="metadata">The metadata to add.</param>
        internal void AddChromosomeMetadata(IChromosomeMetadata metadata)
        {
            if (_chromosomeMetadata.Any(t => t.Name == metadata.Name))
                throw new ArgumentException($"Error! Chromosome metadata named {metadata.Name} already exists.");

            _chromosomeMetadata.Add(metadata);
        }

        /// <summary>
        /// Gets chromosome metadata from the description.
        /// </summary>
        /// <param name="i">The metadata index.</param>
        /// <returns>The metadata.</returns>
        public IChromosomeMetadata this[int i]
        {
            get
            {
                if(i >= _chromosomeMetadata.Count)
                    throw new Exception($"Error! {i} is larger than the creature metadata size.");
                return _chromosomeMetadata[i];
            }
            internal set
            {
                if (i >= _chromosomeMetadata.Count)
                    throw new Exception($"Error! {i} is larger than the creature metadata size.");
                _chromosomeMetadata[i] = value;
            }
        }

        #region IEnumerable Members

        public IEnumerator<IChromosomeMetadata> GetEnumerator()
        {
            return _chromosomeMetadata.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
