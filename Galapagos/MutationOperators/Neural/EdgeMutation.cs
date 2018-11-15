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
    /// NEAT (Neural Evolution of Agumenting Topologies) edge mutaton operator.
    /// </summary>
    public class EdgeMutation : Mutation<NeuralChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="EdgeMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal EdgeMutation(double weigth = 1)
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

            var sourceIndex = Session.Instance.Stochastic.Next(nodeGenes.Count);
            var source = nodeGenes[sourceIndex];

            var targetIndex = Session.Instance.Stochastic.Next(nodeGenes.Count);
            var target = nodeGenes[targetIndex];

            var index = targetIndex;
            while(edgeGenes.Any(edge => edge.Input == source && edge.Output == target))
            {
                index = (index + 1) % nodeGenes.Count;
                target = nodeGenes[index];

                // Source node connects to all other nodes. Return original chromosome.
                if (index == targetIndex)
                    return new NeuralChromosome(nodeGenes, edgeGenes, chromosome.InnovationTrackerName, 
                        chromosome.C1, chromosome.C2, chromosome.C3, chromosome.ActivationFunction);
            }

            var edgeId = innovationTracker.GetNextEdgeInnovationNumber(source.ID, target.ID);
            var weight = Session.Instance.Stochastic.NextDouble();
            var newEdge = new NeuralChromosome.EdgeGene(edgeId, source, target, weight, true);

            edgeGenes.Add(newEdge);

            return new NeuralChromosome(nodeGenes, edgeGenes, chromosome.InnovationTrackerName,
                chromosome.C1, chromosome.C2, chromosome.C3, chromosome.ActivationFunction);
        }
    }
}
