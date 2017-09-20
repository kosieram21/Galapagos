using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Binary
{
    /// <summary>
    /// Boundary mutation operator.
    /// </summary>
    internal class BoundaryMutation : IMutation
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

            var roll = _rng.Next() % 2;
            return roll == 0 ? 
                new BinaryChromosome(uint.MaxValue, binChromosome.BitCount) : 
                new BinaryChromosome(0, binChromosome.BitCount);
        }
    }
}
