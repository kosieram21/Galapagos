﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.API
{
    public interface IPopulationMetadata : IEnumerable<IChromosomeMetadata>
    {
        /// <summary>
        /// Gets the population size.
        /// </summary>
        uint Size { get; }

        /// <summary>
        /// Gets the survival rate used in elitism. A survival rate of 0 disables elitism.
        /// </summary>
        double SurvivalRate { get; }

        /// <summary>
        /// Gets the distance threshold for niches. A distance threshold of 0 disable niches.
        /// </summary>
        uint DistanceThreshold { get; }
        
        /// <summary>
        /// Gets the selection algorithm.
        /// </summary>
        ISelectionAlgorithm SelectionAlgorithm { get; }

        /// <summary>
        /// Gets the termination conditions.
        /// </summary>
        IReadOnlyList<ITerminationCondition> TerminationConditions { get; }

        /// <summary>
        /// Gets the fitness function.
        /// </summary>
        Func<ICreature, double> FitnessFunction { get; }

        /// <summary>
        /// Gets or sets chromosome metadata from the metadata.
        /// </summary>
        /// <param name="i">The metadata index.</param>
        /// <returns>The metadata.</returns>
        IChromosomeMetadata this[int i] { get; }
    }
}
