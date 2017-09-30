using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.CrossoverOperators;
using Galapagos.MutationOperators;

namespace Galapagos
{
    /// <summary>
    /// A genetic description of a creature.
    /// </summary>
    public class GeneticDescription : IEnumerable<GeneticDescription.ChromosomeMetadata>
    {
        /// <summary>
        /// Meta data for a creature chromosome.
        /// </summary>
        public class ChromosomeMetadata
        {
            private readonly string _name;
            private readonly uint _generCount;
            private readonly ChromosomeType _type;
            private readonly double _crossoverRate;
            private readonly double _mutationRate;

            protected IList<ICrossover> _crossovers;
            protected IList<IMutation> _mutations;

            /// <summary>
            /// Constructs a new instance of the <see cref="ChromosomeMetadata"/> class.
            /// </summary>
            /// <param name="name">The chromosome name.</param>
            /// <param name="geneCount">The gene count.</param>
            /// <param name="type">The chromosome type.</param>
            /// <param name="crossoverRate">The crossover rate.</param>
            /// <param name="mutationRate">The mutation rate.</param>
            private ChromosomeMetadata(string name, uint geneCount, ChromosomeType type, double crossoverRate = 1, double mutationRate = 0.25)
            {
                if (crossoverRate < 0 || crossoverRate > 1)
                    throw new ArgumentException("Error! Crossover rate must be a value between 0 and 1.");

                if (mutationRate < 0 || mutationRate > 1)
                    throw new ArgumentException("Error! Mutation rate must be a value between 0 and 1.");

                _name = name;
                _generCount = geneCount;
                _type = type;
                _crossoverRate = crossoverRate;
                _mutationRate = mutationRate;
            }

            /// <summary>
            /// Constructs a new instance of the <see cref="ChromosomeMetadata"/> class.
            /// </summary>
            /// <param name="name">The chromosome name.</param>
            /// <param name="geneCount">The gene count.</param>
            /// <param name="type">The chromosome type.</param>
            /// <param name="crossoverRate">The crossover rate.</param>
            /// <param name="mutationRate">The mutation rate.</param>
            /// <param name="crossoverOptions">The crossover options.</param>
            /// <param name="mutationOptions">The mutation options.</param>
            public ChromosomeMetadata(string name, uint geneCount, ChromosomeType type, uint bitCount = 32, double crossoverRate = 1, double mutationRate = 0.25,
                BinaryCrossover crossoverOptions = BinaryCrossover.SinglePoint,
                BinaryMutation mutationOptions = BinaryMutation.FlipBit | BinaryMutation.SingleBit)
                : this(name, geneCount,type, crossoverRate, mutationRate)
            {
                if (type != ChromosomeType.Binary)
                    throw new ArgumentException("Error! Binary mutaions and crossovers can only be used with binary chromosomes.");

                _crossovers = GeneticFactory.ConstructBinaryCrossoverOperators(crossoverOptions);
                _mutations = GeneticFactory.ConstructBinaryMutationOperators(mutationOptions);
            }

            /// <summary>
            /// Constructs a new instance of the <see cref="ChromosomeMetadata"/> class.
            /// </summary>
            /// <param name="name">The chromosome name.</param>
            /// <param name="geneCount">The gene count.</param>
            /// <param name="type">The chromosome type.</param>
            /// <param name="crossoverRate">The crossover rate.</param>
            /// <param name="mutationRate">The mutation rate.</param>
            /// <param name="crossoverOptions">The crossover options.</param>
            /// <param name="mutationOptions">The mutation options.</param>
            public ChromosomeMetadata(string name, uint geneCount, ChromosomeType type, double crossoverRate = 1, double mutationRate = 0.25,
                PermutationCrossover crossoverOptions = PermutationCrossover.Order,
                PermutationMutation mutationOptions = PermutationMutation.Transposition)
                : this(name, geneCount, type, crossoverRate, mutationRate)
            {
                if (type != ChromosomeType.Permutation)
                    throw new ArgumentException("Error! Permutation mutaions and crossovers can only be used with permutation chromosomes.");

                _crossovers = GeneticFactory.ConstructPermutationCrossoverOperators(crossoverOptions);
                _mutations = GeneticFactory.ConstructPermutationMutationOperators(mutationOptions);
            }

            /// <summary>
            /// The chromosome name.
            /// </summary>
            public string Name => _name;

            /// <summary>
            /// The number of genes in the chromosome.
            /// </summary>
            public uint GeneCount => _generCount;

            /// <summary>
            /// The chromosome type.
            /// </summary>
            public ChromosomeType Type => _type;

            /// <summary>
            /// The crossover rate.
            /// </summary>
            public double CrossoverRate => _crossoverRate;

            /// <summary>
            /// The mutation rate.
            /// </summary>
            public double MutationRate => _mutationRate;

            /// <summary>
            /// The crossover operators.
            /// </summary>
            internal IList<ICrossover> Crossovers => _crossovers;

            /// <summary>
            /// The mutation operators.
            /// </summary>
            internal IList<IMutation> Mutations => _mutations;
        }

        private readonly IList<ChromosomeMetadata> _chromosomeMetadata = new List<ChromosomeMetadata>();
        private readonly Func<Creature, double> _fitnessFunction;

        /// <summary>
        /// Constructs a new instance of the <see cref="GeneticDescription"/> class.
        /// </summary>
        /// <param name="fitnessFunction">The fitness function.</param>
        /// <param name="chromosomeMetadata">The chromosome metadata.</param>
        public GeneticDescription(Func<Creature, double> fitnessFunction, IList<ChromosomeMetadata> chromosomeMetadata = null)
        {
            _fitnessFunction = fitnessFunction;
            if (chromosomeMetadata != null)
            {
                foreach (var metadata in chromosomeMetadata)
                    Add(metadata);
            }
        }

        /// <summary>
        /// Gets the fitness function.
        /// </summary>
        internal Func<Creature, double> FitnessFunction => _fitnessFunction;

        /// <summary>
        /// Adds chromosome metadata to the genetic description.
        /// </summary>
        /// <param name="metadata">The metadata to add.</param>
        public void Add(ChromosomeMetadata metadata)
        {
            if (_chromosomeMetadata.Any(t => t.Name == metadata.Name))
                throw new ArgumentException($"Error! Chromosome metadata named {metadata.Name} already exists in the description.");

            _chromosomeMetadata.Add(metadata);
        }

        /// <summary>
        /// Gets chromosome metadata from the description.
        /// </summary>
        /// <param name="i">The metadata index.</param>
        /// <returns>The metadata.</returns>
        public ChromosomeMetadata this[int i] => _chromosomeMetadata[i];

        #region IEnumerable Members

        public IEnumerator<ChromosomeMetadata> GetEnumerator()
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
