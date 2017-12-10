using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;
using Galapagos.API.Factory;

namespace Galapagos
{
    /// <summary>
    /// An evolvable solution to an optimization problem.
    /// </summary>
    public class Creature : ICreature
    {
        private readonly Species _species;

        private readonly IList<IChromosomeMetadata> _chromosomeMetadata = new List<IChromosomeMetadata>();
        private readonly IDictionary<string, IChromosome> _chromosomes = new Dictionary<string, IChromosome>();

        private double _fitness = 0;

        private Niche _niche = null;

        /// <summary>
        /// Constructs a new instance of the <see cref="Creature"/> class.
        /// </summary>
        /// <param name="species">The species this creature belongs to.</param>
        internal Creature(Species species)
        {
            _species = species;

            _chromosomeMetadata = _species.Population.Metadata
                .Where(metadata => metadata.Name == _species.ChromosomeFilter || _species.ChromosomeFilter == "*")
                .ToList();

            foreach (var chromosomeMetadata in _chromosomeMetadata)
            {
                if (_chromosomes.ContainsKey(chromosomeMetadata.Name))
                    throw new ArgumentException($"Error! Creature already contain a chromosome named {chromosomeMetadata.Name}");

                var chromosome = GeneticFactory.ConstructChromosome(chromosomeMetadata.Type, (uint)chromosomeMetadata.Properties["GeneCount"]);
                _chromosomes.Add(chromosomeMetadata.Name, chromosome);
                chromosome.Creature = this;
            }
        }

        /// <summary>
        /// Clones the creature.
        /// </summary>
        /// <returns>The cloned creature.</returns>
        internal Creature Clone()
        {
            var clone = new Creature(_species);
            foreach(var chromosomeMetadata in _chromosomeMetadata)
            {
                var chromosome = GetChromosome(chromosomeMetadata.Name);
                clone.SetChromosome(chromosomeMetadata.Name, chromosome);
            }
            return clone;
        }

        /// <summary>
        /// Gets the species this creature belongs to.
        /// </summary>
        internal Species Species => _species;

        /// <summary>
        /// Gets the creature's fitness.
        /// </summary>
        public double Fitness => _niche == null ? TrueFitness : TrueFitness / _niche.Size;

        /// <summary>
        /// Gets the true fitness of the creature.
        /// </summary>
        internal double TrueFitness
        {
            get
            {
                EvaluateFitness();
                return _fitness;
            }
        }

        /// <summary>
        /// Evaluates this creatures fitness.
        /// </summary>
        internal void EvaluateFitness()
        {
            if (_fitness == 0)
                _fitness = _species.Population.Metadata.FitnessFunction(this);
        }

        /// <summary>
        /// Measures the distance between two creatures.
        /// </summary>
        /// <param name="other">The other creature.</param>
        /// <returns>The distance between the creatures.</returns>
        internal double Distance(Creature other)
        {
            double sum = 0;

            foreach(var chromosomeMetadata in _chromosomeMetadata)
            {
                var x = GetChromosome(chromosomeMetadata.Name);
                var y = other.GetChromosome(chromosomeMetadata.Name);

                sum += x.Distance(y);
            }

            return sum;
        }

        /// <summary>
        /// Registers the creature to the given niche.
        /// </summary>
        /// <param name="niche">The niche.</param>
        internal void RegisterNiche(Niche niche)
        {
            _niche = niche;
        }

        /// <summary>
        /// Unregisters the creature from it's niche.
        /// </summary>
        internal void UnregisterNiche()
        {
            _niche = null;
        }

        /// <summary>
        /// Breeds with the given mate creature to create a new creature.
        /// </summary>
        /// <param name="mate">The mate creature.</param>
        /// <returns>The child creature.</returns>
        internal Creature Breed(Creature mate)
        {
            var child = new Creature(_species);
            foreach(var chromosomeMetadata in _chromosomeMetadata)
            {
                var crossover = chromosomeMetadata.GetCrossover();
                var mutation = chromosomeMetadata.GetMutation();

                var newDna = Stochastic.EvaluateProbability(chromosomeMetadata.CrossoverRate) ? 
                    crossover.Invoke(GetChromosome(chromosomeMetadata.Name), mate.GetChromosome(chromosomeMetadata.Name)) : 
                    GetChromosome(chromosomeMetadata.Name);

                newDna = Stochastic.EvaluateProbability(chromosomeMetadata.MutationRate) ?
                    mutation.Invoke(newDna) : newDna;

                child.SetChromosome(chromosomeMetadata.Name, newDna);
            }

            return child;
        }

        /// <summary>
        /// Gets the chromosome corresponding to the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The chromosome.</returns>
        private IChromosome GetChromosome(string name)
        {
            if (!_chromosomes.ContainsKey(name))
                throw new ArgumentException($"Error! {name} is not a valid chromosome.");
            return _chromosomes[name];
        }

        /// <summary>
        /// Sets the chromosome corresponding to the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="chromosome">The new chromosome.</param>
        private void SetChromosome(string name, IChromosome chromosome)
        {
            if (!_chromosomes.ContainsKey(name))
                throw new ArgumentException($"Error! {name} is not a valid chromosome.");
            _chromosomes[name] = chromosome;
        }

        /// <summary>
        /// Gets the chromosome corresponding to the given name.
        /// </summary>
        /// <typeparam name="TChromosomeType">The chromosome type.</typeparam>
        /// <param name="name">The chromosome name.</param>
        /// <returns>The chromosome.</returns>
        public TChromosomeType GetChromosome<TChromosomeType>(string name)
            where TChromosomeType : class, IChromosome
        {
            if (Species.ChromosomeFilter == name || Species.ChromosomeFilter == "*")
            {
                var chromosome = GetChromosome(name);
                if (!(chromosome is TChromosomeType))
                    throw new Exception($"Error! The chromosome {name} is not of type {typeof(TChromosomeType).Name}.");
                return chromosome as TChromosomeType;
            }
            else
            {
                var targetSpecies = Species.Population.GetSpecies(name);
                if (targetSpecies == null)
                    throw new ArgumentException($"Error! {name} is not a valid chromosome");
                return targetSpecies.OptimalCreature.GetChromosome<TChromosomeType>(name);
            }
        }
    }
}
