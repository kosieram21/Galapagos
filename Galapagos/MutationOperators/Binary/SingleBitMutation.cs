﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Binary
{
    /// <summary>
    /// Single bit mutation operator.
    /// </summary>
    internal class SingleBitMutation : Mutation<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="SingleBitMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public SingleBitMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(BinaryChromosome chromosome)
        {
            var bits = new bool[chromosome.BitCount];
            for (var i = 0; i < chromosome.BitCount; i++)
                bits[i] = Stochastic.EvaluateProbability(1 / chromosome.BitCount) ? 
                    !chromosome.Bits[i] : chromosome.Bits[i];

            return new BinaryChromosome(bits);
        }
    }
}
