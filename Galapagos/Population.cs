using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;
using Galapagos.Chromosomes;

namespace Galapagos
{
    /// <summary>
    /// A population of creatures.
    /// </summary>
    public class Population : IPopulation
    {
        private readonly IPopulationMetadata _populationMetadata;

        private Species[] _species;
        private Species _optimalSpecies;

        private int _generation = 0;

        private bool _loggingEnabled = false;
        private DataLogger _logger = null;

        /// <summary>
        /// Constructs a new instance of the <see cref="Population"/> class.
        /// </summary>
        /// <param name="populationMetadata">The population metadata.</param>
        internal Population(IPopulationMetadata populationMetadata)
        {
            _populationMetadata = populationMetadata;

            if (_populationMetadata.CooperativeCoevolution)
            {
                _species = new Species[_populationMetadata.Count()];
                for (var i = 0; i < _populationMetadata.Count(); i++)
                    _species[i] = new Species(this, _populationMetadata[i].Name);
            }
            else
            {
                _species = new Species[1];
                _species[0] = new Species(this);
            }

            _optimalSpecies = _species[0];
        }

        /// <summary>
        /// Gets the population metadata.
        /// </summary>
        internal IPopulationMetadata Metadata => _populationMetadata;

        /// <summary>
        /// Gets the population size.
        /// </summary>
        public uint Size => _populationMetadata.Size;

        /// <summary>
        /// Gets the population generation.
        /// </summary>
        internal int Generation => _generation;

        /// <summary>
        /// Accesses a creature from the population.
        /// </summary>
        /// <param name="index">The creature index.</param>
        /// <returns>The creature.</returns>
        public ICreature this[int index] => _optimalSpecies[index];

        /// <summary>
        /// Gets or sets the creature best suited to solve the propblem.
        /// </summary>
        public ICreature OptimalCreature => _optimalSpecies.OptimalCreature;

        /// <summary>
        /// Gets the optimal species for this population.
        /// </summary>
        internal Species OptimalSpecies => _optimalSpecies;

        /// <summary>
        /// Finds the optimal species.
        /// </summary>
        private Species FindOptimalSpecies()
        {
            var optimalSpecies = _species[0];

            for (var i = 1; i < _species.Count(); i++)
            {
                if (_species[i].OptimalCreature.Fitness > optimalSpecies.OptimalCreature.Fitness)
                    optimalSpecies = _species[i];
            }

            return optimalSpecies;
        }

        /// <summary>
        /// Gets the species associated the given chromosome filter.
        /// </summary>
        /// <param name="filter">The chromosome filter.</param>
        /// <returns>The species associated with the given chromosome filter.</returns>
        internal Species GetSpecies(string filter)
        {
            return _species.FirstOrDefault(species => species.ChromosomeFilter == filter);
        }

        /// <summary>
        /// Enables fitness logging.
        /// </summary>
        /// <param name="path">The path to log data to.</param>
        public void EnableLogging(string path = "")
        {
            _logger = new DataLogger(path);
            _loggingEnabled = true;
        }

        /// <summary>
        /// Disables fitness logging.
        /// </summary>
        public void DisableLogging()
        {
            _logger = null;
            _loggingEnabled = false;
        }

        /// <summary>
        /// Optimizes the population.
        /// </summary>
        public void Evolve()
        {
            Action evolveSpecies = () => { foreach (var species in _species) { species.Evolve(); } };
            RunEvolution(evolveSpecies);
        }

        /// <summary>
        /// Optimizes the population. Evaluates creature fitness values in parallel.
        /// </summary>
        public void ParallelEvolve()
        {
            Action evolveSpecies = () => { Parallel.ForEach(_species, (species) => { species.ParallelEvolve(); }); };
            RunEvolution(evolveSpecies);
        }

        /// <summary>
        /// Runs the evolution process.
        /// </summary>
        /// <param name="evolveSpecies">A delegate that evolves the species subpopulations.</param>
        private void RunEvolution(Action evolveSpecies)
        {
            NeuralChromosome.ResetAllInnovationTrackers();

            while (true)
            {
                evolveSpecies();
                _optimalSpecies = FindOptimalSpecies();
                _generation++;

                if (_loggingEnabled)
                    _logger.Log(_generation, OptimalCreature.Fitness);

                if (_populationMetadata.TerminationConditions.Any(condition => condition.Check(this)))
                {
                    foreach (var species in _species)
                        foreach(var creature in species)
                            ((Creature)creature).UnregisterNiche();
                    break;
                }
            }
        }

        #region IEnumerable Members

        public IEnumerator<ICreature> GetEnumerator()
        {
            return _optimalSpecies.ToArray().ToList().GetEnumerator(); 
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
