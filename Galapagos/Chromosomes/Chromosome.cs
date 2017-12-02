using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.Chromosomes
{
    public abstract class Chromosome : IChromosome
    {
        /// <summary>
        /// Measures the distance between two chromosome.
        /// </summary>
        /// <param name="other">The other chromosome.</param>
        /// <returns>The distance between the chromosomes.</returns>
        public abstract double Distance(IChromosome other);

        /// <summary>
        /// Gets or sets the creature associated with this chromosome.
        /// </summary>
        public ICreature Creature { get; set; }
    }
}
