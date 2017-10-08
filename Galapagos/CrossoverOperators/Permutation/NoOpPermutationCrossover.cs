﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.CrossoverOperators.Shared;

namespace Galapagos.CrossoverOperators.Permutation
{
    /// <summary>
    /// NoOp permutation crossover.
    /// </summary>
    internal class NoOpPermutationCrossover : NoOpCrossover<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="NoOpPermutationCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public NoOpPermutationCrossover(uint weigth = 1)
            : base(weigth) { }
    }
}
