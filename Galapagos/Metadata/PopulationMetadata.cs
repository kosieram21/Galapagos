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
        private const double DEFAULT_DISTANCE_THRESHOLD = 0;
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
        /// Gets or sets the population size.
        /// </summary>
        public uint Size { get; set; }

        /// <summary>
        /// Gets or sets the survival rate used in elitism. A survival rate of 0 disables elitism.
        /// </summary>
        public double SurvivalRate { get; set; }

        /// <summary>
        /// Gets or sets the distance threshold for niches. A distance threshold of 0 disable niches.
        /// </summary>
        public double DistanceThreshold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if cooperative coevolution should be used.
        /// </summary>
        public bool CooperativeCoevolution { get; set; }

        /// <summary>
        /// Gets or sets the selection algorithm.
        /// </summary>
        public ISelectionAlgorithm SelectionAlgorithm { get; set; }

        /// <summary>
        /// Gets the termination conditions.
        /// </summary>
        public IList<ITerminationCondition> TerminationConditions
        {
            get
            {
                if (!_terminationConditions.Any())
                    _terminationConditions.Add(GeneticFactory.ConstructTerminationCondition(DEFAULT_TERMINATION_CONDITION, DEFAULT_GENERATION_THRESHOLD));
                return _terminationConditions;
            }
        }

        /// <summary>
        /// Gets or sets the fitness function.
        /// </summary>
        public Func<ICreature, double> FitnessFunction { get; set; }

        /// <summary>
        /// Adds a termination condition to the metadata.
        /// </summary>
        /// <param name="condition"></param>
        internal void AddTerminationCondition(ITerminationCondition condition)
        {
            _terminationConditions.Add(condition);
        }

        /// <summary>
        /// Adds the termination conditions to the metadata.
        /// </summary>
        /// <param name="conditions"></param>
        internal void AddTerminationCondition(IList<ITerminationCondition> conditions)
        {
            foreach (var condition in conditions)
                AddTerminationCondition(condition);
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
        /// Adds the chromosome metadata to the metadata.
        /// </summary>
        /// <param name="metadata">The metadata to add.</param>
        internal void AddChromosomeMetadata(IList<IChromosomeMetadata> metadataList)
        {
            foreach (var metadata in metadataList)
                AddChromosomeMetadata(metadata);
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
