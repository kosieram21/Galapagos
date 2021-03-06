﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.Chromosomes;
using Galapagos.API;
using Galapagos.API.Factory;
using Galapagos.UnitTests.Fakes;

namespace Galapagos.UnitTests.ChromosomeTests
{
    [TestClass]
    public class NeuralChromosomeUnitTests
    {
        private NeuralChromosome GetChromosomeX()
        {
            var creature = new CreatureFake(2);

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
            var creature = new CreatureFake(1);

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

        [TestMethod]
        public void DistanceTest()
        {
            var x = GetChromosomeX();
            var y = GetChromosomeY();
            var distance = Math.Round(x.Distance(y), 2);

            Assert.AreEqual(0.96, distance, $"Error! Expected 0.96 but recieved {distance}.");
        }
    }
}
