using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos
{
    /// <summary>
    /// An evolvable solution to an optimization problem.
    /// </summary>
    public class Creature
    {
        private readonly GeneticDescription _geneticDescription;
        private readonly IDictionary<string, IChromosome> _chromosomes = new Dictionary<string, IChromosome>();

        private double _fitness = 0;

        /// <summary>
        /// Constructs a new instance of the <see cref="Creature"/> class.
        /// </summary>
        /// <param name="description">The genetic descrition of a creature.</param>
        internal Creature(GeneticDescription description)
        {
            _geneticDescription = description;
            foreach (var trait in description)
                _chromosomes.Add(trait.Name, GeneticFactory.ConstructChromosome(trait));
        }

        /// <summary>
        /// Clones the creature.
        /// </summary>
        /// <returns>The cloned creature.</returns>
        internal Creature Clone()
        {
            var clone = new Creature(_geneticDescription);
            foreach(var trait in _geneticDescription)
            {
                var chromosome = GetChromosome(trait.Name);
                clone.SetChromosome(trait.Name, chromosome);
            }
            return clone;
        }

        /// <summary>
        /// Gets the creature's fitness.
        /// </summary>
        public double Fitness
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
                _fitness = _geneticDescription.FitnessFunction(this);
        }

        /// <summary>
        /// Breeds with the given mate creature to create a new creature.
        /// </summary>
        /// <param name="mate">The mate creature.</param>
        /// <returns>The child creature.</returns>
        internal Creature Breed(Creature mate)
        {
            var rng = new Random(DateTime.Now.Millisecond);

            var child = new Creature(_geneticDescription);
            foreach(var trait in _geneticDescription)
            {
                var mutation = trait.Mutations[rng.Next() % trait.Mutations.Count()];
                var crossover = trait.Crossovers[rng.Next() % trait.Crossovers.Count()];

                var R = (double)(rng.Next() % 100) / 100;
                var newDna = R < trait.CrossoverRate ? crossover.Invoke(GetChromosome(trait.Name), mate.GetChromosome(trait.Name)) : GetChromosome(trait.Name);

                R = (double)(rng.Next() % 100) / 100;
                if (R < trait.MutationRate)
                    child.SetChromosome(trait.Name, mutation.Invoke(newDna));
                else
                    child.SetChromosome(trait.Name, newDna);
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
            var chromosome = GetChromosome(name);
            if (!(chromosome is TChromosomeType))
                throw new Exception($"Error! The chromosome {name} is not of type {typeof(TChromosomeType).Name}.");
            return GetChromosome(name) as TChromosomeType;
        }
    }
}
