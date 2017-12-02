using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.API.ANN
{
    public class Connection
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="source">The source neuron.</param>
        /// <param name="target">The target neuron.</param>
        /// <param name="weight">The connection weight.</param>
        internal Connection(Neuron source, Neuron target, double weight)
        {
            Source = source;
            Target = target;

            Weight = weight;

            Target.AddInputConnection(this);
        }

        /// <summary>
        /// Gets the source neuron of this connection.
        /// </summary>
        public Neuron Source { get; private set; }

        /// <summary>
        /// Gets the target neuron of this connection.
        /// </summary>
        public Neuron Target { get; private set; }

        /// <summary>
        /// Gets the connection weight.
        /// </summary>
        public double Weight { get; private set; }
    }
}
