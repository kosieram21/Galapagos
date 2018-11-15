using Galapagos.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Galapagos
{
    /// <summary>
    /// A group of createres within a population who only breed with each other.
    /// </summary>
    internal class Group : IPopulation
    {
        private readonly Species _species;

        private Creature _optimalCreature;
        private readonly Creature[] _creatures;

        private IList<Niche> _niches = new List<Niche>();

        /// <summary>
        /// Constructs a new instance of the <see cref="Species"/> class.
        /// </summary>
        /// <param name="species">The species this group belongs to.</param>
        /// <param name="size">The number of creatures in the group.</param>
        internal Group(Species species, uint size)
        {
            _species = species;
            
            _creatures = new Creature[size];
            for (var i = 0; i < size; i++)
                _creatures[i] = new Creature(this);
            _optimalCreature = _creatures[0];
        }

        /// <summary>
        /// Gets the species this group belongs to.
        /// </summary>
        internal Species Species => _species;

        /// <summary>
        /// Gets the population size.
        /// </summary>
        public uint Size => (uint)_creatures.Length;

        /// <summary>
        /// Gets the niches in this species.
        /// </summary>
        internal IReadOnlyList<Niche> Niches => _niches as IReadOnlyList<Niche>;

        /// <summary>
        /// Gets the creatures in this group.
        /// </summary>
        internal Creature[] Creatures => _creatures;

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
            Species.EnableLogging(path);
        }

        /// <summary>
        /// Disables fitness logging.
        /// </summary>
        public void DisableLogging()
        {
            Species.DisableLogging();
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

            Species.Population.Metadata.SelectionAlgorithm.Initialize(this);

            var i = 0;
            if (Species.Population.Metadata.SurvivalRate > 0)
            {
                var sortedCreatures = _creatures.OrderByDescending(creature => creature.Fitness).ToArray();
                for (var j = 0; j < Size * Species.Population.Metadata.SurvivalRate; j++)
                {
                    newGeneration[j] = sortedCreatures[j];
                    i++;
                }
            }

            while (i < Size)
            {
                var parentX = Species.Population.Metadata.SelectionAlgorithm.Invoke();
                var parentY = Species.Population.Metadata.SelectionAlgorithm.Invoke();
                newGeneration[i] = ((Creature)parentX).Breed((Creature)parentY);
                i++;
            }

            Array.Copy(newGeneration, _creatures, Size);

            if (Species.Population.Metadata.DistanceThreshold > 0)
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
                    _niches.Add(new Niche(creature, Species.Population.Metadata.DistanceThreshold));
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
