using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.Chromosomes;
using Galapagos.API;
using Galapagos.API.Factory;

namespace Galapagos.UnitTests.CrossoverTests
{
    [TestClass]
    public class BinaryCrossoverUnitTests : StochasticUnitTestBase
    {
        private const int GENE_COUNT = 5000;

        private BinaryChromosome GetChromosome()
        {
            return UnitTestFactory.ConstructChromosome(ChromosomeType.Binary, GENE_COUNT)
                as BinaryChromosome;
        }

        private BinaryChromosome GetCrossover(BinaryChromosome x, BinaryChromosome y, BinaryCrossover crossover)
        {
            var rator = GeneticFactory.ConstructBinaryCrossoverOperators(crossover).First();
            return rator.Invoke(x, y) as BinaryChromosome;
        }

        [TestMethod]
        public void SinglePointCrossoverTest()
        {
            ExecuteUnitTest(() =>
            {
                var x = GetChromosome();
                var y = GetChromosome();

                var midPoint = (int)(x.BitCount / 2);
                _NextMax = (maxValue) => midPoint;

                var crossover = GetCrossover(x, y, BinaryCrossover.SinglePoint);

                for (var i = 0; i < x.BitCount; i++)
                {
                    if (i <= midPoint) Assert.AreEqual(x.Bits[i], crossover.Bits[i], "Inccorrect implementation.");
                    else Assert.AreEqual(y.Bits[i], crossover.Bits[i], "Inccorrect implementation.");
                }
            });
        }

        [TestMethod]
        public void TwoPointCrossoverTest()
        {
            ExecuteUnitTest(() =>
            {
                var x = GetChromosome();
                var y = GetChromosome();

                var start = (int)(x.BitCount / 4);
                var end = (int)(x.BitCount / 2);
                _NextMax = (maxVale) => start;
                _NextMinMax = (minValue, maxValue) => end;

                var crossover = GetCrossover(x, y, BinaryCrossover.TwoPoint);

                for (var i = 0; i < x.BitCount; i++)
                {
                    if (i < start || i >= end) Assert.AreEqual(x.Bits[i], crossover.Bits[i], "Inccorrect implementation.");
                    else Assert.AreEqual(y.Bits[i], crossover.Bits[i], "Inccorrect implementation.");
                }
            });
        }

        [TestMethod]
        public void UniformCrossoverTest()
        {
            ExecuteUnitTest(() =>
            {
                var x = GetChromosome();
                var y = GetChromosome();
                var crossover = GetCrossover(x, y, BinaryCrossover.SinglePoint);

                VerifyChangeOccured(x, crossover);
                VerifyChangeOccured(y, crossover);

                for (var i = 0; i < GENE_COUNT; i++)
                {
                    var consistent = crossover.Bits[i] == x.Bits[i] || crossover.Bits[i] == y.Bits[i];
                    Assert.IsTrue(consistent, "Inccorrect implementation.");
                }
            });
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
    }
}
