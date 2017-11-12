using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Galapagos.API;

namespace Galapagos.UnitTests.Problems
{
    class NQueens
    {
        public uint[] Solve()
        {
            var metadata = Session.Instance.LoadMetadata(Metadata.NQueens, FitnessFunction);
            var population = Session.Instance.CreatePopulation(metadata);

            population.EnableLogging();
            population.Evolve();

            return population.OptimalCreature.GetChromosome<IPermutationChromosome>("ordering").ToArray();
        }

        private double FitnessFunction(ICreature creature)
        {
            var ordering = creature.GetChromosome<IPermutationChromosome>("ordering");
            double fitness = 0;

            for (var i = 1; i < ordering.Count(); i++)
            {
                for (var j = 1; j < i + 1; j++)
                {
                    if ((ordering[i] != (ordering[i - j] - j)) &&
                        (ordering[i] != (ordering[i - j] + j)))
                        fitness++;
                }
            }

            return fitness;
        }

        public static void PrintBoard(uint[] board)
        {
            for (var i = 0; i < board.Count(); i++)
            {
                var row = new StringBuilder();
                for(var j = 0; j < board.Count(); j++)
                {
                    if (board[i] == j)
                        row.Append("Q");
                    else
                        row.Append("*");
                }
                Debug.WriteLine(row.ToString());
            }
        }
    }
}
