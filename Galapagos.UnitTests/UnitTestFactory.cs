using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;
using Galapagos.CrossoverOperators;
using Galapagos.CrossoverOperators.Binary;
using Galapagos.CrossoverOperators.Permutation;
using Galapagos.CrossoverOperators.Neural;
using Galapagos.MutationOperators;
using Galapagos.MutationOperators.Binary;
using Galapagos.MutationOperators.Permutation;
using Galapagos.MutationOperators.Neural;
using Galapagos.SelectionAlgorithms;
using Galapagos.TerminationConditions;
using Galapagos.API;

namespace Galapagos.UnitTests
{
    /// <summary>
    /// Utility class for unit testing pieces of a genetic algorithm.
    /// </summary>
    public static class UnitTestFactory
    {
        /// <summary>
        /// Constructs a chromosome from metadata.
        /// </summary>
        /// <param name="chromosomeType">The chromosome type.</param>
        /// <param name="geneCount">The gene count.</param>
        /// <returns>The chromosome.</returns>
        public static IChromosome ConstructChromosome(ChromosomeType chromosomeType, uint geneCount)
        {
            switch (chromosomeType)
            {
                case ChromosomeType.Binary:
                    return new BinaryChromosome(geneCount);
                case ChromosomeType.Permutation:
                    return new PermutationChromosome(geneCount);
                default:
                    throw new ArgumentException($"Error! Invalid chromosome type.");
            }
        }

        /// <summary>
        /// Constructs a chromosome from metadata.
        /// </summary>
        /// <typeparam name="T">The array data type.</typeparam>
        /// <param name="chromosomeType">The chromosome type.</param>
        /// <param name="genes">The chromosome genes genes.</param>
        /// <returns>The chromosome.</returns>
        public static IChromosome ConstructChromosome<T>(ChromosomeType chromosomeType, T[] genes)
        {
            switch (chromosomeType)
            {
                case ChromosomeType.Binary:
                    return new BinaryChromosome(genes as bool[]);
                case ChromosomeType.Permutation:
                    return new PermutationChromosome(genes as uint[]);
                default:
                    throw new ArgumentException($"Error! Invalid chromosome type.");
            }
        }

        /// <summary>
        /// Constructs a chromosome from metadata.
        /// </summary>
        /// <remarks>This constructor is for neural chromosomes only.</remarks>
        /// <param name="chromosomeType">The chromosome type.</param>
        /// <param name="inputSize">The input size.</param>
        /// <param name="outputSize">The output size.</param>
        /// <param name="innovationTrackerName">The innovation tracker name.</param>
        /// <returns></returns>
        public static IChromosome ConstructChromosome(ChromosomeType chromosomeType,
            uint inputSize, uint outputSize, string innovationTrackerName)
        {
            switch (chromosomeType)
            {
                case ChromosomeType.Neural:
                    return new NeuralChromosome(inputSize, outputSize, innovationTrackerName);
                default:
                    throw new ArgumentException($"Error! Invalid chromosome type.");
            }
        }

        /// <summary>
        /// Constructs a chromosome from metadata.
        /// </summary>
        /// <remarks>This constructor is for neural chromosomes only.</remarks>
        /// <param name="chromosomeType">The chromosome type.</param>
        /// <param name="nodeGenes">The node genes.</param>
        /// <param name="edgeGenes">The edge genes.</param>
        /// <returns></returns>
        public static IChromosome ConstructChromosome(ChromosomeType chromosomeType,
            IList<NeuralChromosome.NodeGene> nodeGenes, IList<NeuralChromosome.EdgeGene> edgeGenes)
        {
            switch (chromosomeType)
            {
                case ChromosomeType.Neural:
                    return new NeuralChromosome(nodeGenes, edgeGenes, "UT", 1, 1, 1, ActivationFunction.Sigmoid);
                default:
                    throw new ArgumentException($"Error! Invalid chromosome type.");
            }
        }

        /// <summary>
        /// Constructs a node gene.
        /// </summary>
        /// <remarks>This constructor is for neural chromosomes.</remarks>
        /// <param name="id">The node gene id.</param>
        /// <param name="type">The node gene type.</param>
        /// <returns></returns>
        public static NeuralChromosome.NodeGene ConstructNodeGene(uint id,
            NeuralChromosome.NodeGene.NodeType type)
        {
            return new NeuralChromosome.NodeGene(id, type);
        }

        /// <summary>
        /// Constructs an edge gene.
        /// </summary>
        /// <remarks>This constructor is for neural chromosomes.</remarks>
        /// <param name="id">The edge gene id.</param>
        /// <param name="input">The input node gene.</param>
        /// <param name="output">The output node gene.</param>
        /// <param name="weight">The edge genge weight.</param>
        /// <param name="enabled">A value indicating if the edge gene is enabled.</param>
        /// <returns></returns>
        public static NeuralChromosome.EdgeGene ConstructEdgeGene(uint id,
            NeuralChromosome.NodeGene input, NeuralChromosome.NodeGene output, double weight, bool enabled)
        {
            return new NeuralChromosome.EdgeGene(id, input, output, weight, enabled);
        }
    }
}
