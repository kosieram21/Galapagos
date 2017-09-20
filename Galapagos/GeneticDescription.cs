using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.CrossoverOperators;
using Galapagos.MutationOperators;

namespace Galapagos
{
    /// <summary>
    /// A genetic description of a creature.
    /// </summary>
    public class GeneticDescription : IEnumerable<GeneticDescription.Trait>
    {
        /// <summary>
        /// A genetic description of a chromosome.
        /// </summary>
        public abstract class Trait
        {
            private readonly string _name;
            private readonly double _crossoverRate;
            private readonly double _mutationRate;

            protected IList<ICrossover> _crossovers;
            protected IList<IMutation> _mutations;

            /// <summary>
            /// Constructs a new instance of the <see cref="GeneticDescription.Trait"/> class.
            /// </summary>
            /// <param name="name">The trait name.</param>
            /// <param name="crossoverRate">The crossover rate.</param>
            /// <param name="mutationRate">The mutation rate.</param>
            internal Trait(string name, double crossoverRate = 0.75, double mutationRate = 0.25)
            {
                if (crossoverRate < 0 || crossoverRate > 1)
                    throw new ArgumentException("Error! Crossover rate must be a value between 0 and 1.");

                if (mutationRate < 0 || mutationRate > 1)
                    throw new ArgumentException("Error! Mutation rate must be a value between 0 and 1.");

                _name = name;
                _crossoverRate = crossoverRate;
                _mutationRate = mutationRate;
            }

            /// <summary>
            /// The trait name.
            /// </summary>
            public string Name => _name;

            /// <summary>
            /// The crossover rate.
            /// </summary>
            public double CrossoverRate => _crossoverRate;

            /// <summary>
            /// The trait mutation rate.
            /// </summary>
            public double MutationRate => _mutationRate;

            /// <summary>
            /// The crossover operators.
            /// </summary>
            internal IList<ICrossover> Crossovers => _crossovers;

            /// <summary>
            /// The mutation operators.
            /// </summary>
            internal IList<IMutation> Mutations => _mutations;
        }

        /// <summary>
        /// A genetic description of a binary chromosome.
        /// </summary>
        public class BinaryTrait : Trait
        {
            private readonly uint _bitCount;

            /// <summary>
            /// Constructs a new instance of the <see cref="GeneticDescription.BinaryTrait"/> class.
            /// </summary>
            /// <param name="name">The trait name.</param>
            /// <param name="bitCount">The trait bit count.</param>
            /// <param name="crossoverRate">The crossover rate.</param>
            /// <param name="mutationRate">The mutation rate.</param>
            /// <param name="crossoverOptions">The crossover options.</param>
            /// <param name="mutationOptions">The mutation options.</param>
            public BinaryTrait(string name, uint bitCount = 32, double crossoverRate = 0.75, double mutationRate = 0.25,
                BinaryCrossover crossoverOptions = BinaryCrossover.SinglePoint,
                BinaryMutation mutationOptions = BinaryMutation.FlipBit | BinaryMutation.SingleBit)
                : base(name, crossoverRate, mutationRate)
            {
                if (mutationRate < 0 || mutationRate > 1)
                    throw new ArgumentException("Error! Mutation rate must be a value between 0 and 1.");

                _bitCount = bitCount;

                _crossovers = GeneticFactory.ConstructBinaryCrossoverOperators(crossoverOptions);
                _mutations = GeneticFactory.ConstructBinaryMutationOperators(mutationOptions);
            }

            /// <summary>
            /// The bit count.
            /// </summary>
            public uint Bitcount => _bitCount;
        }

        /// <summary>
        /// A genetic description of a permutation chromosome.
        /// </summary>
        public class PermutationTrait : Trait
        {
            private readonly uint _n;

            /// <summary>
            /// Constructs a new instance of the <see cref="GeneticDescription.PermutationTrait"/> class.
            /// </summary>
            /// <param name="name">The trait name.</param>
            /// <param name="n">The trait permutation size.</param>
            /// <param name="crossoverRate">The crossover rate.</param>
            /// <param name="mutationRate">The mutation rate.</param>
            /// <param name="crossoverOptions">The crossover options.</param>
            /// <param name="mutationOptions">The mutation options.</param>
            public PermutationTrait(string name, uint n, double crossoverRate = 0.75, double mutationRate = 0.25,
                PermutationCrossover crossoverOptions = PermutationCrossover.Order,
                PermutationMutation mutationOptions = PermutationMutation.Transposition)
                : base(name, crossoverRate, mutationRate)
            {
                if (mutationRate < 0 || mutationRate > 1)
                    throw new ArgumentException("Error! Mutation rate must be a value between 0 and 1.");

                _n = n;

                _crossovers = GeneticFactory.ConstructPermutationCrossoverOperators(crossoverOptions);
                _mutations = GeneticFactory.ConstructPermutationMutationOperators(mutationOptions);
            }

            /// <summary>
            /// The permutation size.
            /// </summary>
            public uint N => _n;
        }

        private readonly IList<Trait> _traits = new List<Trait>();
        private readonly Func<Creature, uint> _fitnessFunction;

        /// <summary>
        /// Constructs a new instance of the <see cref="GeneticDescription"/> class.
        /// </summary>
        /// <param name="fitnessFunction">The fitness function.</param>
        /// <param name="traits">The genetic traits.</param>
        public GeneticDescription(Func<Creature, uint> fitnessFunction, IList<Trait> traits = null)
        {
            _fitnessFunction = fitnessFunction;
            if (traits != null)
            {
                foreach (var trait in traits)
                    Add(trait);
            }
        }

        /// <summary>
        /// Gets the fitness function.
        /// </summary>
        internal Func<Creature, uint> FitnessFunction => _fitnessFunction;

        /// <summary>
        /// Adds a trait to the genetic description.
        /// </summary>
        /// <param name="trait">The trait to add.</param>
        public void Add(Trait trait)
        {
            if (_traits.Any(t => t.Name == trait.Name))
                throw new ArgumentException($"Error! A trait named {trait.Name} already exists in the description.");

            _traits.Add(trait);
        }

        /// <summary>
        /// Gets a trait from the description.
        /// </summary>
        /// <param name="i">The trait index.</param>
        /// <returns>The trait.</returns>
        public Trait this[int i] => _traits[i];

        #region IEnumerable Members

        public IEnumerator<Trait> GetEnumerator()
        {
            return _traits.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
