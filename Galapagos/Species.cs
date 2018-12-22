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

            var groupCount = (int)Population.Metadata.GroupCount;

            // GROUP SIZE PATAMETERS
            //   N: The total number of creatures in the species
            //   Q: The quotient when dividing N by number of groups
            //   R: The remainder when dividing N by number of groups
            var N = (int)Population.Metadata.Size;
            var R = 0;
            var Q = Math.DivRem(N, groupCount, out R);

            _groups = new Group[groupCount];
            for(var i = 0; i < groupCount; i++)
            {
                var groupSize = i == (groupCount - 1) ? // is this the last group?
                    Q + R : Q;
                _groups[i] = new Group(this, (uint)groupSize);
            }

            _optimalGroup = _groups[0];
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
                return GetCreature(index) as ICreature;
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
            // within group dynamics
            var iter = (int)Population.Metadata.GroupIter;
            for(var i = 0; i < iter; i++)
                evolveGroups();

            // between group dynamics

            _optimalGroup = FindOptimalGroup();
        }

        /// <summary>
        /// Utility method for indexing creatures as if they where in a single collection.
        /// </summary>
        /// <param name="index">The creature index.</param>
        /// <returns>The creature.</returns>
        private Creature GetCreature(int index)
        {
            var groupCount = (int)Population.Metadata.GroupCount;

            uint count = 0;
            var j = index; // real index into group
            for (var i = 0; i < groupCount; i++)
            {
                var group = _groups[i];

                count = i == (groupCount - 1) ? // is this the last group?
                    Population.Metadata.Size : count + group.Size;

                if (index < count)
                    return group[j] as Creature;

                j = (index - (int)count);
            }

            throw new IndexOutOfRangeException();
        }

        #region IEnumerable Members

        public IEnumerator<ICreature> GetEnumerator()
        {
            var creatures = new List<ICreature>();

            var N = Population.Metadata.Size;
            for (var i = 0; i < N; i++)
                creatures.Add(GetCreature(i));

            return creatures.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
