﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;
using Galapagos.API;

namespace Galapagos.MutationOperators.Neural
{
    public class EnableDisableMutation : Mutation<NeuralChromosome>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="EnableDisableMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        internal EnableDisableMutation(double weigth = 1)
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
                        chromosome.C1, chromosome.C2, chromosome.C3, chromosome.ActivationFunction);

            var index = Stochastic.Next(edgeGenes.Count);
            var selectedEdge = edgeGenes[index];

            selectedEdge.Enabled = !selectedEdge.Enabled;

            return new NeuralChromosome(nodeGenes, edgeGenes, chromosome.InnovationTrackerName,
                chromosome.C1, chromosome.C2, chromosome.C3, chromosome.ActivationFunction);
        }
    }
}
