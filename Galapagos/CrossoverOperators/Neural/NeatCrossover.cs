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
            if (x.InnovationTrackerName != y.InnovationTrackerName)
                throw new ArgumentException("Error! Incompatible chromosomes.");

            var geneQueueX = GetGeneQueue(x);
            var geneQueueY = GetGeneQueue(y);

            var seen = InitializeSeenTracker(x, y);

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
                    if (x.Creature.Fitness == y.Creature.Fitness && Stochastic.FlipCoin())
                    {
                        approvedEdgeGenes.Add(geneX);
                        ApproveNodeGenes(geneX, ref approvedNodeGenes, ref seen);
                    }
                    else if (mostFitChromosome == x)
                    {
                        approvedEdgeGenes.Add(geneX);
                        ApproveNodeGenes(geneX, ref approvedNodeGenes, ref seen);
                    }
                    
                    geneQueueX.Dequeue();
                }
                else
                {
                    if (x.Creature.Fitness == y.Creature.Fitness && Stochastic.FlipCoin())
                    {
                        approvedEdgeGenes.Add(geneY);
                        ApproveNodeGenes(geneX, ref approvedNodeGenes, ref seen);
                    }
                    else if (mostFitChromosome == y)
                    {
                        approvedEdgeGenes.Add(geneY);
                        ApproveNodeGenes(geneY, ref approvedNodeGenes, ref seen);
                    }

                    geneQueueY.Dequeue();
                }
            }

            var remainingQueue = geneQueueX.Any() ? geneQueueX : geneQueueY;
            var remainingChromosome = geneQueueX.Any() ? x : y;
            while(remainingQueue.Any())
            {
                var selected = remainingQueue.Dequeue();
                if (x.Creature.Fitness == y.Creature.Fitness && Stochastic.FlipCoin())
                {
                    approvedEdgeGenes.Add(selected);
                    ApproveNodeGenes(selected, ref approvedNodeGenes, ref seen);
                }
                else if (mostFitChromosome == remainingChromosome)
                {
                    approvedEdgeGenes.Add(selected);
                    ApproveNodeGenes(selected, ref approvedNodeGenes, ref seen);
                }
            }

            return new NeuralChromosome(approvedNodeGenes, approvedEdgeGenes, x.InnovationTrackerName, x.C1, x.C2, x.C3);
        }

        /// <summary>
        /// Gets a queue of edge genes sorted by ID for the given chromosome.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The gene queue.</returns>
        private Queue<NeuralChromosome.EdgeGene> GetGeneQueue(NeuralChromosome chromosome)
        {
            var genes = chromosome.CloneGenes();
            var edgeGenes = genes.Item2;

            var sortedGenes = edgeGenes.OrderByDescending(gene => gene.ID).Reverse();
            var geneQueue = new Queue<NeuralChromosome.EdgeGene>(sortedGenes);

            return geneQueue;
        }

        /// <summary>
        /// Initializes the node gene seen tracker.
        /// </summary>
        /// <param name="x">The x parent.</param>
        /// <param name="y">The y parent.</param>
        /// <returns>The initialized seen tracker.</returns>
        private bool[] InitializeSeenTracker(NeuralChromosome x, NeuralChromosome y)
        {
            var maxX = x.NodeGenes.Max(gene => gene.ID);
            var maxY = y.NodeGenes.Max(gene => gene.ID);
            var range = maxX < maxY ? maxY : maxX;
            var seen = new bool[range + 1];

            return seen;
        }

        /// <summary>
        /// Approves a node gene for inclusion in the child chromosome.
        /// </summary>
        /// <param name="selected">The selected edge gene.</param>
        /// <param name="approvedNodeGenes">The approved node genes.</param>
        /// <param name="seen">The node gene seen tracker.</param>
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
