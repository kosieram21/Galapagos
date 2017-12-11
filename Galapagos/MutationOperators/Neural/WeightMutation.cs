using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;
using Galapagos.API;

namespace Galapagos.MutationOperators.Neural
{
    public class WeightMutation : Mutation<NeuralChromosome>
    {
        private const double STEP_SIZE = 0.1;

        /// <summary>
        /// Constructs a new instance of the <see cref="WeightMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal WeightMutation(double weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Internal invocation the mutation operator.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The new DNA.</returns>
        protected override IChromosome InternalInvoke(NeuralChromosome chromosome)
        {
            var genes = chromosome.CloneGenes();
            var nodeGenes = genes.Item1;
            var edgeGenes = genes.Item2;

            //No edge genes in the genotype yet. Return original chromosome.
            if (edgeGenes.Count == 0)
                return new NeuralChromosome(nodeGenes, edgeGenes, chromosome.InnovationTrackerName,
                        chromosome.C1, chromosome.C2, chromosome.C3);

            var index = Stochastic.Next(edgeGenes.Count);
            var selectedEdge = edgeGenes[index];

            selectedEdge.Weight = selectedEdge.Weight + (Stochastic.NextDouble() * STEP_SIZE * 2) - STEP_SIZE;

            return new NeuralChromosome(nodeGenes, edgeGenes, chromosome.InnovationTrackerName,
                chromosome.C1, chromosome.C2, chromosome.C3);
        }
    }
}
