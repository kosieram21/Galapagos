using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.API;
using Galapagos.Chromosomes;
using Galapagos.SelectionAlgorithms;
using Galapagos.TerminationConditions;
using System.IO;
using System.Reflection;

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

            Assert.AreEqual("binary", metadata[0].Name, "Failed to parse binary chromosome name.");
            Assert.AreEqual(ChromosomeType.Binary, metadata[0].Type, "Failed to parse binary chromosome type.");
            Assert.AreEqual(1, metadata[0].CrossoverRate, "Failed to parse binary chromosome crossover rate.");
            Assert.AreEqual(0.25, metadata[0].MutationRate, "Failled to parse binary chromosome mutation rate.");
            Assert.AreEqual(8, metadata[0].Properties["GeneCount"], "Failed to parse binary chromosome gene count.");

            Assert.AreEqual("permutation", metadata[1].Name, "Failed to parse permutation chromosome name.");
            Assert.AreEqual(ChromosomeType.Permutation, metadata[1].Type, "Failed to parse permutation chromosome type.");
            Assert.AreEqual(1, metadata[1].CrossoverRate, "Failed to parse permutation chromosome crossover rate.");
            Assert.AreEqual(0.1, metadata[1].MutationRate, "Failled to parse permutation chromosome mutation rate.");
            Assert.AreEqual(10, metadata[1].Properties["GeneCount"], "Failed to parse permutation chromosome gene count.");

            Assert.AreEqual("neural", metadata[2].Name, "Failed to parse neural chromosome name.");
            Assert.AreEqual(ChromosomeType.Neural, metadata[2].Type, "Failed to parse neural chromosome type.");
            Assert.AreEqual(1, metadata[2].CrossoverRate, "Failed to parse neural chromosome crossover rate.");
            Assert.AreEqual(0.15, metadata[2].MutationRate, "Failled to parse neural chromosome mutation rate.");
            Assert.AreEqual(4, metadata[2].Properties["InputSize"], "Failed to parse neural chromosome input size.");
            Assert.AreEqual(2, metadata[2].Properties["OutputSize"], "Failed to parse neural chromosome output size.");
            Assert.AreEqual(0.25, metadata[2].Properties["C1"], "Failed to parse neural chromosome C1 weight.");
            Assert.AreEqual(0.25, metadata[2].Properties["C2"], "Failed to parse neural chromosome C2 weight.");
            Assert.AreEqual(0.5, metadata[2].Properties["C3"], "Failed to parse neural chromosome C3 weight.");
        }
    }
}
