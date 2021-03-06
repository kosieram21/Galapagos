﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;
using Galapagos.CrossoverOperators.Shared;
using Galapagos.API;

namespace Galapagos.CrossoverOperators.Binary
{
    /// <summary>
    /// NoOp binary crossover.
    /// </summary>
    public class NoOpBinaryCrossover : NoOpCrossover<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="NoOpBinaryCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal NoOpBinaryCrossover(double weigth = 1)
            : base(weigth) { }
    }
}
