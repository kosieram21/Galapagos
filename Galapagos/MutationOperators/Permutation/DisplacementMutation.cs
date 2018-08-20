using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;
using Galapagos.API;

namespace Galapagos.MutationOperators.Permutation
{
    class DisplacementMutation : Mutation<PermutationChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="DisplacementMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal DisplacementMutation(double weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(PermutationChromosome chromosome)
        {
            var N = chromosome.N;
            var permutation = new uint[N];
            Array.Copy(chromosome.Permutation, permutation, chromosome.N);

            var displacementSize = Stochastic.Next(1, N);
            var displacementStart = Stochastic.Next(N);
            var displacementEnd = (displacementStart + displacementSize) % N;
            var insertionPoint = Stochastic.Next(N - displacementSize);
            var offset = displacementEnd > displacementStart ? 0 : displacementEnd;

            for(var i = 0; i < insertionPoint; i++)
            {
                if (i < displacementStart)
                    permutation[i] = chromosome[(int)(i + offset)];
                else
                    permutation[i] = chromosome[(int)((i + offset + displacementSize) % N)];
            }

            for (var i = 0; i < displacementSize; i++)
                permutation[insertionPoint + i] = chromosome[(int)((i + displacementStart) % N)];

            for (var i = insertionPoint; i < N - displacementSize; i++)
            {
                if(i < displacementStart)
                    permutation[i + displacementSize] = chromosome[(int)(i + offset)];
                else
                    permutation[i + displacementSize] = chromosome[(int)((i + offset + displacementSize) % N)];
            }

            return new PermutationChromosome(permutation);
        }
    }
}
