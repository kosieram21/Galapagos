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

            long displacementSize = Stochastic.Next(N);
            long displacementStart = Stochastic.Next(N);
            long displacementEnd = displacementStart + displacementSize - 1;
            long insertionPoint = Stochastic.Next(
                displacementEnd + 1,
                displacementEnd + N - displacementSize
                ) % N;
            long leftSize = ((insertionPoint - displacementEnd) % N) - 1; 

            // Copy displaced elements
            var displacedElements = new uint[displacementSize];
            for (long i = 0; i < displacementSize; i++)
                displacedElements[i] = permutation[(i + displacementStart) % N];

            // Overwritting permutation
            for (long i = 0; i < leftSize; i++)
                permutation[(displacementStart + i) % N] = permutation[(displacementEnd + i + 1) % N];
            for (long i = 0; i < displacementSize; i++)
                permutation[(displacementEnd + leftSize + i + 1) % N] = displacedElements[i];

            // Cyclic shift the permutation
            if(insertionPoint <= displacementStart)
            {
                var k = leftSize + 1;
                var first = permutation[insertionPoint];
                for (var i = 0; i < k; i++)
                    permutation[insertionPoint + i] = permutation[displacementStart + i];
                permutation[insertionPoint + k - 1] = first;
            }

            return new PermutationChromosome(permutation);
        }
    }
}
