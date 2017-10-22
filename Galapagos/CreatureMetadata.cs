using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos
{
    /// <summary>
    /// Metadata for a creature.
    /// </summary>
    public class CreatureMetadata : IEnumerable<ChromosomeMetadata>
    {
        private readonly IList<ChromosomeMetadata> _chromosomeMetadata = new List<ChromosomeMetadata>();
        private readonly Func<ICreature, double> _fitnessFunction;

        /// <summary>
        /// Constructs a new instance of the <see cref="CreatureMetadata"/> class.
        /// </summary>
        /// <param name="fitnessFunction">The fitness function.</param>
        /// <param name="chromosomeMetadata">The chromosome metadata.</param>
        public CreatureMetadata(Func<ICreature, double> fitnessFunction, IList<ChromosomeMetadata> chromosomeMetadata = null)
        {
            _fitnessFunction = fitnessFunction;
            if (chromosomeMetadata != null)
            {
                foreach (var metadata in chromosomeMetadata)
                    Add(metadata);
            }
        }

        /// <summary>
        /// Gets the fitness function.
        /// </summary>
        internal Func<ICreature, double> FitnessFunction => _fitnessFunction;

        /// <summary>
        /// Adds chromosome metadata to the genetic description.
        /// </summary>
        /// <param name="metadata">The metadata to add.</param>
        public void Add(ChromosomeMetadata metadata)
        {
            if (_chromosomeMetadata.Any(t => t.Name == metadata.Name))
                throw new ArgumentException($"Error! Chromosome metadata named {metadata.Name} already exists.");

            _chromosomeMetadata.Add(metadata);
        }

        /// <summary>
        /// Gets chromosome metadata from the description.
        /// </summary>
        /// <param name="i">The metadata index.</param>
        /// <returns>The metadata.</returns>
        public ChromosomeMetadata this[int i] => _chromosomeMetadata[i];

        #region IEnumerable Members

        public IEnumerator<ChromosomeMetadata> GetEnumerator()
        {
            return _chromosomeMetadata.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
