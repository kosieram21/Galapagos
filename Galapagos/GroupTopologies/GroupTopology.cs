using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.GroupTopologies
{
    internal abstract class GroupTopology : IEnumerable<Group>
    {
        protected readonly Group[] _groups;

        /// <summary>
        /// Constructs a new instance of the <see cref="GroupTopology"/> class.
        /// </summary>
        /// <param name="groups">The collection of groups in the topology.</param>
        internal GroupTopology(Group[] groups)
        {
            _groups = groups;
        }

        /// <summary>
        /// Accesses a group from the topology.
        /// </summary>
        /// <param name="index">The group index.</param>
        /// <returns>The group.</returns>
        public Group this[int index] => _groups[index];

        /// <summary>
        /// Gets the groups adjacent to the group at the given index.
        /// </summary>
        /// <param name="index">The group index.</param>
        /// <returns>The adjacent groups.</returns>
        public abstract Group[] GetNeighbors(int index);

        #region IEnumerable Members

        public IEnumerator<Group> GetEnumerator()
        {
            return _groups.ToArray().ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
