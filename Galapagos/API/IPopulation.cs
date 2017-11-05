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
        /// Enables fitness logging.
        /// </summary>
        /// <param name="path">The path to log data to.</param>
        void EnableLogging(string path = "");

        /// <summary>
        /// Disables fitness logging.
        /// </summary>
        void DisableLogging();

        /// <summary>
        /// Optimizes the population.
        /// </summary>
        void Evolve();

        /// <summary>
        /// Optimizes the population. Evaluates creature fitness values in parallel.
        /// </summary>
        void ParallelEvolve();
    }
}
