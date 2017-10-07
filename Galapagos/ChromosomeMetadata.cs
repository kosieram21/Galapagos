using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.CrossoverOperators;
using Galapagos.MutationOperators;

namespace Galapagos
{
    /// <summary>
    /// Metadata for a creature chromosome.
    /// </summary>
    public abstract class ChromosomeMetadata
    {
        protected IList<ICrossover> _crossovers;
        protected IList<IMutation> _mutations;

        /// <summary>
        /// Constructs a new instance of the <see cref="ChromosomeMetadata"/> class.
        /// </summary>
        /// <param name="name">The chromosome name.</param>
        /// <param name="geneCount">The gene count.</param>
        /// <param name="crossoverRate">The crossover rate.</param>
        /// <param name="mutationRate">The mutation rate.</param>
        protected ChromosomeMetadata(string name, uint geneCount, double crossoverRate = 1, double mutationRate = 0.25)
        {
            if (crossoverRate < 0 || crossoverRate > 1)
                throw new ArgumentException("Error! Crossover rate must be a value between 0 and 1.");

            if (mutationRate < 0 || mutationRate > 1)
                throw new ArgumentException("Error! Mutation rate must be a value between 0 and 1.");

            Name = name;
            GeneCount = geneCount;
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
        }

        /// <summary>
        /// The chromosome name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The number of genes in the chromosome.
        /// </summary>
        public uint GeneCount { get; set; }

        /// <summary>
        /// The chromosome type.
        /// </summary>
        public ChromosomeType Type { get; protected set; }

        /// <summary>
        /// The crossover rate.
        /// </summary>
        public double CrossoverRate { get; set; }

        /// <summary>
        /// The mutation rate.
        /// </summary>
        public double MutationRate { get; set; }

        /// <summary>
        /// The crossover operators.
        /// </summary>
        internal IList<ICrossover> Crossovers => _crossovers;

        /// <summary>
        /// The mutation operators.
        /// </summary>
        internal IList<IMutation> Mutations => _mutations;
    }

    /// <summary>
    /// Metadata for a binary creature chromosome.
    /// </summary>
    public class BinaryChromosomeMetadata : ChromosomeMetadata
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="BinaryChromosomeMetadata"/> class.
        /// </summary>
        /// <param name="name">The chromosome name.</param>
        /// <param name="geneCount">The gene count.</param>
        /// <param name="crossoverRate">The crossover rate.</param>
        /// <param name="mutationRate">The mutation rate.</param>
        /// <param name="crossoverOptions">The crossover options.</param>
        /// <param name="mutationOptions">The mutation options.</param>
        public BinaryChromosomeMetadata(string name, uint geneCount, double crossoverRate = 1, double mutationRate = 0.25,
            BinaryCrossover crossoverOptions = BinaryCrossover.SinglePoint,
            BinaryMutation mutationOptions = BinaryMutation.FlipBit | BinaryMutation.SingleBit)
            : base(name, geneCount, crossoverRate, mutationRate)
        {
            Type = ChromosomeType.Binary;

            _crossovers = GeneticFactory.ConstructBinaryCrossoverOperators(crossoverOptions);
            _mutations = GeneticFactory.ConstructBinaryMutationOperators(mutationOptions);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BinaryChromosomeMetadata"/> class.
        /// </summary>
        /// <param name="name">The chromosome name.</param>
        /// <param name="geneCount">The gene count.</param>
        /// <param name="crossoverRate">The crossover rate.</param>
        /// <param name="mutationRate">The mutation rate.</param>
        /// <param name="crossoverOptions">The crossover options.</param>
        /// <param name="mutationOptions">The mutation options.</param>
        public BinaryChromosomeMetadata(string name, int geneCount, ChromosomeType type, double crossoverRate = 1, double mutationRate = 0.25,
            BinaryCrossover crossoverOptions = BinaryCrossover.SinglePoint,
            BinaryMutation mutationOptions = BinaryMutation.FlipBit | BinaryMutation.SingleBit)
            : this(name, (uint)geneCount, crossoverRate, mutationRate, crossoverOptions, mutationOptions)
        {
            if (geneCount <= 0)
                throw new ArgumentException("Error! Gene count must be a positive value.");
        }
    }

    public class PermutationChromosomeMetadata : ChromosomeMetadata
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="PermutationChromosomeMetadata"/> class.
        /// </summary>
        /// <param name="name">The chromosome name.</param>
        /// <param name="geneCount">The gene count.</param>
        /// <param name="crossoverRate">The crossover rate.</param>
        /// <param name="mutationRate">The mutation rate.</param>
        /// <param name="crossoverOptions">The crossover options.</param>
        /// <param name="mutationOptions">The mutation options.</param>
        public PermutationChromosomeMetadata(string name, uint geneCount, double crossoverRate = 1, double mutationRate = 0.25,
            PermutationCrossover crossoverOptions = PermutationCrossover.Order,
            PermutationMutation mutationOptions = PermutationMutation.Transposition)
            : base(name, geneCount, crossoverRate, mutationRate)
        {
            Type = ChromosomeType.Permutation;

            _crossovers = GeneticFactory.ConstructPermutationCrossoverOperators(crossoverOptions);
            _mutations = GeneticFactory.ConstructPermutationMutationOperators(mutationOptions);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="PermutationChromosomeMetadata"/> class.
        /// </summary>
        /// <param name="name">The chromosome name.</param>
        /// <param name="geneCount">The gene count.</param>
        /// <param name="crossoverRate">The crossover rate.</param>
        /// <param name="mutationRate">The mutation rate.</param>
        /// <param name="crossoverOptions">The crossover options.</param>
        /// <param name="mutationOptions">The mutation options.</param>
        public PermutationChromosomeMetadata(string name, int geneCount, double crossoverRate = 1, double mutationRate = 0.25,
            PermutationCrossover crossoverOptions = PermutationCrossover.Order,
            PermutationMutation mutationOptions = PermutationMutation.Transposition)
            : this(name, (uint)geneCount, crossoverRate, mutationRate, crossoverOptions, mutationOptions)
        {
            if (geneCount <= 0)
                throw new ArgumentException("Error! Gene count must be a positive value.");
        }
    }
}
