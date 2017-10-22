using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.API
{
    public interface ICreature
    {
        /// <summary>
        /// Gets the creature's fitness.
        /// </summary>
        double Fitness { get; }

        /// <summary>
        /// Gets the chromosome corresponding to the given name.
        /// </summary>
        /// <typeparam name="TChromosomeType">The chromosome type.</typeparam>
        /// <param name="name">The chromosome name.</param>
        /// <returns>The chromosome.</returns>
        TChromosomeType GetChromosome<TChromosomeType>(string name)
            where TChromosomeType : class, IChromosome;
    }
}
