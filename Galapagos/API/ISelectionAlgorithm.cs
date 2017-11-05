using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.API
{
    /// <summary>
    /// A creature selection algorithm.
    /// </summary>
    public interface ISelectionAlgorithm
    {
        /// <summary>
        /// Initializes the selection algorithm.
        /// </summary>
        /// <param name="population">The population to select from.</param>
        void Initialize(IPopulation population);

        /// <summary>
        /// Invokes the selection algorithm.
        /// </summary>
        /// <returns>The selected creature.</returns>
        ICreature Invoke();
    }
}
