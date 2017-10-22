﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.API
{ 
    /// <summary>
    /// The available binary crossover operators.
    /// </summary>
    [Flags]
    public enum BinaryCrossover
    {
        SinglePoint = 1,
        TwoPoint = 1 << 1,
        Uniform = 1 << 2,
        NoOp = 1 << 3,
        All = SinglePoint | TwoPoint | Uniform | NoOp
    }

    /// <summary>
    /// The available permutation crossover operators.
    /// </summary>
    [Flags]
    public enum PermutationCrossover
    {
        AlternatingPosition = 1,
        Order = 1 << 1,
        Midpoint = 1 << 2,
        NoOp = 1 << 3,
        All = AlternatingPosition | Order | Midpoint | NoOp
    }

    /// <summary>
    /// The available binary mutation operators.
    /// </summary>
    [Flags]
    public enum BinaryMutation
    {
        CyclicShift = 1,
        Randomization = 1 << 1,
        Reverse = 1 << 2,
        FlipBit = 1 << 3,
        SingleBit = 1 << 4,
        All = CyclicShift | Randomization | Reverse | FlipBit | SingleBit
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

    /// <summary>
    /// The available chromosome types.
    /// </summary>
    public enum ChromosomeType
    {
        Binary,
        Permutation
    }
}