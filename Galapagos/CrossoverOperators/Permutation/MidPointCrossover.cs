using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.CrossoverOperators.Permutation
{
    /// <summary>
    /// Midpoint crossover operator
    /// </summary>
    internal class MidpointCrossover : Crossover<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="MidpointCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public MidpointCrossover(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation of the crossover operator.
        /// </summary>
        /// <param name="x">The mother chromosome.</param>
        /// <param name="y">The father chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(PermutationChromosome x, PermutationChromosome y)
        {
            if (x.N != y.N)
                throw new ArgumentException("Error! Incompatible chromosomes.");

            var seen = new bool[x.N];
            var midPoint = x.N / 2;
            var permutation = new uint[x.N];

            for (var i = 0; i < midPoint; i++)
            {
                var number = x.Permutation[i];
                permutation[i] = number;
                seen[number] = true;
            }

            var index = midPoint;
            for (var i = 0; i < x.N; i++)
            {
                var number = y.Permutation[i];
                if (!seen[number])
                {
                    permutation[index] = number;
                    seen[number] = true;
                    index++;
                }
            }

            return new PermutationChromosome(permutation);
        }
    }
}
