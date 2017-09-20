using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Binary
{
    /// <summary>
    /// Single bit mutation operator.
    /// </summary>
    internal class SingleBitMutation : IMutation
    {
        private static readonly Random _rng = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Invokes the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        public IChromosome Invoke(IChromosome chromosome)
        {
            if (!(chromosome is BinaryChromosome))
                throw new ArgumentException("Error! Incompatible chromosome.");

            var binChromosome = chromosome as BinaryChromosome;

            var bits = binChromosome.Bits;
            var power = (_rng.Next() % binChromosome.BitCount - 1);
            var mask = (uint)Math.Pow(2, power);

            return new BinaryChromosome(bits ^ mask, binChromosome.BitCount);
        }
    }
}
