using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;
using Galapagos.API;

namespace Galapagos.CrossoverOperators.Binary
{
    /// <summary>
    /// Single point crossover operator.
    /// </summary>
    public class SinglePointCrossover : Crossover<BinaryChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="SinglePointCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal SinglePointCrossover(double weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation of the crossover operator.
        /// </summary>
        /// <param name="x">The mother chromosome.</param>
        /// <param name="y">The father chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(BinaryChromosome x, BinaryChromosome y)
        {
            if (x.BitCount != y.BitCount)
                throw new ArgumentException("Error! Incompatible chromosomes.");

            var bits = new bool[x.BitCount];
            Array.Copy(x.Bits, bits, x.BitCount);

            var point = Session.Instance.Stochastic.Next(x.BitCount - 1);

            for (var i = point + 1; i < x.BitCount; i++)
                bits[i] = y.Bits[i];

            return new BinaryChromosome(bits);
        }
    }
}
