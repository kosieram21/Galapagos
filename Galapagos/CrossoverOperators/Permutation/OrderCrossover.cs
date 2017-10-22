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
    /// Order crossover operator.
    /// </summary>
    internal class OrderCrossover : Crossover<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="OrderCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        public OrderCrossover(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation of the crossover operator.
        /// </summary>
        /// <param name="x">The mother chromosome.</param>
        /// <param name="y">The father chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(PermutationChromosome x, PermutationChromosome y)
        {
            if(x.N != y.N)
                throw new ArgumentException("Error! Incompatible chromosomes.");

            var seen = new bool[x.N];
            var permutation = new uint[x.N];

            var start = Stochastic.Next(x.N - 1);
            var end = Stochastic.Next(start + 1, x.N);

            for (var i = start; i <= end; i++)
            {
                var number = x.Permutation[i];
                permutation[i] = number;
                seen[number] = true;
            }

            var index = (end + 1) % x.N;
            for(var i = 1; i <= x.N; i++)
            {
                var number = y.Permutation[(end + i) % x.N];
                if(!seen[number])
                {
                    permutation[index % x.N] = number;
                    seen[number] = true;
                    index = (index + 1) % x.N;
                }
            }

            return new PermutationChromosome(permutation);
        }
    }
}
