/*using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.Chromosomes;
using Galapagos.API;
using Galapagos.API.Factory;

namespace Galapagos.UnitTests.CrossoverTests
{
    [TestClass]
    public class BinaryCrossoverUnitTests
    {
        private const int GENE_COUNT = 5000;

        private BinaryChromosome GetChromosome()
        {
            return GeneticFactory.ConstructChromosome(new BinaryChromosomeMetadata("NULL", GENE_COUNT))
                as BinaryChromosome;
        }

        private BinaryChromosome GetCrossover(BinaryChromosome x, BinaryChromosome y, BinaryCrossover crossover)
        {
            var rator = GeneticFactory.ConstructBinaryCrossoverOperators(crossover).First();
            return rator.Invoke(x, y) as BinaryChromosome;
        }

        private void VerifyChangeOccured(BinaryChromosome x, BinaryChromosome y)
        {
            var diff = 0;
            for (var i = 0; i < GENE_COUNT; i++)
            {
                if (x.Bits[i] != y.Bits[i])
                    diff++;
            }

            Assert.IsTrue(diff > 0, "Mutation failed to change chromosome data.");
        }

        [TestMethod]
        public void SinglePointCrossoverTest()
        {
            var x = GetChromosome();
            var y = GetChromosome();
            var crossover = GetCrossover(x, y, BinaryCrossover.SinglePoint);

            VerifyChangeOccured(x, crossover);
            VerifyChangeOccured(y, crossover);

            var point = 0;
            for(var i = 0; i < GENE_COUNT; i++)
            {
                if(crossover.Bits[i] != x.Bits[i])
                {
                    point = i;
                    break;
                }
            }

            for (var i = point; i < GENE_COUNT; i++)
                Assert.IsTrue(crossover.Bits[i] == y.Bits[i], "Inccorrect implementation.");
        }

        [TestMethod]
        public void TwoPointCrossoverTest()
        {
            var x = GetChromosome();
            var y = GetChromosome();
            var crossover = GetCrossover(x, y, BinaryCrossover.TwoPoint);

            VerifyChangeOccured(x, crossover);
            VerifyChangeOccured(y, crossover);

            var start = 0;
            var end = 0;

            for (var i = 0; i < GENE_COUNT; i++)
            {
                if (crossover.Bits[i] != x.Bits[i])
                {
                    start = i;
                    while (i < GENE_COUNT)
                    {
                        if (crossover.Bits[i] != x.Bits[i])
                            end = i;
                        i++;
                    }
                }
            }

            for (var i = 0; i < start; i++)
                Assert.IsTrue(crossover.Bits[i] == x.Bits[i], "Inccorrect implementation.");

            for(var i = start; i <= end; i++)
                Assert.IsTrue(crossover.Bits[i] == y.Bits[i], "Inccorrect implementation.");

            for (var i = end + 1; i < GENE_COUNT; i++)
                Assert.IsTrue(crossover.Bits[i] == x.Bits[i], "Inccorrect implementation.");
        }

        [TestMethod]
        public void UniformCrossoverTest()
        {
            var x = GetChromosome();
            var y = GetChromosome();
            var crossover = GetCrossover(x, y, BinaryCrossover.SinglePoint);

            VerifyChangeOccured(x, crossover);
            VerifyChangeOccured(y, crossover);

            for(var i = 0; i < GENE_COUNT; i++)
            {
                var consistent = crossover.Bits[i] == x.Bits[i] || crossover.Bits[i] == y.Bits[i];
                Assert.IsTrue(consistent, "Inccorrect implementation.");
            }
        }
    }
}*/
