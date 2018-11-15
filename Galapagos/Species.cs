using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Galapagos.API;

namespace Galapagos
{
    /// <summary>
    /// A subpopulation of creatures evolving one dimension of a optimization problem.
    /// </summary>
    internal class Species : IPopulation
    {
        private readonly Population _population;

        private readonly string _chromosomeFilter;

        private Creature[] _creatures;

        private Group _optimalGroup;
        private readonly Group[] _groups;

        /// <summary>
        /// Constructs a new instance of the <see cref="Species"/> class.
        /// </summary>
        /// <param name="population">The population this species belongs to.</param>
        /// <param name="chromosomeFilter">
        /// A filter that determines what chromosome this species is responsible for. 
        /// The * character means the species is responsible for all chromosomes.
        /// </param>
        internal Species(Population population, string chromosomeFilter = "*")
        {
            _population = population;

            _chromosomeFilter = chromosomeFilter;

            _groups = new Group[1];
            _groups[0] = new Group(this, Population.Metadata.Size);

            _optimalGroup = _groups[0];

            _creatures = _groups[0].Creatures;
        }

        /// <summary>
        /// Gets the population this creature belongs to.
        /// </summary>
        internal Population Population => _population;

        /// <summary>
        /// Gets the chromosome filter for this species.
        /// </summary>
        internal string ChromosomeFilter => _chromosomeFilter;

        /// <summary>
        /// Gets the population size.
        /// </summary>
        public uint Size => Population.Metadata.Size;

        /// <summary>
        /// Accesses a creature from the population.
        /// </summary>
        /// <param name="index">The creature index.</param>
        /// <returns>The creature.</returns>
        public ICreature this[int index]
        {
            get
            {
                if (index >= Size)
                    throw new Exception($"Error! {index} is larger than the population size.");
                return _creatures[index] as ICreature;
            }
        }

        /// <summary>
        /// Gets or sets the creature best suited to solve the propblem.
        /// </summary>
        public ICreature OptimalCreature => _optimalGroup.OptimalCreature;

        /// <summary>
        /// Internal optimal creature getter/setter.
        /// </summary>
        internal Creature InternalOptimalCreature => _optimalGroup.InternalOptimalCreature;

        /// <summary>
        /// Finds the optimal group.
        /// </summary>
        private Group FindOptimalGroup()
        {
            var optimalGroup = _groups[0];

            for (var i = 1; i < _groups.Count(); i++)
            {
                if (_groups[i].OptimalCreature.Fitness > optimalGroup.OptimalCreature.Fitness)
                    optimalGroup = _groups[i];
            }

            return optimalGroup;
        }

        /// <summary>
        /// Enables fitness logging.
        /// </summary>
        /// <param name="path">The path to log data to.</param>
        public void EnableLogging(string path = "")
        {
            Population.EnableLogging(path);
        }

        /// <summary>
        /// Disables fitness logging.
        /// </summary>
        public void DisableLogging()
        {
            Population.DisableLogging();
        }

        /// <summary>
        /// Optimizes the population.
        /// </summary>
        public void Evolve()
        {
            Action evolveGroups = () => { foreach (var group in _groups) { group.Evolve(); } };
            RunEvolution(evolveGroups);
        }

        /// <summary>
        /// Optimizes the population. Evaluates creature fitness values in parallel.
        /// </summary>
        public void ParallelEvolve()
        {
            Action evolveGroups = () => { Parallel.ForEach(_groups, (group) => { group.ParallelEvolve(); }); };
            RunEvolution(evolveGroups);
        }

        /// <summary>
        /// Runs the evolution process.
        /// </summary>
        /// <param name="evolveGroups">A delegate that evolves the group subpopulations.</param>
        private void RunEvolution(Action evolveGroups)
        {
            evolveGroups();

            _optimalGroup = FindOptimalGroup();
            _creatures = _groups[0].Creatures;
        }

        #region IEnumerable Members

        public IEnumerator<ICreature> GetEnumerator()
        {
            return _creatures.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
