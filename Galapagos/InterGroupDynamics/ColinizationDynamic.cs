using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;
using Galapagos.GroupTopologies;

namespace Galapagos.InterGroupDynamics
{
    internal class ColinizationDynamic
    {
        /// <summary>
        /// Invocation of the inter group dynamic.
        /// </summary>
        /// <param name="groupTopology">The group topology.</param>
        public void Invoke(GroupTopologies.GroupTopology groupTopology)
        {
            var indices = Selection(groupTopology);
            var best = indices[0]; var worst = indices[1];

            var colonist = groupTopology[best];
            var pool = Reproduction(colonist);

            var extinct = groupTopology[worst];
            Colinization(pool, colonist, extinct);
        }

        /// <summary>
        /// Selects the 'best' and 'worst' group from the topology in terms of fitness.
        /// </summary>
        /// <param name="groupTopology">The group topology.</param>
        /// <returns>The indices of the 'best' and 'worst' group.</returns>
        private int[] Selection(GroupTopologies.GroupTopology groupTopology)
        {
            var best = 0;
            var worst = 0;
            for(var i = 0; i < groupTopology.Count(); i++)
            {
                if (groupTopology[i].OptimalCreature.Fitness >
                    groupTopology[best].OptimalCreature.Fitness)
                    best = i;

                if (groupTopology[i].OptimalCreature.Fitness <
                    groupTopology[worst].OptimalCreature.Fitness)
                    worst = i;
            }

            return new int[] { best, worst };
        }

        /// <summary>
        /// Use individuals in the colonist group to create a pool
        /// of creatures consisting of the current generation and the
        /// next generation of creatures.
        /// </summary>
        /// <param name="colonist">The colonist group.</param>
        /// <returns>The pool of creatures.</returns>
        private Creature[] Reproduction(Group colonist)
        {
            var currentGen = colonist.Creatures;
            var nextGen = colonist.BreedNewGeneration();
            var pool = Session.Instance.Stochastic.Shuffle(currentGen, nextGen);
            return pool;
        }

        /// <summary>
        /// Individuals from the pool replace the populations of 
        /// the colonist and the extinct groups.
        /// </summary>
        /// <param name="pool">The pool of creatures.</param>
        /// <param name="colonist">The colonist group.</param>
        /// <param name="extinct">The extinct group.</param>
        public void Colinization(Creature[] pool, Group colonist, Group extinct)
        {
            // the front of the pool will be used as the individuals of the colonist group
            var front = pool.Take(pool.Length / 2).ToArray();
            colonist.AssignCreatures(front);

            // the back of the pool will be used as the individuals of the old colonist group
            var back = pool.Skip(pool.Length / 2).ToArray();
            extinct.AssignCreatures(back);
        }
    }
}
