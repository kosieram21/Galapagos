using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.API
{
    public interface IOperator
    {
        /// <summary>
        /// Gets the operator weight.
        /// </summary>
        double Weight { get; }
    }
}
