using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.Chromosomes;
using Galapagos.API.ANN;
using Galapagos.API;
using Galapagos.API.Factory;

namespace Galapagos.UnitTests.ChromosomeTests
{
    [TestClass]
    public class NeuralChromosomeUnitTests
    {
        [TestMethod]
        public void NeuralNetworkTest()
        {
            var inputNeurons = new uint[] { 0, 1, 2 };
            var outputNeurons = new uint[] { 5, 6 };
            var adjacencyMatrix = new double[,]
            {
                { 0, 0, 0, 1, 0, .5, 0 },
                { 0, 0, 0, .5, 0, 0, .25 },
                { 0, 0, 0, 0, .25, 0, 1 },
                { 0, 0, 0, 0, .25, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0 }
            };

            var neuralNetwork = new NeuralNetwork(adjacencyMatrix, inputNeurons, outputNeurons);

            var input = new double[] { 3, 4, 5 };
            var output = neuralNetwork.Evaluate(input);

            Assert.AreEqual(2, output.Length, $"Expected 2 outputs but recieved {output.Length}");
            Assert.AreEqual(4, output[0], $"Expected first output to be 4 but recieved {output.Length}");
            Assert.AreEqual(6, output[1], $"Expected second output to be 6 but recieved {output.Length}");
        }

        [TestMethod]
        public void DistanceTest()
        {
        }
    }
}
