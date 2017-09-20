using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.TerminationConditions
{
    /// <summary>
    /// A termination condition for population evolution.
    /// </summary>
    public interface ITerminationCondition
    {
        /// <summary>
        /// Checks the termination condition.
        /// </summary>
        /// <returns>A value indicating if evolution should terminate.</returns>
        bool Check();
    }
}
