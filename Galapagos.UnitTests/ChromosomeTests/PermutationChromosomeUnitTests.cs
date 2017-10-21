using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galapagos.UnitTests.ChromosomeTests
{
    [TestClass]
    public class PermutationChromosomeUnitTests
    {
        [TestMethod]
        public void DistanceTest()
        {
            var x = GeneticFactory.ConstructChromosome(new PermutationChromosomeMetadata("x", 5), new uint[] { 0, 1, 2, 3, 4 });
            var y = GeneticFactory.ConstructChromosome(new PermutationChromosomeMetadata("y", 5), new uint[] { 1, 3, 0, 2, 4 });

            var distance = x.Distance(y);

            Assert.AreEqual((uint)3, distance);
        }
    }
}
