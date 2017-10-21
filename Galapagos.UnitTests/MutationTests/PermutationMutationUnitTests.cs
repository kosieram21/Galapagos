using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galapagos.UnitTests.MutationTests
{
    [TestClass]
    public class PermutationMutationUnitTests
    {
        private const int GENE_COUNT = 10;

        private PermutationChromosome GetChromosome()
        {
            return GeneticFactory.ConstructChromosome(new PermutationChromosomeMetadata("test", GENE_COUNT))
                as PermutationChromosome;
        }

        private PermutationChromosome GetMutation(PermutationChromosome chromosome, PermutationMutation mutation)
        {
            var rator = GeneticFactory.ConstructPermutationMutationOperators(mutation).First();
            return rator.Invoke(chromosome) as PermutationChromosome;
        }

        [TestMethod]
        public void CyclicShiftMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, PermutationMutation.CyclicShift);

            for (var i = 0; i < GENE_COUNT; i++)
            {
                if (mutation.Permutation[i] != chromosome.Permutation[i])
                {
                    while (mutation.Permutation[i] != chromosome.Permutation[i])
                    {
                        if (i >= GENE_COUNT)
                            break;

                        Assert.IsTrue(mutation.Permutation[i] == chromosome.Permutation[i + 1]);

                        i++;
                    }
                }
            }
        }

        [TestMethod]
        public void ReverseMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, PermutationMutation.Reverse);

            var start = 0;
            var end = 0;

            for (var i = 0; i < GENE_COUNT; i++)
            {
                if (mutation.Permutation[i] != chromosome.Permutation[i])
                {
                    start = i;
                    while (i < GENE_COUNT)
                    {
                        if (mutation.Permutation[i] != chromosome.Permutation[i])
                            end = i;
                        i++;
                    }
                }
            }

            for (var i = 0; i < (end - start); i++)
                Assert.IsTrue(mutation.Permutation[start + i] == chromosome.Permutation[end - i]);
        }

        [TestMethod]
        public void TranspositionMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, PermutationMutation.Transposition);

            var different = 0;
            for(var i = 0; i < GENE_COUNT; i++)
            {
                if (mutation.Permutation[i] != chromosome.Permutation[i])
                    different++;
            }

            Assert.AreEqual(2, different);
        }
    }
}
