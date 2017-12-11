using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.Chromosomes;
using Galapagos.API;
using Galapagos.API.Factory;

namespace Galapagos.UnitTests.ChromosomeTests
{
    [TestClass]
    public class PermutationChromosomeUnitTests
    {
        [TestMethod]
        public void DistanceTest()
        {
            var x = UnitTestFactory.ConstructChromosome(ChromosomeType.Permutation, 
                new uint[] { 0, 1, 2, 3, 4 });
            var y = UnitTestFactory.ConstructChromosome(ChromosomeType.Permutation, 
                new uint[] { 1, 3, 0, 2, 4 });

            var distance = x.Distance(y);

            Assert.AreEqual((uint)3, distance, $"Error! Expected 3 but recieved {distance}.");
        }
    }
}
