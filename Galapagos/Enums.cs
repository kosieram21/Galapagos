using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos
{ 
    /// <summary>
    /// The available binary crossover operators.
    /// </summary>
    [Flags]
    public enum BinaryCrossover
    {
        SinglePoint = 1,
        NoOp = 1 << 1,
        All = SinglePoint | NoOp
    }

    /// <summary>
    /// The available permutation crossover operators.
    /// </summary>
    [Flags]
    public enum PermutationCrossover
    {
        AlternatingPosition = 1,
        Order = 1 << 1,
        NoOp = 1 << 2,
        All = AlternatingPosition | Order | NoOp
    }

    /// <summary>
    /// The available binary mutation operators.
    /// </summary>
    [Flags]
    public enum BinaryMutation
    {
        Boundary = 1,
        CyclicShift = 1 << 1,
        FlipBit = 1 << 2,
        SingleBit = 1 << 3,
        All = Boundary | CyclicShift | FlipBit | SingleBit
    }

    /// <summary>
    /// The available permutation mutation operators.
    /// </summary>
    [Flags]
    public enum PermutationMutation
    {
        CyclicShift = 1,
        Randomization = 1 << 1,
        Reverse = 1 << 2,
        Transposition = 1 << 3,
        All = CyclicShift | Randomization | Reverse | Transposition
    }

    /// <summary>
    /// The available selection algorithms.
    /// </summary>
    public enum SelectionAlgorithm
    {
        FitnessProportionate,
        StochasticUniversalSampling,
        Tournament,
        Truncation
    }

    /// <summary>
    /// The available termination conditions.
    /// </summary>
    public enum TerminationCondition
    {
        FitnessPlateau,
        FitnessThreshold,
        GenerationThreshold,
        Timer
    }
}
