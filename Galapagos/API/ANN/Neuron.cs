using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.API.ANN
{
    public class Neuron
    {
        private readonly IList<Connection> _inputs = new List<Connection>();

        /// <summary>
        /// Constructs a new instance of the <see cref="Neuron"/> class.
        /// </summary>
        /// <param name="id">The neuron Id.</param>
        internal Neuron(uint id)
        {
            ID = id;
            Value = 0;
        }

        /// <summary>
        /// Gets or sets the neuron ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// Gets or sets the value of this neuron.
        /// </summary>
        public double Value { get; internal set; }

        /// <summary>
        /// Gets the input for this neuron.
        /// </summary>
        public IReadOnlyList<Connection> Inputs => _inputs as IReadOnlyList<Connection>;

        /// <summary>
        /// Adds a new connection to this neuron's inputs.
        /// </summary>
        /// <param name="input">The input connection.</param>
        internal void AddInputConnection(Connection input)
        {
            _inputs.Add(input);
        }
    }
}
