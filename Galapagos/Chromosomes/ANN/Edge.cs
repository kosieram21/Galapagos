using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.Chromosomes.ANN
{
    internal class Edge
    {
        private readonly Node _source;
        private readonly Node _target;

        private double _weight;

        /// <summary>
        /// Constructs a new instance of the <see cref="Edge"/> class.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="target">The target node.</param>
        /// <param name="weight">The edge weight.</param>
        internal Edge(Node source, Node target, double weight)
        {
            _source = source;
            _target = target;

            _weight = weight;

            _target.AddInputEdge(this);
        }

        /// <summary>
        /// Gets the source node of this edge.
        /// </summary>
        public Node Source => _source;

        /// <summary>
        /// Gets the target node of this edge.
        /// </summary>
        public Node Target => _target;

        /// <summary>
        /// Gets the edge weight.
        /// </summary>
        public double Weight => _weight;
    }
}
