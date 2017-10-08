using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.CrossoverOperators;
using Galapagos.CrossoverOperators.Binary;
using Galapagos.CrossoverOperators.Permutation;
using Galapagos.MutationOperators;
using Galapagos.MutationOperators.Binary;
using Galapagos.MutationOperators.Permutation;
using Galapagos.SelectionAlgorithms;
using Galapagos.TerminationConditions;

namespace Galapagos
{
    /// <summary>
    /// Utility class for creating pieces of a genetic algorithm.
    /// </summary>
    internal static class GeneticFactory
    {
        /// <summary>
        /// Constructs a list of binary crossovers operators.
        /// </summary>
        /// <param name="crossoverOptions">The binary crossover options.</param>
        /// <param name="weight">The crossover weight.</param>
        /// <returns>The constructed binary crossovers operators.</returns>
        internal static List<ICrossover> ConstructBinaryCrossoverOperators(BinaryCrossover crossoverOptions, uint weight = 1)
        {
            var crossovers = new List<ICrossover>();

            if ((crossoverOptions & BinaryCrossover.SinglePoint) == BinaryCrossover.SinglePoint) crossovers.Add(new SinglePointCrossover(weight));
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
        internal static List<ICrossover> ConstructPermutationCrossoverOperators(PermutationCrossover crossoverOptions, uint weight = 1)
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
        internal static List<IMutation> ConstructBinaryMutationOperators(BinaryMutation mutationOptions, uint weight = 1)
        {
            var mutations = new List<IMutation>();

            if ((mutationOptions & BinaryMutation.Boundary) == BinaryMutation.Boundary) mutations.Add(new BoundaryMutation(weight));
            if ((mutationOptions & BinaryMutation.CyclicShift) == BinaryMutation.CyclicShift) mutations.Add(new CyclicBitShiftMutation(weight));
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
        internal static List<IMutation> ConstructPermutationMutationOperators(PermutationMutation mutationOptions, uint weight = 1)
        {
            var mutations = new List<IMutation>();

            if ((mutationOptions & PermutationMutation.CyclicShift) == PermutationMutation.CyclicShift) mutations.Add(new CyclicShiftMutation(weight));
            if ((mutationOptions & PermutationMutation.Randomization) == PermutationMutation.Randomization) mutations.Add(new RandomizationMutation(weight));
            if ((mutationOptions & PermutationMutation.Reverse) == PermutationMutation.Reverse) mutations.Add(new ReverseMutation(weight));
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
        internal static ISelectionAlgorithm ConstructSelectionAlgorithm(Creature[] creatures, SelectionAlgorithm algorithm, object param)
        {
            try
            {
                switch (algorithm)
                {
                    case SelectionAlgorithm.FitnessProportionate:
                        return new FitnessProportionateSelection(creatures);
                    case SelectionAlgorithm.StochasticUniversalSampling:
                        //if (param != null) ValidateParameterType(typeof(int), param?.GetType());
                        int? n = param == null ? default(int?) : Convert.ToInt32(param);
                        return new StochasticUniversalSampling(creatures, n);
                    case SelectionAlgorithm.Tournament:
                        //if (param != null) ValidateParameterType(typeof(int), param?.GetType());
                        int? k = param == null ? default(int?) : Convert.ToInt32(param);
                        return new TournamentSelection(creatures, k);
                    case SelectionAlgorithm.Truncation:
                        //if(param != null) ValidateParameterType(typeof(double), param?.GetType());
                        double? truncationRate = param == null ? default(double?) : Convert.ToDouble(param);
                        return new TruncationSelection(creatures, truncationRate);
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
        internal static ITerminationCondition ConstructTerminationCondition(Population population, TerminationCondition condition, object param)
        {
            try
            {
                switch (condition)
                {
                    case TerminationCondition.FitnessPlateau:
                        //ValidateParameterType(typeof(int), param?.GetType());
                        var plateauLength = Convert.ToInt32(param);
                        return new FitnessPlateau(population, plateauLength);
                    case TerminationCondition.FitnessThreshold:
                        //ValidateParameterType(typeof(uint), param?.GetType());
                        var fitnessThreshold = Convert.ToUInt32(param);
                        return new FitnessThreshold(population, fitnessThreshold);
                    case TerminationCondition.GenerationThreshold:
                        //ValidateParameterType(typeof(int), param?.GetType());
                        var generationThreshold = Convert.ToInt32(param);
                        return new GenerationThreshold(population, generationThreshold);
                    case TerminationCondition.Timer:
                        //ValidateParameterType(typeof(TimeSpan), param?.GetType());
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
        internal static IChromosome ConstructChromosome(ChromosomeMetadata chromosomeMetadata)
        {
            switch(chromosomeMetadata.Type)
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
        /// Validates a given parameter is of the correct type.
        /// </summary>
        /// <param name="expected">The expected type.</param>
        /// <param name="received">The recieved type.</param>
        private static void ValidateParameterType(Type expected, Type received)
        {
            if(expected != received) throw new ArgumentException($"Error! Expected an {expected} but received a {received}");
        }
    }
}
