using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.Chromosomes.ANN
{
    internal class Node
    {
        private readonly IList<Edge> _inputs = new List<Edge>();

        /// <summary>
        /// Constructs a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="id">The node Id.</param>
        internal Node(uint id)
        {
            ID = id;
            Value = 0;
        }

        /// <summary>
        /// Gets or sets the node ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// Gets or sets the value of this node.
        /// </summary>
        public double Value { get; internal set; }

        /// <summary>
        /// Gets the input for this node.
        /// </summary>
        public IReadOnlyList<Edge> Inputs => _inputs as IReadOnlyList<Edge>;

        /// <summary>
        /// Adds a new edge to this nodes inputs.
        /// </summary>
        /// <param name="input">The input edge.</param>
        internal void AddInputEdge(Edge input)
        {
            _inputs.Add(input);
        }
    }
}
