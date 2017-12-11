using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.Chromosomes;
using Galapagos.API;
using Galapagos.API.Factory;

namespace Galapagos.UnitTests.ChromosomeTests
{
    [TestClass]
    public class BinaryChromosomeUnitTests
    {
        [TestMethod]
        public void DistanceTest()
        {
            var x = UnitTestFactory.ConstructChromosome(ChromosomeType.Binary, 
                new bool[] { true, false, true, true, true, false, true });
            var y = UnitTestFactory.ConstructChromosome(ChromosomeType.Binary, 
                new bool[] { true, false, false, true, false, false, true });

            var distance = x.Distance(y);

            Assert.AreEqual((uint)2, distance, $"Error! Expected 2 but recieved {distance}.");
        }
    }
}
