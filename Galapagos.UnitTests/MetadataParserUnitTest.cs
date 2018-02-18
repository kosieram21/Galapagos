using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using Galapagos.API;
using Galapagos.Chromosomes;
using Galapagos.SelectionAlgorithms;
using Galapagos.TerminationConditions;
using Galapagos.UnitTests.Fakes;

namespace Galapagos.UnitTests
{
    [TestClass]
    public class MetadataParserUnitTest
    {
        private const string NAMESPACE = @"Galapagos.UnitTests";

        private static Stream LoadEmbeddedResource(string name)
        {
            var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream($"{NAMESPACE}.{name}.xml");

            return stream;
        }

        [TestMethod]
        public void MetadataParseTest()
        {
            var stream = LoadEmbeddedResource(@"Metadata");
            var metadata = Session.Instance.LoadMetadata(stream, null);

            Assert.AreEqual(7500, (int)metadata.Size, "Failed to parse population size.");
            Assert.AreEqual(0, metadata.SurvivalRate, "Failed to parse survival rate.");
            Assert.AreEqual(0, (int)metadata.DistanceThreshold, "Failed to parse distance threshold.");
            Assert.IsFalse(metadata.CooperativeCoevolution, "Failed to parse cooperative coevolution.");
            Assert.IsTrue(metadata.SelectionAlgorithm is TournamentSelection, "Failed to parse selection algorithm.");
            Assert.IsTrue(metadata.TerminationConditions.Count == 1 && metadata.TerminationConditions[0] is FitnessThreshold, "Failed to parse termination conditions.");

            var binary = metadata[0];
            Assert.AreEqual("binary", binary.Name, "Failed to parse binary chromosome name.");
            Assert.AreEqual(ChromosomeType.Binary, binary.Type, "Failed to parse binary chromosome type.");
            Assert.AreEqual(1, binary.CrossoverRate, "Failed to parse binary chromosome crossover rate.");
            Assert.AreEqual(0.25, binary.MutationRate, "Failled to parse binary chromosome mutation rate.");
            Assert.AreEqual(8, binary.Properties["GeneCount"], "Failed to parse binary chromosome gene count.");

            var permutation = metadata[1];
            Assert.AreEqual("permutation", permutation.Name, "Failed to parse permutation chromosome name.");
            Assert.AreEqual(ChromosomeType.Permutation, permutation.Type, "Failed to parse permutation chromosome type.");
            Assert.AreEqual(1, permutation.CrossoverRate, "Failed to parse permutation chromosome crossover rate.");
            Assert.AreEqual(0.1, permutation.MutationRate, "Failled to parse permutation chromosome mutation rate.");
            Assert.AreEqual(10, permutation.Properties["GeneCount"], "Failed to parse permutation chromosome gene count.");

            var neural = metadata[2];
            Assert.AreEqual("neural", neural.Name, "Failed to parse neural chromosome name.");
            Assert.AreEqual(ChromosomeType.Neural, neural.Type, "Failed to parse neural chromosome type.");
            Assert.AreEqual(1, neural.CrossoverRate, "Failed to parse neural chromosome crossover rate.");
            Assert.AreEqual(0.15, neural.MutationRate, "Failled to parse neural chromosome mutation rate.");
            Assert.AreEqual(4, neural.Properties["InputSize"], "Failed to parse neural chromosome input size.");
            Assert.AreEqual(2, neural.Properties["OutputSize"], "Failed to parse neural chromosome output size.");
            Assert.AreEqual(0.25, neural.Properties["C1"], "Failed to parse neural chromosome C1 weight.");
            Assert.AreEqual(0.25, neural.Properties["C2"], "Failed to parse neural chromosome C2 weight.");
            Assert.AreEqual(0.5, neural.Properties["C3"], "Failed to parse neural chromosome C3 weight.");
            Assert.AreEqual(ActivationFunction.ReLu, (ActivationFunction)neural.Properties["ActivationFunction"], "Failed to parse neural chromosome activation function.");
        }

        [TestMethod]
        public void AddRemoveCrossoverTest()
        {
            var stream = LoadEmbeddedResource(@"Metadata");
            var metadata = Session.Instance.LoadMetadata(stream, null);

            var crossover = new CrossoverFake();
            metadata[0].AddCrossover(crossover);
            metadata[0].RemoveCrossover(crossover);
        }

        [TestMethod]
        public void AddRemoveMutationTest()
        {
            var stream = LoadEmbeddedResource(@"Metadata");
            var metadata = Session.Instance.LoadMetadata(stream, null);

            var mutation = new MutationFake();
            metadata[0].AddMutation(mutation);
            metadata[0].RemoveMutation(mutation);
        }
    }
}
