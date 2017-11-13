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

        private Creature _optimalCreature;
        private readonly Creature[] _creatures;

        private IList<Niche> _niches = new List<Niche>();

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

            _creatures = new Creature[Population.Metadata.Size];
            for (var i = 0; i < _creatures.Count(); i++)
                _creatures[i] = new Creature(this);
            _optimalCreature = _creatures[0];
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
        public ICreature OptimalCreature => InternalOptimalCreature as ICreature;

        /// <summary>
        /// Internal optimal creature getter/setter.
        /// </summary>
        internal Creature InternalOptimalCreature
        {
            get
            {
                if (_optimalCreature == null)
                    _optimalCreature = FindOptimalCreature();
                return _optimalCreature;
            }
            private set
            {
                if (_optimalCreature == null || value.Fitness > _optimalCreature.Fitness)
                    _optimalCreature = value;
            }
        }

        /// <summary>
        /// Finds the optimal creature.
        /// </summary>
        private Creature FindOptimalCreature()
        {
            var optimalCreature = _creatures[0];

            for (var i = 1; i < Size; i++)
            {
                if (_creatures[i].Fitness > optimalCreature.Fitness)
                    optimalCreature = _creatures[i];
            }

            return optimalCreature;
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
            Action evaluateFitness = () => { foreach (var creature in _creatures) { creature.EvaluateFitness(); } };
            RunEvolution(evaluateFitness);
        }

        /// <summary>
        /// Optimizes the population. Evaluates creature fitness values in parallel.
        /// </summary>
        public void ParallelEvolve()
        {
            Action evaluateFitness = () => { Parallel.ForEach(_creatures, (creature) => { creature.EvaluateFitness(); }); };
            RunEvolution(evaluateFitness);
        }

        /// <summary>
        /// Runs the evolution process.
        /// </summary>
        /// <param name="evaluateFitness">A delegate that evaluates creature fitness.</param>
        private void RunEvolution(Action evaluateFitness)
        {
            evaluateFitness();
            BreedNewGeneration();

            _optimalCreature = FindOptimalCreature();
        }

        /// <summary>
        /// Breeds a new generation of creatures.
        /// </summary>
        /// <param name="selection">The selection algorithm.</param>
        private void BreedNewGeneration()
        {
            var newGeneration = new Creature[Size];

            Population.Metadata.SelectionAlgorithm.Initialize(this);

            var i = 0;
            if (Population.Metadata.SurvivalRate > 0)
            {
                var sortedCreatures = _creatures.OrderByDescending(creature => creature.Fitness).ToArray();
                for (var j = 0; j < Size * Population.Metadata.SurvivalRate; j++)
                {
                    newGeneration[j] = sortedCreatures[j];
                    i++;
                }
            }

            while (i < Size)
            {
                var parentX = Population.Metadata.SelectionAlgorithm.Invoke();
                var parentY = Population.Metadata.SelectionAlgorithm.Invoke();
                newGeneration[i] = ((Creature)parentX).Breed((Creature)parentY);
                i++;
            }

            Array.Copy(newGeneration, _creatures, Size);

            if (Population.Metadata.DistanceThreshold > 0)
            {
                ClearNiches();
                AssignNiches();
            }
        }

        /// <summary>
        /// Assigns each creature in the population to a niche.
        /// </summary>
        private void AssignNiches()
        {
            foreach (var creature in _creatures)
            {
                var candidate = _niches.FirstOrDefault(niche => niche.Compatible(creature));
                if (candidate != null)
                    candidate.Add(creature);
                else
                    _niches.Add(new Niche(creature, Population.Metadata.DistanceThreshold));
            }

            var activeNiches = new List<Niche>();
            foreach (var niche in _niches)
            {
                if (niche.Size > 0)
                    activeNiches.Add(niche);
            }

            _niches = activeNiches;
        }

        /// <summary>
        /// Clears all niches of creatures.
        /// </summary>
        private void ClearNiches()
        {
            foreach (var niche in _niches)
                niche.Clear();
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
