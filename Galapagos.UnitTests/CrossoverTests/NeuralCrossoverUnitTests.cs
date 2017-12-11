using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.Chromosomes;
using Galapagos.API;
using Galapagos.API.Factory;
using Galapagos.UnitTests.Fakes;

namespace Galapagos.UnitTests.CrossoverTests
{
    [TestClass]
    public class NeuralCrossoverUnitTests
    {
        private NeuralChromosome GetChromosomeX()
        {
            var creature = new CreatureFake(3);

            var nodeGenes = new List<NeuralChromosome.NodeGene>
            {
                UnitTestFactory.ConstructNodeGene(0, NeuralChromosome.NodeGene.NodeType.Input),
                UnitTestFactory.ConstructNodeGene(1, NeuralChromosome.NodeGene.NodeType.Input),
                UnitTestFactory.ConstructNodeGene(2, NeuralChromosome.NodeGene.NodeType.Input),
                UnitTestFactory.ConstructNodeGene(3, NeuralChromosome.NodeGene.NodeType.Output),
                UnitTestFactory.ConstructNodeGene(4, NeuralChromosome.NodeGene.NodeType.Hidden)
            };

            var edgeGenes = new List<NeuralChromosome.EdgeGene>
            {
                UnitTestFactory.ConstructEdgeGene(0, nodeGenes[0], nodeGenes[3], 1, true),
                UnitTestFactory.ConstructEdgeGene(1, nodeGenes[1], nodeGenes[3], 2, false),
                UnitTestFactory.ConstructEdgeGene(2, nodeGenes[2], nodeGenes[3], 4.5, true),
                UnitTestFactory.ConstructEdgeGene(3, nodeGenes[1], nodeGenes[4], 1, true),
                UnitTestFactory.ConstructEdgeGene(4, nodeGenes[4], nodeGenes[3], 1.5, true),
                UnitTestFactory.ConstructEdgeGene(7, nodeGenes[0], nodeGenes[4], 1, true),
            };

            var chromosome = UnitTestFactory.ConstructChromosome(ChromosomeType.Neural, nodeGenes, edgeGenes);
            chromosome.Creature = creature;

            return chromosome as NeuralChromosome;
        }

        private NeuralChromosome GetChromosomeY()
        {
            var creature = new CreatureFake(2);

            var nodeGenes = new List<NeuralChromosome.NodeGene>
            {
                UnitTestFactory.ConstructNodeGene(0, NeuralChromosome.NodeGene.NodeType.Input),
                UnitTestFactory.ConstructNodeGene(1, NeuralChromosome.NodeGene.NodeType.Input),
                UnitTestFactory.ConstructNodeGene(2, NeuralChromosome.NodeGene.NodeType.Input),
                UnitTestFactory.ConstructNodeGene(3, NeuralChromosome.NodeGene.NodeType.Output),
                UnitTestFactory.ConstructNodeGene(4, NeuralChromosome.NodeGene.NodeType.Hidden),
                UnitTestFactory.ConstructNodeGene(5, NeuralChromosome.NodeGene.NodeType.Hidden)
            };

            var edgeGenes = new List<NeuralChromosome.EdgeGene>
            {
                UnitTestFactory.ConstructEdgeGene(0, nodeGenes[0], nodeGenes[3], 1, true),
                UnitTestFactory.ConstructEdgeGene(1, nodeGenes[1], nodeGenes[3], 2.5, false),
                UnitTestFactory.ConstructEdgeGene(2, nodeGenes[2], nodeGenes[3], 3.5, true),
                UnitTestFactory.ConstructEdgeGene(3, nodeGenes[1], nodeGenes[4], 1, true),
                UnitTestFactory.ConstructEdgeGene(4, nodeGenes[4], nodeGenes[3], 1, false),
                UnitTestFactory.ConstructEdgeGene(5, nodeGenes[4], nodeGenes[5], 1, true),
                UnitTestFactory.ConstructEdgeGene(6, nodeGenes[5], nodeGenes[3], 1, true),
                UnitTestFactory.ConstructEdgeGene(8, nodeGenes[2], nodeGenes[4], 1, true),
                UnitTestFactory.ConstructEdgeGene(9, nodeGenes[0], nodeGenes[5], 1, true)
            };

            var chromosome = UnitTestFactory.ConstructChromosome(ChromosomeType.Neural, nodeGenes, edgeGenes);
            chromosome.Creature = creature;

            return chromosome as NeuralChromosome;
        }

        private NeuralChromosome GetCrossover(NeuralChromosome x, NeuralChromosome y, NeuralCrossover crossover)
        {
            var rator = GeneticFactory.ConstructNeuralCrossoverOperators(crossover).First();
            return rator.Invoke(x, y) as NeuralChromosome;
        }

        [TestMethod]
        public void NeatCrossoverTest()
        {
            var x = GetChromosomeX();
            var y = GetChromosomeY();
            var crossover = GetCrossover(x, y, NeuralCrossover.Neat);

            Assert.AreEqual(5, crossover.NodeGenes.Count, $"Error! Expexted 5 node genes but recieved {crossover.NodeGenes.Count}");
            Assert.AreEqual(1, crossover.NodeGenes.Where(gene => gene.ID == 0 && gene.Type == NeuralChromosome.NodeGene.NodeType.Input).Count(),
                "Error! Did not pass on input node 0 to child.");
            Assert.AreEqual(1, crossover.NodeGenes.Where(gene => gene.ID == 1 && gene.Type == NeuralChromosome.NodeGene.NodeType.Input).Count(),
                "Error! Did not pass on input node 1 to child.");
            Assert.AreEqual(1, crossover.NodeGenes.Where(gene => gene.ID == 2 && gene.Type == NeuralChromosome.NodeGene.NodeType.Input).Count(),
                "Error! Did not pass on input node 2 to child.");
            Assert.AreEqual(1, crossover.NodeGenes.Where(gene => gene.ID == 3 && gene.Type == NeuralChromosome.NodeGene.NodeType.Output).Count(),
                "Error! Did not pass on output node 3 to child.");
            Assert.AreEqual(1, crossover.NodeGenes.Where(gene => gene.ID == 4 && gene.Type == NeuralChromosome.NodeGene.NodeType.Hidden).Count(),
                "Error! Did not pass on hidden node 4 to child.");

            Assert.AreEqual(6, crossover.EdgeGenes.Count, $"Error! Expexted 6 edge genes but recieved {crossover.EdgeGenes.Count}.");
            Assert.AreEqual(1, crossover.EdgeGenes.Where(gene => gene.ID == 0 && gene.Input.ID == 0 && gene.Output.ID == 3).Count(),
                "Error! Did not pass on edge 0:[0->3] to child.");
            Assert.AreEqual(1, crossover.EdgeGenes.Where(gene => gene.ID == 1 && gene.Input.ID == 1 && gene.Output.ID == 3).Count(),
                "Error! Did not pass on edge 1:[1->3] to child.");
            Assert.AreEqual(1, crossover.EdgeGenes.Where(gene => gene.ID == 2 && gene.Input.ID == 2 && gene.Output.ID == 3).Count(),
                "Error! Did not pass on edge 2:[2->3] to child.");
            Assert.AreEqual(1, crossover.EdgeGenes.Where(gene => gene.ID == 3 && gene.Input.ID == 1 && gene.Output.ID == 4).Count(),
                "Error! Did not pass on edge 3:[1->4] to child.");
            Assert.AreEqual(1, crossover.EdgeGenes.Where(gene => gene.ID == 4 && gene.Input.ID == 4 && gene.Output.ID == 3).Count(),
                "Error! Did not pass on edge 4:[4->3] to child.");
            Assert.AreEqual(1, crossover.EdgeGenes.Where(gene => gene.ID == 7 && gene.Input.ID == 0 && gene.Output.ID == 4).Count(),
                "Error! Did not pass on edge 7:[0->4] to child.");
        }
    }
}
