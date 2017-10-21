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
        protected List<ICrossover> _crossovers;
        protected List<IMutation> _mutations;

        protected double _crossoverF = 0;
        protected double _mutationF = 0;

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
        /// The chromosome type.
        /// </summary>
        public ChromosomeType Type { get; protected set; }

        /// <summary>
        /// The number of genes in the chromosome.
        /// </summary>
        public uint GeneCount { get; set; }

        /// <summary>
        /// The crossover rate.
        /// </summary>
        public double CrossoverRate { get; set; }

        /// <summary>
        /// The mutation rate.
        /// </summary>
        public double MutationRate { get; set; }

        /// <summary>
        /// Gets a crossover from the metadata.
        /// </summary>
        /// <returns>A crossover operator.</returns>
        internal ICrossover GetCrossover()
        {
            if (_crossovers == null)
            {
                _crossovers = GetDefaultCrossovers();
                _crossoverF = ComputeWeightedSum(_crossovers);
            }

            return SelectOperator(_crossovers, _crossoverF);
        }

        /// <summary>
        /// Selects a mutation from the metadata.
        /// </summary>
        /// <returns>A mutation operator.</returns>
        internal IMutation GetMutation()
        {
            if (_mutations == null)
            {
                _mutations = GetDefaultMutations();
                _mutationF = ComputeWeightedSum(_mutations);
            }

            return SelectOperator(_mutations, _mutationF);
        }

        /// <summary>
        /// Selects an operator from the list of operators.
        /// </summary>
        /// <typeparam name="TOperator">The operator type.</typeparam>
        /// <param name="operators">The operators.</param>
        /// <param name="F">The weighted sum of the given operators.</param>
        /// <returns>The selected operator.</returns>
        private TOperator SelectOperator<TOperator>(IList<TOperator> operators, double F)
            where TOperator : IOperator
        {
            var value = Stochastic.NextDouble() * F;
            foreach (var op in operators)
            {
                value -= op.Weight;
                if (value <= 0) return op;
            }

            return default(TOperator);
        }

        /// <summary>
        /// Computes the weighted sum of a list of operators.
        /// </summary>
        /// <typeparam name="TOperator">The operator type.</typeparam>
        /// <param name="operators">The operators.</param>
        /// <returns>The weighted sum.</returns>
        protected double ComputeWeightedSum<TOperator>(IList<TOperator> operators)
            where TOperator : IOperator
        {
            double F = 0;

            foreach (var op in operators)
                F += op.Weight;

            return F;
        }

        /// <summary>
        /// Gets the default crossover operators.
        /// </summary>
        /// <returns>The default crossover operators.</returns>
        protected abstract List<ICrossover> GetDefaultCrossovers();

        /// <summary>
        /// Gets the default mutation operators.
        /// </summary>
        /// <returns>The default mutation operators.</returns>
        protected abstract List<IMutation> GetDefaultMutations();
    }

    /// <summary>
    /// Metadata for a binary creature chromosome.
    /// </summary>
    public class BinaryChromosomeMetadata : ChromosomeMetadata
    {
        private const BinaryCrossover DEFAULT_CROSSOVER_OPTIONS = BinaryCrossover.SinglePoint;
        private const BinaryMutation DEFAULT_MUTATION_OPTIONS = BinaryMutation.SingleBit;

        /// <summary>
        /// Constructs a new instance of the <see cref="BinaryChromosomeMetadata"/> class.
        /// </summary>
        /// <param name="name">The chromosome name.</param>
        /// <param name="geneCount">The gene count.</param>
        /// <param name="crossoverRate">The crossover rate.</param>
        /// <param name="mutationRate">The mutation rate.</param>
        public BinaryChromosomeMetadata(string name, uint geneCount, double crossoverRate = 1, double mutationRate = 0.25)
            : base(name, geneCount, crossoverRate, mutationRate)
        {
            Type = ChromosomeType.Binary;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BinaryChromosomeMetadata"/> class.
        /// </summary>
        /// <remarks>Python friendly constructor.</remarks>
        /// <param name="name">The chromosome name.</param>
        /// <param name="geneCount">The gene count.</param>
        /// <param name="crossoverRate">The crossover rate.</param>
        /// <param name="mutationRate">The mutation rate.</param>
        public BinaryChromosomeMetadata(string name, int geneCount, double crossoverRate = 1, double mutationRate = 0.25)
            : this(name, (uint)geneCount, crossoverRate, mutationRate)
        {
            if (geneCount <= 0)
                throw new ArgumentException("Error! Gene count must be a positive value.");
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
        public BinaryChromosomeMetadata(string name, uint geneCount, double crossoverRate, double mutationRate,
            BinaryCrossover crossoverOptions, BinaryMutation mutationOptions)
            : base(name, geneCount, crossoverRate, mutationRate)
        {
            Type = ChromosomeType.Binary;

            _crossovers = GeneticFactory.ConstructBinaryCrossoverOperators(crossoverOptions);
            _mutations = GeneticFactory.ConstructBinaryMutationOperators(mutationOptions);

            _crossoverF = ComputeWeightedSum(_crossovers);
            _mutationF = ComputeWeightedSum(_mutations);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BinaryChromosomeMetadata"/> class.
        /// </summary>
        /// <remarks>Python friendly constructor.</remarks>
        /// <param name="name">The chromosome name.</param>
        /// <param name="geneCount">The gene count.</param>
        /// <param name="crossoverRate">The crossover rate.</param>
        /// <param name="mutationRate">The mutation rate.</param>
        /// <param name="crossoverOptions">The crossover options.</param>
        /// <param name="mutationOptions">The mutation options.</param>
        public BinaryChromosomeMetadata(string name, int geneCount, ChromosomeType type, double crossoverRate, double mutationRate,
            BinaryCrossover crossoverOptions, BinaryMutation mutationOptions)
            : this(name, (uint)geneCount, crossoverRate, mutationRate, crossoverOptions, mutationOptions)
        {
            if (geneCount <= 0)
                throw new ArgumentException("Error! Gene count must be a positive value.");
        }

        /// <summary>
        /// Adds new crossover operators to the metadata.
        /// </summary>
        /// <param name="crossoverOptions">The crossover options.</param>
        /// <param name="weight">The operator weight.</param>
        public void AddCrossoverOperators(BinaryCrossover crossoverOptions, uint weight = 1)
        {
            var crossovers = GeneticFactory.ConstructBinaryCrossoverOperators(crossoverOptions, weight);

            if (_crossovers == null)
                _crossovers = crossovers;
            else
                _crossovers.AddRange(crossovers);

            _crossoverF += ComputeWeightedSum(crossovers);
        }

        /// <summary>
        /// Adds new mutation operators to the metadata.
        /// </summary>
        /// <param name="mutationOptions">The mutation options.</param>
        /// <param name="weight">The operator weight.</param>
        public void AddMutationOperators(BinaryMutation mutationOptions, uint weight = 1)
        {
            var mutations = GeneticFactory.ConstructBinaryMutationOperators(mutationOptions, weight);

            if (_mutations == null)
                _mutations = mutations;
            else
                _mutations.AddRange(mutations);

            _mutationF += ComputeWeightedSum(mutations);
        }

        /// <summary>
        /// Gets the default crossover operators.
        /// </summary>
        /// <returns>The default crossover operators.</returns>
        protected override List<ICrossover> GetDefaultCrossovers()
        {
            return GeneticFactory.ConstructBinaryCrossoverOperators(DEFAULT_CROSSOVER_OPTIONS);
        }

        /// <summary>
        /// Gets the default mutation operators.
        /// </summary>
        /// <returns>The default mutation operators.</returns>
        protected override List<IMutation> GetDefaultMutations()
        {
            return GeneticFactory.ConstructBinaryMutationOperators(DEFAULT_MUTATION_OPTIONS);
        }
    }

    public class PermutationChromosomeMetadata : ChromosomeMetadata
    {
        private const PermutationCrossover DEFAULT_CROSSOVER_OPTIONS = PermutationCrossover.Order;
        private const PermutationMutation DEFAULT_MUTATION_OPTIONS = PermutationMutation.Transposition;

        /// <summary>
        /// Constructs a new instance of the <see cref="PermutationChromosomeMetadata"/> class.
        /// </summary>
        /// <param name="name">The chromosome name.</param>
        /// <param name="geneCount">The gene count.</param>
        /// <param name="crossoverRate">The crossover rate.</param>
        /// <param name="mutationRate">The mutation rate.</param>
        public PermutationChromosomeMetadata(string name, uint geneCount, double crossoverRate = 1, double mutationRate = 0.25)
            : base(name, geneCount, crossoverRate, mutationRate)
        {
            Type = ChromosomeType.Permutation;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="PermutationChromosomeMetadata"/> class.
        /// </summary>
        /// <remarks>Python friendly constructor.</remarks>
        /// <param name="name">The chromosome name.</param>
        /// <param name="geneCount">The gene count.</param>
        /// <param name="crossoverRate">The crossover rate.</param>
        /// <param name="mutationRate">The mutation rate.</param>
        public PermutationChromosomeMetadata(string name, int geneCount, double crossoverRate = 1, double mutationRate = 0.25)
            : this(name, (uint)geneCount, crossoverRate, mutationRate)
        {
            if (geneCount <= 0)
                throw new ArgumentException("Error! Gene count must be a positive value.");
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
        public PermutationChromosomeMetadata(string name, uint geneCount, double crossoverRate, double mutationRate,
            PermutationCrossover crossoverOptions, PermutationMutation mutationOptions)
            : base(name, geneCount, crossoverRate, mutationRate)
        {
            Type = ChromosomeType.Permutation;

            _crossovers = GeneticFactory.ConstructPermutationCrossoverOperators(crossoverOptions);
            _mutations = GeneticFactory.ConstructPermutationMutationOperators(mutationOptions);

            _crossoverF = ComputeWeightedSum(_crossovers);
            _mutationF = ComputeWeightedSum(_mutations);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="PermutationChromosomeMetadata"/> class.
        /// </summary>
        /// <remarks>Python friendly constructor.</remarks>
        /// <param name="name">The chromosome name.</param>
        /// <param name="geneCount">The gene count.</param>
        /// <param name="crossoverRate">The crossover rate.</param>
        /// <param name="mutationRate">The mutation rate.</param>
        /// <param name="crossoverOptions">The crossover options.</param>
        /// <param name="mutationOptions">The mutation options.</param>
        public PermutationChromosomeMetadata(string name, int geneCount, double crossoverRate, double mutationRate,
            PermutationCrossover crossoverOptions, PermutationMutation mutationOptions)
            : this(name, (uint)geneCount, crossoverRate, mutationRate, crossoverOptions, mutationOptions)
        {
            if (geneCount <= 0)
                throw new ArgumentException("Error! Gene count must be a positive value.");
        }

        /// <summary>
        /// Adds new crossover operators to the metadata.
        /// </summary>
        /// <param name="crossoverOptions">The crossover options.</param>
        /// <param name="weight">The operator weight.</param>
        public void AddCrossoverOperators(PermutationCrossover crossoverOptions, uint weight = 1)
        {
            var crossovers = GeneticFactory.ConstructPermutationCrossoverOperators(crossoverOptions, weight);

            if (_crossovers == null)
                _crossovers = crossovers;
            else
                _crossovers.AddRange(crossovers);

            _crossoverF += ComputeWeightedSum(crossovers);
        }

        /// <summary>
        /// Adds new mutation operators to the metadata.
        /// </summary>
        /// <param name="mutationOptions">The mutation options.</param>
        /// <param name="weight">The operator weight.</param>
        public void AddMutationOperators(PermutationMutation mutationOptions, uint weight = 1)
        {
            var mutations = GeneticFactory.ConstructPermutationMutationOperators(mutationOptions, weight);

            if (_mutations == null)
                _mutations = mutations;
            else
                _mutations.AddRange(mutations);

            _mutationF += ComputeWeightedSum(mutations);
        }

        /// <summary>
        /// Gets the default crossover operators.
        /// </summary>
        /// <returns>The default crossover operators.</returns>
        protected override List<ICrossover> GetDefaultCrossovers()
        {
            return GeneticFactory.ConstructPermutationCrossoverOperators(DEFAULT_CROSSOVER_OPTIONS);
        }

        /// <summary>
        /// Gets the default mutation operators.
        /// </summary>
        /// <returns>The default mutation operators.</returns>
        protected override List<IMutation> GetDefaultMutations()
        {
            return GeneticFactory.ConstructPermutationMutationOperators(DEFAULT_MUTATION_OPTIONS);
        }
    }
}
