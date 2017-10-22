using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.API
{
    public interface IPopulation : IEnumerable<ICreature>
    {
        /// <summary>
        /// Gets the population size.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Gets the population generation.
        /// </summary>
        int Generation { get; }

        /// <summary>
        /// Accesses a creature from the population.
        /// </summary>
        /// <param name="index">The creature index.</param>
        /// <returns>The creature.</returns>
        ICreature this[int index] { get; }

        /// <summary>
        /// Gets the creature best suited to solve the propblem.
        /// </summary>
        ICreature OptimalCreature { get; }

        /// <summary>
        /// Registers an evolution termination condition.
        /// </summary>
        /// <param name="condition">The termination condition to set.</param>
        /// <param name="param">The termination condition parameter.</param>
        void RegisterTerminationCondition(TerminationCondition condition, object param);

        /// <summary>
        /// Unregisters an evolution termination condition.
        /// </summary>
        /// <param name="condition">The termination condition to set.</param>
        void UnregisterTerminationCondition(TerminationCondition condition);

        /// <summary>
        /// Enables fitness logging.
        /// </summary>
        /// <param name="path">The path to log data to.</param>
        void EnableLogging(string path = "");

        /// <summary>
        /// Disables fitness logging.
        /// </summary>
        void DisableLogging();

        /// <summary>
        /// Enables niches in the population.
        /// </summary>
        /// <param name="distanceThreshold">The distance threshold for the niches.</param>
        void EnableNiches(int distanceThreshold);

        /// <summary>
        /// Disables niches in the population.
        /// </summary>
        void DisableNiches();

        /// <summary>
        /// Enables elitism in the population.
        /// </summary>
        /// <param name="survivalRate">The percentage of creature to carry on unchanged to the next generation.</param>
        void EnableElitism(double survivalRate = 0.25);

        /// <summary>
        /// Disable elitism in the population.
        /// </summary>
        void DisableElitism();

        /// <summary>
        /// Optimizes the population.
        /// </summary>
        /// <param name="selectionAlgorithm">The selection algorithm to use.</param>
        /// <param name="param">The selection algorithm parameter.</param>
        void Evolve(SelectionAlgorithm selectionAlgorithm, object param = null);

        /// <summary>
        /// Optimizes the population. Evaluates creature fitness values in parallel.
        /// </summary>
        /// <param name="selectionAlgorithm">The selection algorithm to use.</param>
        /// <param name="param">The selection algorithm parameter.</param>
        void ParallelEvolve(SelectionAlgorithm selectionAlgorithm, object param = null);
    }
}
