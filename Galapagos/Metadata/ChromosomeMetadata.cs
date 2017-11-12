using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.CrossoverOperators;
using Galapagos.MutationOperators;
using Galapagos.API;
using Galapagos.API.Factory;

namespace Galapagos.Metadata
{
    /// <summary>
    /// Metadata for a creature chromosome.
    /// </summary>
    public abstract class ChromosomeMetadata : IChromosomeMetadata
    {
        private const string DEFAULT_NAME = "[NULL]";
        private const uint DEFAULT_GENE_COUNT = 0;
        private const double DEFAULT_CROSSOVER_RATE = 1;
        private const double DEFAULT_MUTATION_RATE = 0.25;

        protected List<ICrossover> _crossovers;
        protected List<IMutation> _mutations;

        protected double _crossoverF = 0;
        protected double _mutationF = 0;

        /// <summary>
        /// Constructs a new instance of the <see cref="ChromosomeMetadata"/> class.
        /// </summary>
        protected ChromosomeMetadata()
        {
            Name = DEFAULT_NAME;
            GeneCount = DEFAULT_GENE_COUNT;
            CrossoverRate = DEFAULT_CROSSOVER_RATE;
            MutationRate = DEFAULT_MUTATION_RATE;
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
        public ICrossover GetCrossover()
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
        public IMutation GetMutation()
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
        internal BinaryChromosomeMetadata()
            : base()
        {
            Type = ChromosomeType.Binary;
        }

        /// <summary>
        /// Adds new crossover operators to the metadata.
        /// </summary>
        /// <param name="crossoverOptions">The crossover options.</param>
        /// <param name="weight">The operator weight.</param>
        internal void AddCrossoverOperators(BinaryCrossover crossoverOptions, uint weight = 1)
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
        internal void AddMutationOperators(BinaryMutation mutationOptions, uint weight = 1)
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
        internal PermutationChromosomeMetadata()
            : base()
        {
            Type = ChromosomeType.Permutation;
        }

        /// <summary>
        /// Adds new crossover operators to the metadata.
        /// </summary>
        /// <param name="crossoverOptions">The crossover options.</param>
        /// <param name="weight">The operator weight.</param>
        internal void AddCrossoverOperators(PermutationCrossover crossoverOptions, uint weight = 1)
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
        internal void AddMutationOperators(PermutationMutation mutationOptions, uint weight = 1)
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
