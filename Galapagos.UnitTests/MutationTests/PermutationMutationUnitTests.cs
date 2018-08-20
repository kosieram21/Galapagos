using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.Chromosomes;
using Galapagos.API;
using Galapagos.API.Factory;

namespace Galapagos.UnitTests.MutationTests
{
    [TestClass]
    public class PermutationMutationUnitTests
    {
        private const int GENE_COUNT = 5000;

        private PermutationChromosome GetChromosome()
        {
            return UnitTestFactory.ConstructChromosome(ChromosomeType.Permutation, GENE_COUNT)
                as PermutationChromosome;
        }

        private PermutationChromosome GetMutation(PermutationChromosome chromosome, PermutationMutation mutation)
        {
            var rator = GeneticFactory.ConstructPermutationMutationOperators(mutation).First();
            return rator.Invoke(chromosome) as PermutationChromosome;
        }

        private void VerifyChangeOccured(PermutationChromosome x, PermutationChromosome y)
        {
            var diff = 0;
            for (var i = 0; i < GENE_COUNT; i++)
            {
                if (x.Permutation[i] != y.Permutation[i])
                    diff++;
            }

            Assert.IsTrue(diff > 0, "Mutation failed to change chromosome data.");
        }

        private Tuple<int, int> GetDifferenceRange(PermutationChromosome chromosome, PermutationChromosome mutation)
        {
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

            return new Tuple<int, int>(start, end);
        }

        [TestMethod]
        public void CyclicShiftMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, PermutationMutation.CyclicShift);

            VerifyChangeOccured(chromosome, mutation);

            var range = GetDifferenceRange(chromosome, mutation);

            for (var i = range.Item1; i < range.Item2; i++)
                Assert.IsTrue(mutation.Permutation[i] == chromosome.Permutation[i + 1], "Inccorrect implementation.");
            Assert.IsTrue(mutation.Permutation[range.Item2] == chromosome.Permutation[range.Item1], "Incorrect implementation.");
        }

        [TestMethod]
        public void RandomizationMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, PermutationMutation.Randomization);

            VerifyChangeOccured(chromosome, mutation);
        }

        [TestMethod]
        public void ReverseMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, PermutationMutation.Reverse);

            VerifyChangeOccured(chromosome, mutation);

            var range = GetDifferenceRange(chromosome, mutation);

            for (var i = 0; i < (range.Item2 - range.Item1); i++)
                Assert.IsTrue(mutation.Permutation[range.Item1 + i] == chromosome.Permutation[range.Item2 - i], "Incorrect implementation.");
        }

        [TestMethod]
        public void TranspositionMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, PermutationMutation.Transposition);

            VerifyChangeOccured(chromosome, mutation);

            var different = 0;
            for(var i = 0; i < GENE_COUNT; i++)
            {
                if (mutation.Permutation[i] != chromosome.Permutation[i])
                    different++;
            }

            Assert.AreEqual(2, different);
        }

        [TestMethod]
        public void DisplacementMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, PermutationMutation.Displacement);

            VerifyChangeOccured(chromosome, mutation);
        }
    }
}
