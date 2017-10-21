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
        private readonly CreatureMetadata CREATURE_METADATA;

        public NQueens(uint boardSize)
        {
            FitnessThreshold = GetFitnessThreshold(boardSize);

            CREATURE_METADATA = new CreatureMetadata(creature => 
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
            orderingMetadata.AddMutationOperators(PermutationMutation.Transposition | PermutationMutation.CyclicShift, 2);
            orderingMetadata.AddMutationOperators(PermutationMutation.Randomization, 1);

            CREATURE_METADATA.Add(orderingMetadata);
        }

        public int FitnessThreshold { get; private set; }

        public Creature Solve()
        {
            while (true)
            {
                var solution = Evolve();
                if (solution.Fitness >= FitnessThreshold)
                    return solution;
            }
        }

        private Creature Evolve()
        {
            var population = new Population(POPULATION_SIZE, CREATURE_METADATA);
            population.EnableLogging();

            //population.EnableNiches(50);

            population.RegisterTerminationCondition(TerminationCondition.GenerationThreshold, 1000);
            population.RegisterTerminationCondition(TerminationCondition.FitnessThreshold, FitnessThreshold);

            population.Evolve(SelectionAlgorithm.Tournament, 5, false, 0.10);

            return population.OptimalCreature;
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

        private int GetFitnessThreshold(uint size)
        {
            var sum = 0;
            for (var i = 1; i < size; i++)
                sum += i;
            return sum;
        }
    }
}
