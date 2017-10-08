﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.MutationOperators.Shared;

namespace Galapagos.MutationOperators.Binary
{
    /// <summary>
    /// Cyclic shift mutation operator.
    /// </summary>
    internal class CyclicShiftBinaryMutation : CyclicShiftMutation<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="CyclicShiftBinaryMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public CyclicShiftBinaryMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(BinaryChromosome chromosome)
        {
            var bits = CyclicShift(chromosome.Bits);

            return new BinaryChromosome(bits);
        }
    }
}