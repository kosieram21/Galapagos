using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Galapagos;

namespace Galapagos.UnitTests.Problems
{
    class NQueens
    {
        public const uint POPULATION_SIZE = 100;

        public readonly Population _population;

        public NQueens(uint boardSize)
        {
            var metatdata = new CreatureMetadata(creature => 
            {
                var ordering = creature.GetChromosome<PermutationChromosome>("ordering").Permutation;
                double fitness = 0;

                for(var i = 1; i < ordering.Count(); i++)
                {
                    for(var j = 1; j < i + 1; j++)
                    {
                        if ((ordering[i] != (ordering[i -j] - j)) &&
                            (ordering[i] != (ordering[i - j] + j)))
                            fitness++;
                    }
                }

                return fitness;
            });

            var orderingMetadata = new PermutationChromosomeMetadata("ordering", boardSize, 1, 0.25);
            orderingMetadata.AddCrossoverOperators(PermutationCrossover.Order);
            orderingMetadata.AddMutationOperators(PermutationMutation.Transposition, 2);
            orderingMetadata.AddMutationOperators(PermutationMutation.Randomization, 1);

            metatdata.Add(orderingMetadata);

            _population = new Population(POPULATION_SIZE, metatdata);
            _population.EnableLogging();

            _population.RegisterTerminationCondition(TerminationCondition.GenerationThreshold, 1000);
        }

        public Creature Solve()
        {
            _population.Evolve(SelectionAlgorithm.Tournament, 5, false, 0.10);
            return _population.OptimalCreature;
        }

        public static void PrintBoard(Creature creature)
        {
            var ordering = creature.GetChromosome<PermutationChromosome>("ordering").Permutation;
            for (var i = 0; i < ordering.Count(); i++)
            {
                var row = new StringBuilder();
                for(var j = 0; j < ordering.Count(); j++)
                {
                    if (ordering[i] == j)
                        row.Append("Q");
                    else
                        row.Append("*");
                }
                Debug.WriteLine(row.ToString());
            }
        }
    }
}
