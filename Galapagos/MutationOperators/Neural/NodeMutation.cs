using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;
using Galapagos.API;

namespace Galapagos.MutationOperators.Neural
{
    /// <summary>
    /// NEAT (Neural Evolution of Agumenting Topologies) node mutaton operator.
    /// </summary>
    public class NodeMutation : Mutation<NeuralChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="NodeMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal NodeMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(NeuralChromosome chromosome)
        {
            var innovationTracker = NeuralChromosome.GetInnovationTracker(chromosome.InnovationTrackerName);
            var genes = chromosome.CloneGenes();
            var nodeGenes = genes.Item1;
            var edgeGenes = genes.Item2;

            var nodeId = innovationTracker.GetNextNodeInnovationNumber();
            var newNode = new NeuralChromosome.NodeGene(nodeId, NeuralChromosome.NodeGene.NodeType.Hidden);

            var index = Stochastic.Next(edgeGenes.Count);
            var selectedEdge = edgeGenes[index];
            var sourceNode = selectedEdge.Input;
            var targetNode = selectedEdge.Output;

            selectedEdge.Enabled = false;

            var edgeId1 = innovationTracker.GetNextEdgeInnovationNumber();
            var newEdge1 = new NeuralChromosome.EdgeGene(edgeId1, selectedEdge.Input, newNode, 1, true);

            var edgeId2 = innovationTracker.GetNextEdgeInnovationNumber();
            var newEdge2 = new NeuralChromosome.EdgeGene(edgeId2, newNode, selectedEdge.Output, selectedEdge.Weight, true);

            nodeGenes.Add(newNode);
            edgeGenes.Add(newEdge1);
            edgeGenes.Add(newEdge2);

            return new NeuralChromosome(nodeGenes, edgeGenes, chromosome.InnovationTrackerName,
                chromosome.C1, chromosome.C2, chromosome.C3);
        }
    }
}
