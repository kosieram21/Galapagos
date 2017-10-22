using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;
using Galapagos.API;

namespace Galapagos.CrossoverOperators.Permutation
{
    /// <summary>
    /// Alternating position crossover operator.
    /// </summary>
    public class AlternatingPositionCrossover : Crossover<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="AlternatingPositionCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal AlternatingPositionCrossover(uint weigth = 1)
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
            var permutation = new uint[x.N];

            var j = 0;
            for(var i = 0; i < x.N; i++)
            {
                var numX = x.Permutation[i];
                var numY = y.Permutation[i];

                if(!seen[numX])
                {
                    permutation[j] = numX;
                    seen[numX] = true;
                    j++;
                }

                if(!seen[numY])
                {
                    permutation[j] = numY;
                    seen[numY] = true;
                    j++;
                }

                if (j == x.N)
                    break;
            }

            return new PermutationChromosome(permutation);
        }
    }
}
