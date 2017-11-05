using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;
using Galapagos.CrossoverOperators;
using Galapagos.CrossoverOperators.Binary;
using Galapagos.CrossoverOperators.Permutation;
using Galapagos.MutationOperators;
using Galapagos.MutationOperators.Binary;
using Galapagos.MutationOperators.Permutation;
using Galapagos.SelectionAlgorithms;
using Galapagos.TerminationConditions;
using Galapagos.API;

namespace Galapagos.API.Factory
{
    /// <summary>
    /// Utility class for creating pieces of a genetic algorithm.
    /// </summary>
    public static class GeneticFactory
    {
        /// <summary>
        /// Constructs a list of binary crossovers operators.
        /// </summary>
        /// <param name="crossoverOptions">The binary crossover options.</param>
        /// <param name="weight">The crossover weight.</param>
        /// <returns>The constructed binary crossovers operators.</returns>
        public static List<ICrossover> ConstructBinaryCrossoverOperators(BinaryCrossover crossoverOptions, uint weight = 1)
        {
            var crossovers = new List<ICrossover>();

            if ((crossoverOptions & BinaryCrossover.SinglePoint) == BinaryCrossover.SinglePoint) crossovers.Add(new SinglePointCrossover(weight));
            if ((crossoverOptions & BinaryCrossover.TwoPoint) == BinaryCrossover.TwoPoint) crossovers.Add(new TwoPointCrossover(weight));
            if ((crossoverOptions & BinaryCrossover.Uniform) == BinaryCrossover.Uniform) crossovers.Add(new UniformCrossover(weight));
            if ((crossoverOptions & BinaryCrossover.NoOp) == BinaryCrossover.NoOp) crossovers.Add(new NoOpBinaryCrossover(weight));
            if (crossovers.Count == 0) throw new ArgumentException("Error! Invalid binary crossover selection.");

            return crossovers;
        }

        /// <summary>
        /// Constructs a list of permutation crossovers operators.
        /// </summary>
        /// <param name="crossoverOptions">The permutation crossover options.</param>
        /// <param name="weight">The crossover weight.</param>
        /// <returns>The constructed permutation crossovers operators.</returns>
        public static List<ICrossover> ConstructPermutationCrossoverOperators(PermutationCrossover crossoverOptions, uint weight = 1)
        {
            var crossovers = new List<ICrossover>();

            if ((crossoverOptions & PermutationCrossover.AlternatingPosition) == PermutationCrossover.AlternatingPosition) crossovers.Add(new AlternatingPositionCrossover(weight));
            if ((crossoverOptions & PermutationCrossover.Order) == PermutationCrossover.Order) crossovers.Add(new OrderCrossover(weight));
            if ((crossoverOptions & PermutationCrossover.Midpoint) == PermutationCrossover.Midpoint) crossovers.Add(new MidpointCrossover(weight));
            if ((crossoverOptions & PermutationCrossover.NoOp) == PermutationCrossover.NoOp) crossovers.Add(new NoOpPermutationCrossover(weight));
            if (crossovers.Count == 0) throw new ArgumentException("Error! Invalid permutation selection.");

            return crossovers;
        }

        /// <summary>
        /// Constructs a list of binary mutations operators.
        /// </summary>
        /// <param name="mutationOptions">The binary mutation options.</param>
        /// <param name="weight">The mutation weight.</param>
        /// <returns>The constructed binary mutations operators.</returns>
        public static List<IMutation> ConstructBinaryMutationOperators(BinaryMutation mutationOptions, uint weight = 1)
        {
            var mutations = new List<IMutation>();

            if ((mutationOptions & BinaryMutation.CyclicShift) == BinaryMutation.CyclicShift) mutations.Add(new CyclicShiftBinaryMutation(weight));
            if ((mutationOptions & BinaryMutation.Randomization) == BinaryMutation.Randomization) mutations.Add(new RandomizationBinaryMutation(weight));
            if ((mutationOptions & BinaryMutation.Reverse) == BinaryMutation.Reverse) mutations.Add(new ReverseBinaryMutation(weight));
            if ((mutationOptions & BinaryMutation.FlipBit) == BinaryMutation.FlipBit) mutations.Add(new FlipBitMutation(weight));
            if ((mutationOptions & BinaryMutation.SingleBit) == BinaryMutation.SingleBit) mutations.Add(new SingleBitMutation(weight));
            if (mutations.Count == 0) throw new ArgumentException("Error! Invalid binary mutation selection.");

            return mutations;
        }

        /// <summary>
        /// Constructs a list of permutation mutations operators.
        /// </summary>
        /// <param name="mutationOptions">The permutation mutation options.</param>
        /// <param name="weight">The mutation weight.</param>
        /// <returns>The constructed permutation mutations operators.</returns>
        public static List<IMutation> ConstructPermutationMutationOperators(PermutationMutation mutationOptions, uint weight = 1)
        {
            var mutations = new List<IMutation>();

            if ((mutationOptions & PermutationMutation.CyclicShift) == PermutationMutation.CyclicShift) mutations.Add(new CyclicShiftPermutationMutation(weight));
            if ((mutationOptions & PermutationMutation.Randomization) == PermutationMutation.Randomization) mutations.Add(new RandomizationPermutationMutation(weight));
            if ((mutationOptions & PermutationMutation.Reverse) == PermutationMutation.Reverse) mutations.Add(new ReversePermutationMutation(weight));
            if ((mutationOptions & PermutationMutation.Transposition) == PermutationMutation.Transposition) mutations.Add(new TranspositionMutation(weight));
            if (mutations.Count == 0) throw new ArgumentException("Error! Invalid permutation mutation selection.");

            return mutations;
        }

        /// <summary>
        /// Constructs a selection algorithm.
        /// </summary>
        /// <param name="creatures">The creature population.</param>
        /// <param name="algorithm">The selection algorithm to construct.</param>
        /// <param name="param">The selection algorithm parameter.</param>
        /// <returns>The selection algorithm.</returns>
        public static ISelectionAlgorithm ConstructSelectionAlgorithm(SelectionAlgorithm algorithm, object param = null)
        {
            try
            {
                switch (algorithm)
                {
                    case SelectionAlgorithm.FitnessProportionate:
                        return new FitnessProportionateSelection();
                    case SelectionAlgorithm.StochasticUniversalSampling:
                        uint? n = param == null ? default(uint?) : Convert.ToUInt32(param);
                        return new StochasticUniversalSampling(n);
                    case SelectionAlgorithm.Tournament:
                        uint? k = param == null ? default(uint?) : Convert.ToUInt32(param);
                        return new TournamentSelection(k);
                    case SelectionAlgorithm.Truncation:
                        double? truncationRate = param == null ? default(double?) : Convert.ToDouble(param);
                        return new TruncationSelection(truncationRate);
                    default:
                        throw new ArgumentException("Error! Invalid algorithm selection.");
                }
            }
            catch
            {
                throw new ArgumentException("Error! Invalid constructor argument.");
            }
        }

        /// <summary>
        /// Constructs a termination condition.
        /// </summary>
        /// <param name="population">The creature population.</param>
        /// <param name="condition">The termination condition to construct.</param>
        /// <param name="param">The termination condition parameter.</param>
        /// <returns>The constructed termination condition.</returns>
        public static ITerminationCondition ConstructTerminationCondition(TerminationCondition condition, object param = null)
        {
            try
            {
                switch (condition)
                {
                    case TerminationCondition.FitnessPlateau:
                        var plateauLength = Convert.ToUInt32(param);
                        return new FitnessPlateau(plateauLength);
                    case TerminationCondition.FitnessThreshold:
                        var fitnessThreshold = Convert.ToUInt32(param);
                        return new FitnessThreshold(fitnessThreshold);
                    case TerminationCondition.GenerationThreshold:
                        var generationThreshold = Convert.ToUInt32(param);
                        return new GenerationThreshold(generationThreshold);
                    case TerminationCondition.Timer:
                        var stopTime = (TimeSpan)param;
                        return new Timer(stopTime);
                    default:
                        throw new ArgumentException("Error! Invalid termination condition.");
                }
            }
            catch
            {
                throw new ArgumentException("Error! Invalid constructor argument.");
            }
        }

        /// <summary>
        /// Constructs a chromosome from metadata.
        /// </summary>
        /// <param name="chromosomeMetadata">The chromosome metadata.</param>
        /// <returns>The chromosome.</returns>
        public static IChromosome ConstructChromosome(IChromosomeMetadata chromosomeMetadata)
        {
            switch (chromosomeMetadata.Type)
            {
                case ChromosomeType.Binary:
                    return new BinaryChromosome(chromosomeMetadata.GeneCount);
                case ChromosomeType.Permutation:
                    return new PermutationChromosome(chromosomeMetadata.GeneCount);
                default:
                    throw new ArgumentException($"Error! Invalid chromosome type.");
            }
        }

        /// <summary>
        /// Constructs a chromosome from metadata.
        /// </summary>
        /// <typeparam name="T">The array data type.</typeparam>
        /// <param name="chromosomeMetadata">The chromosome metadata.</param>
        /// <param name="genes">The chromosome genes genes.</param>
        /// <returns>The chromosome.</returns>
        public static IChromosome ConstructChromosome<T>(IChromosomeMetadata chromosomeMetadata, T[] genes)
        {
            switch(chromosomeMetadata.Type)
            {
                case ChromosomeType.Binary:
                    return new BinaryChromosome(genes as bool[]);
                case ChromosomeType.Permutation:
                    return new PermutationChromosome(genes as uint[]);
                default:
                    throw new ArgumentException($"Error! Invalid chromosome type.");
            }
        }
    }
}
