﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.API
{
    public interface IChromosomeMetadata
    {
        /// <summary>
        /// The chromosome name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The chromosome type.
        /// </summary>
        ChromosomeType Type { get;  }

        /// <summary>
        /// The number of genes in the chromosome.
        /// </summary>
        uint GeneCount { get; set; }

        /// <summary>
        /// The crossover rate.
        /// </summary>
        double CrossoverRate { get; set; }

        /// <summary>
        /// The mutation rate.
        /// </summary>
        double MutationRate { get; set; }

        /// <summary>
        /// Gets a crossover from the metadata.
        /// </summary>
        /// <returns>The crossover.</returns>
        ICrossover GetCrossover();

        /// <summary>
        /// Gets a mutation from the metadata.
        /// </summary>
        /// <returns>The mutation.</returns>
        IMutation GetMutation();
    }
}