using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos
{
    /// <summary>
    /// A permutation genetic property of a creature.
    /// </summary>
    public class PermutationChromosome : IChromosome
    {
        private static readonly Random _rng = new Random(DateTime.Now.Millisecond);

        private readonly uint[] _permutation;

        /// <summary>
        /// Constructs a new instance of the <see cref="PermutationChromosome"/> class.
        /// </summary>
        /// <param name="n">The permutation length.</param>
        internal PermutationChromosome(uint n)
        {
            _permutation = GeneratePermutation(n);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="PermutationChromosome"/> class.
        /// </summary>
        /// <param name="permutation">The initial permutation.</param>
        internal PermutationChromosome(uint[] permutation)
        {
            if (!ValidatePermutation(permutation))
                throw new ArgumentException("Error! The given permutation is invalid.");
            _permutation = permutation;
        }

        /// <summary>
        /// Generates a randome permutation.
        /// </summary>
        /// <param name="n">The permutation length.</param>
        /// <returns>A random permutation.</returns>
        private uint[] GeneratePermutation(uint n)
        {
            var permutation = new uint[n];

            for (uint i = 0; i < n; i++)
                permutation[i] = i;

            return permutation.OrderBy(index => _rng.Next()).ToArray();
        }

        /// <summary>
        /// Validates a permutation only contains one of each number.
        /// </summary>
        /// <param name="permutation">The permutation to validate.</param>
        /// <returns>A value indicating if the permutation is valid.</returns>
        private bool ValidatePermutation(uint[] permutation)
        {
            var seen = new bool[permutation.Length];
            for(var i = 0; i < permutation.Length; i++)
            {
                var number = permutation[i];
                if (number >= permutation.Length)
                    return false;
                seen[number] = true;
            }

            return seen.All(o => o);
        }

        /// <summary>
        /// Gets the permutation.
        /// </summary>
        public uint[] Permutation => _permutation;

        /// <summary>
        /// Gets the permutation length.
        /// </summary>
        public uint N => (uint)_permutation.Length;
    }
}
