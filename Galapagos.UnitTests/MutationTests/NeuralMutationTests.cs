using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.Chromosomes;
using Galapagos.API;
using Galapagos.API.Factory;

namespace Galapagos.UnitTests.MutationTests
{
    [TestClass]
    public class NeuralMutationTests
    {
        private const uint INPUT_SIZE = 20;
        private const uint OUTPUT_SIZE = 10;
        private const string INNOVATION_TRACKER_NAME = "nn";

        private NeuralChromosome GetChromosome()
        {
            return UnitTestFactory.ConstructChromosome(ChromosomeType.Neural, INPUT_SIZE, OUTPUT_SIZE, INNOVATION_TRACKER_NAME)
                as NeuralChromosome;
        }

        private NeuralChromosome GetMutation(NeuralChromosome chromosome, NeuralMutation mutation)
        {
            var rator = GeneticFactory.ConstructNeuralMutationOperators(mutation).First();
            return rator.Invoke(chromosome) as NeuralChromosome;
        }

        [TestMethod]
        public void EdgeMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, NeuralMutation.Edge);

            Assert.IsTrue(mutation.EdgeGenes.Count != 0, "Failed to create new edge gene.");
        }

        [TestMethod]
        public void NodeMutationTest()
        {
            var chromosome = GetChromosome();
            for (var i = 0; i < 10; i++)
                chromosome = GetMutation(chromosome, NeuralMutation.Edge);

            var mutation = GetMutation(chromosome, NeuralMutation.Node);

            Assert.IsTrue(mutation.EdgeGenes.Count == chromosome.EdgeGenes.Count + 2, 
                "Failed to create two new edge genes");
            Assert.AreEqual(1, mutation.EdgeGenes[chromosome.EdgeGenes.Count].Weight,
                "Failed to properly set incoming edge weight.");
            Assert.IsTrue(
                chromosome.EdgeGenes
                .Any(gene => gene.Weight == mutation.EdgeGenes[chromosome.EdgeGenes.Count + 1].Weight),
                "Failed to properly set outgoing edge weight");
            Assert.IsTrue(mutation.EdgeGenes.Any(gene => !gene.Enabled),
                "Failed to disable unused edge.");
        }
    }
}
