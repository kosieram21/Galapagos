using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galapagos.UnitTests.MutationTests
{
    [TestClass]
    public class BinaryMutationUnitTests
    {
        private const int GENE_COUNT = 5000;

        private BinaryChromosome GetChromosome()
        {
            return GeneticFactory.ConstructChromosome(new BinaryChromosomeMetadata("test", GENE_COUNT)) 
                as BinaryChromosome;
        }

        private BinaryChromosome GetMutation(BinaryChromosome chromosome, BinaryMutation mutation)
        {
            var rator = GeneticFactory.ConstructBinaryMutationOperators(mutation).First();
            return rator.Invoke(chromosome) as BinaryChromosome;
        }

        private void VerifyChangeOccured(BinaryChromosome x, BinaryChromosome y)
        {
            var diff = 0;
            for(var i = 0; i < GENE_COUNT; i++)
            {
                if (x.Bits[i] != y.Bits[i])
                    diff++;
            }

            Assert.IsTrue(diff > 0, "Mutation failed to change chromosome data.");
        }

        private Tuple<int, int> GetDifferenceRange(BinaryChromosome chromosome, BinaryChromosome mutation)
        {
            var start = 0;
            var end = 0;

            for (var i = 0; i < GENE_COUNT; i++)
            {
                if (mutation.Bits[i] != chromosome.Bits[i])
                {
                    start = i;
                    while (i < GENE_COUNT)
                    {
                        if (mutation.Bits[i] != chromosome.Bits[i])
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
            var mutation = GetMutation(chromosome, BinaryMutation.CyclicShift);

            VerifyChangeOccured(chromosome, mutation);

            var range = GetDifferenceRange(chromosome, mutation);

            for (var i = range.Item1; i < range.Item2; i++)
                Assert.IsTrue(mutation.Bits[i] == chromosome.Bits[i + 1], "Inccorrect implementation");
            Assert.IsTrue(mutation.Bits[range.Item2] == chromosome.Bits[range.Item1], "Incorrect implementation");
        }

        [TestMethod]
        public void FlipBitMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, BinaryMutation.FlipBit);

            VerifyChangeOccured(chromosome, mutation);

            for (var i = 0; i < GENE_COUNT; i++)
                Assert.IsTrue(mutation.Bits[i] != chromosome.Bits[i], "Incorrect implementation.");
        }

        [TestMethod]
        public void RandomizationMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, BinaryMutation.Randomization);

            VerifyChangeOccured(chromosome, mutation);
        }

        [TestMethod]
        public void ReverseMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, BinaryMutation.Reverse);

            VerifyChangeOccured(chromosome, mutation);

            var range = GetDifferenceRange(chromosome, mutation);

            for (var i = 0; i < (range.Item2 - range.Item1); i++)
                Assert.IsTrue(mutation.Bits[range.Item1 + i] == chromosome.Bits[range.Item2 - i], "Incorrect implementation.");
        }

        [TestMethod]
        public void SingleBitMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, BinaryMutation.SingleBit);

            VerifyChangeOccured(chromosome, mutation);
        }
    }
}
