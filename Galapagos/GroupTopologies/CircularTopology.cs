using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.GroupTopologies
{
    internal class CircularTopology : GroupTopology
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="CircularTopology"/> class.
        /// </summary>
        /// <param name="groups">The collection of groups in the topology.</param>
        internal CircularTopology(Group[] groups)
            : base(groups) { }

        /// <summary>
        /// Gets the groups adjacent to the group at the given index.
        /// </summary>
        /// <param name="index">The group index.</param>
        /// <returns>The adjacent groups.</returns>
        public override Group[] GetNeighbors(int index)
        {
            var size = _groups.Length;
            return new Group[]
            {
                _groups[(index - 1) % size],
                _groups[(index + 1) % size]
            };
        }
    }
}
