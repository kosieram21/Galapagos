using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos
{
    /// <summary>
    /// A population of creatures.
    /// </summary>
    public class Population : IPopulation
    {
        private readonly IPopulationMetadata _populationMetadata;

        private Creature _optimalCreature;
        private readonly Creature[] _creatures;

        private int _generation = 0;

        private IList<Niche> _niches = new List<Niche>();

        private bool _loggingEnabled = false;
        private DataLogger _logger = null;

        /// <summary>
        /// Constructs a new instance of the <see cref="Population"/> class.
        /// </summary>
        /// <param name="populationMetadata">The population metadata.</param>
        internal Population(IPopulationMetadata populationMetadata)
        {
            _populationMetadata = populationMetadata;

            _creatures = new Creature[_populationMetadata.Size];
            for (var i = 0; i < _creatures.Count(); i++)
                _creatures[i] = new Creature(populationMetadata);
        }

        /// <summary>
        /// Gets the population size.
        /// </summary>
        public int Size => _creatures.Count();

        /// <summary>
        /// Gets the population generation.
        /// </summary>
        internal int Generation => _generation;

        /// <summary>
        /// Gets the population creatures.
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
            if (path != string.Empty) _logger = new DataLogger(path);
            _loggingEnabled = true;
        }

        /// <summary>
        /// Disables fitness logging.
        /// </summary>
        public void DisableLogging()
        {
            if (_logger != null) _logger = null;
            _loggingEnabled = false;
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
            while (true)
            {
                evaluateFitness();
                BreedNewGeneration();

                _generation++;
                _optimalCreature = FindOptimalCreature();

                if (_loggingEnabled)
                {
                    //Temp until better logging infrastructure is established.
                    var msg = $"Generation: {_generation}, Fitness: {InternalOptimalCreature.Fitness}";
                    Console.WriteLine(msg);
                    System.Diagnostics.Debug.WriteLine(msg);
                    if (_logger != null) _logger.Log(_generation, InternalOptimalCreature.Fitness);
                }

                if (_populationMetadata.TerminationConditions.Any(condition => condition.Check(this)))
                {
                    foreach (var creature in _creatures)
                        creature.UnregisterNiche();
                    break;
                }
            }
        }

        /// <summary>
        /// Breeds a new generation of creatures.
        /// </summary>
        /// <param name="selection">The selection algorithm.</param>
        private void BreedNewGeneration()
        {
            var newGeneration = new Creature[Size];

            _populationMetadata.SelectionAlgorithm.Initialize(this);

            var i = 0;
            if(_populationMetadata.SurvivalRate > 0)
            {
                var sortedCreatures = _creatures.OrderByDescending(creature => creature.Fitness).ToArray();
                for (var j = 0; j < Size * _populationMetadata.SurvivalRate; j++)
                {
                    newGeneration[j] = sortedCreatures[j];
                    i++;
                }
            }

            while (i < Size)
            {
                var parentX = _populationMetadata.SelectionAlgorithm.Invoke();
                var parentY = _populationMetadata.SelectionAlgorithm.Invoke();
                newGeneration[i] = ((Creature)parentX).Breed((Creature)parentY);
                i++;
            }

            Array.Copy(newGeneration, _creatures, Size);

            if (_populationMetadata.DistanceThreshold > 0)
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
                    _niches.Add(new Niche(creature, _populationMetadata.DistanceThreshold));
            }

            var activeNiches = new List<Niche>();
            foreach(var niche in _niches)
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
            return ((ICreature[])_creatures).ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
