using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.API;
using Galapagos.API.ANN;

namespace Galapagos.UnitTests
{
    [TestClass]
    public class NeuralNetworkUnitTests
    {
        private static readonly string _name = "test";
        private static readonly ActivationFunction _activationFunction = ActivationFunction.Identity;
        private static readonly uint[] _inputNeurons = new uint[] { 0, 1, 2 };
        private static readonly uint[] _outputNeurons = new uint[] { 5, 6 };
        private static readonly double[,] _adjacencyMatrix = new double[,]
        {
                { 0, 0, 0, 1, 0, .5, 0 },
                { 0, 0, 0, .5, 0, 0, .25 },
                { 0, 0, 0, 0, .25, 0, 1 },
                { 0, 0, 0, 0, .25, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0 }
        };

        private readonly AnnFile _annFile = new AnnFile
        {
            Name = _name,
            Activation = _activationFunction,
            InputNeurons = _inputNeurons,
            OutputNeurons = _outputNeurons,
            AdjacencyMatrix = _adjacencyMatrix
        };

        [TestMethod]
        public void AnnFileInputOutputTest()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _annFile.WriteToDisk(path);

            var annFile = AnnFile.Open(Path.Combine(path, _name));

            Assert.AreEqual(_annFile.Name, annFile.Name, "Failed to read .ann file name");
            Assert.AreEqual(_annFile.Activation, annFile.Activation, "Failed to read .ann file activation function.");

            for(var i = 0; i < annFile.InputNeurons.Count; i++)
                Assert.AreEqual(_annFile.InputNeurons[i], annFile.InputNeurons[i], "Failed to read .ann file input neurons.");

            for(var i = 0; i < annFile.OutputNeurons.Count; i++)
                Assert.AreEqual(_annFile.OutputNeurons[i], annFile.OutputNeurons[i], "Failed to read .ann file output neurons.");

            var n = annFile.AdjacencyMatrix.GetLength(0);
            var m = annFile.AdjacencyMatrix.GetLength(1);
            for(var i = 0; i < n; i++)
                for(var j = 0; j < m; j++)
                    Assert.AreEqual(_annFile.AdjacencyMatrix[i, j], annFile.AdjacencyMatrix[i, j], "Failed to read .ann file adjacency matrix.");
        }

        [TestMethod]
        public void NeuralNetworkEvaluateTest()
        {
            var network = Session.Instance.LoadNeuralNetwork(_annFile);

            var input = new double[] { 3, 4, 5 };
            var output = network.Evaluate(input);

            Assert.AreEqual(2, output.Length, $"Expected 2 outputs but recieved {output.Length}");
            Assert.AreEqual(4, output[0], $"Expected first output to be 4 but recieved {output[0]}");
            Assert.AreEqual(6, output[1], $"Expected second output to be 6 but recieved {output[1]}");
        }
    }
}
