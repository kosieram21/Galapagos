using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;
using Galapagos.Chromosomes;

namespace Galapagos.CrossoverOperators.Neural
{
    /// <summary>
    /// NEAT (Neural Evolution of Agumenting Topologies) crossover operator.
    /// </summary>
    public class NeatCrossover : Crossover<NeuralChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="NeatCrossover"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal NeatCrossover(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation of the crossover operator.
        /// </summary>
        /// <param name="x">The mother chromosome.</param>
        /// <param name="y">The father chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(NeuralChromosome x, NeuralChromosome y)
        {
            var sortedNodeGenesX = GetSortedGenes<NeuralChromosome.NodeGene>(x);
            var sortedNodeGenesY = GetSortedGenes<NeuralChromosome.NodeGene>(y);

            var sortedEdgeGenesX = GetSortedGenes<NeuralChromosome.EdgeGene>(x);
            var sortedEdgeGenesY = GetSortedGenes<NeuralChromosome.EdgeGene>(y);

            var geneQueueX = new Queue<NeuralChromosome.EdgeGene>(sortedEdgeGenesX);
            var geneQueueY = new Queue<NeuralChromosome.EdgeGene>(sortedEdgeGenesY);

            var range = sortedNodeGenesX.Last().ID < sortedNodeGenesY.Last().ID ? 
                sortedNodeGenesY.Last().ID : sortedNodeGenesX.Last().ID;
            var seen = new bool[range];

            var approvedNodeGenes = new List<NeuralChromosome.NodeGene>();
            var approvedEdgeGenes = new List<NeuralChromosome.EdgeGene>();

            var mostFitChromosome = x.Creature.Fitness < y.Creature.Fitness ? y : x;

            while(geneQueueX.Any() && geneQueueY.Any())
            {
                var geneX = geneQueueX.Peek();
                var geneY = geneQueueY.Peek();

                if(geneX.ID == geneY.ID)
                {
                    var selected = Stochastic.FlipCoin() ? geneX : geneY;
                    approvedEdgeGenes.Add(selected);

                    ApproveNodeGenes(selected, ref approvedNodeGenes, ref seen);

                    geneQueueX.Dequeue();
                    geneQueueY.Dequeue();
                }
                else if(geneX.ID < geneY.ID)
                {
                    if (mostFitChromosome == x)
                        approvedEdgeGenes.Add(geneX);

                    ApproveNodeGenes(geneX, ref approvedNodeGenes, ref seen);

                    geneQueueX.Dequeue();
                }
                else
                {
                    if (mostFitChromosome == y)
                        approvedEdgeGenes.Add(geneY);

                    ApproveNodeGenes(geneY, ref approvedNodeGenes, ref seen);

                    geneQueueY.Dequeue();
                }
            }

            var remainingQueue = geneQueueX.Any() ? geneQueueX : geneQueueY;
            var remainingChromosome = geneQueueX.Any() ? x : y;
            while(remainingQueue.Any())
            {
                var selected = remainingQueue.Dequeue();
                if (mostFitChromosome == remainingChromosome)
                    approvedEdgeGenes.Add(selected);

                ApproveNodeGenes(selected, ref approvedNodeGenes, ref seen);
            }

            return new NeuralChromosome(approvedNodeGenes, approvedEdgeGenes, x.InnovationTrackerName);
        }

        private IList<TGene> GetSortedGenes<TGene>(NeuralChromosome chromosome)
        {
            var clonedGenes = chromosome.CloneGenes();
            if(typeof(TGene) == typeof(NeuralChromosome.NodeGene))
            {
                var clonedNodeGenes = clonedGenes.Item1;
                return clonedNodeGenes.OrderByDescending(gene => gene.ID).Reverse() as IList<TGene>;
            }
            else if(typeof(TGene) == typeof(NeuralChromosome.EdgeGene))
            {
                var clonedEdgeGenes = clonedGenes.Item2;
                return clonedEdgeGenes.OrderByDescending(gene => gene.ID).Reverse() as IList<TGene>;
            }
            else
            {
                throw new ArgumentException($"Error! {typeof(TGene)} is not a valid neural gene.");
            }
                
        }

        private void ApproveNodeGenes(NeuralChromosome.EdgeGene selected, 
            ref List<NeuralChromosome.NodeGene> approvedNodeGenes, ref bool[] seen)
        {
            if (!seen[selected.Input.ID])
            {
                approvedNodeGenes.Add(selected.Input);
                seen[selected.Input.ID] = true;
            }
            if (!seen[selected.Output.ID])
            {
                approvedNodeGenes.Add(selected.Output);
                seen[selected.Output.ID] = true;
            }
        }
    }
}
