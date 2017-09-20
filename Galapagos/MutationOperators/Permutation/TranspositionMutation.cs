﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Permutation
{
    /// <summary>
    /// Transposition mutation operator.
    /// </summary>
    internal class TranspositionMutation : IMutation
    {
        private static readonly Random _rng = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Invokes the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        public IChromosome Invoke(IChromosome chromosome)
        {
            if (!(chromosome is PermutationChromosome))
                throw new ArgumentException("Error! Incompatible chromosome.");

            var permChromosome = chromosome as PermutationChromosome;
            var permutation = permChromosome.Permutation;
            long i = 0;
            long j = 0;

            do
            {
                i = _rng.Next() % permChromosome.N;
                j = _rng.Next() % permChromosome.N;
            } while (i == j);

            permutation[i] = permutation[i] + permutation[j];
            permutation[j] = permutation[i] - permutation[j];
            permutation[i] = permutation[i] - permutation[j];

            return new PermutationChromosome(permutation);
        }
    }
}
