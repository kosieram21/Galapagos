using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos
{
    public interface IOperator
    {
        /// <summary>
        /// Gets the operator weight.
        /// </summary>
        uint Weight { get; }
    }
}
