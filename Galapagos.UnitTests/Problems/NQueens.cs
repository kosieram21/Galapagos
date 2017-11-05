/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Galapagos;
using Galapagos.API;

namespace Galapagos.UnitTests.Problems
{
    class NQueens
    {
        private const uint POPULATION_SIZE = 100;
        private const uint GENERATION_THRESHOLD = 1000;
        private readonly int FITNESS_THRESHOLD;
        private readonly PopulationMetadata CREATURE_METADATA;

        public NQueens(uint boardSize)
        {
            FITNESS_THRESHOLD = GetFitnessThreshold(boardSize);

            CREATURE_METADATA = new PopulationMetadata(creature => 
            {
                var ordering = creature.GetChromosome<IPermutationChromosome>("ordering");
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

            CREATURE_METADATA.AddChromosomeMetadata(orderingMetadata);
        }

        public uint[] Solve()
        {
            while (true)
            {
                var solution = Evolve();
                if (solution.Fitness >= FITNESS_THRESHOLD)
                    return solution.GetChromosome<IPermutationChromosome>("ordering").ToArray();
            }
        }

        private ICreature Evolve()
        {
            var population = Population.Create(POPULATION_SIZE, CREATURE_METADATA);
            population.EnableLogging();

            //population.EnableNiches(50);

            population.RegisterTerminationCondition(TerminationCondition.GenerationThreshold, GENERATION_THRESHOLD);
            population.RegisterTerminationCondition(TerminationCondition.FitnessThreshold, FITNESS_THRESHOLD);

            population.Evolve(SelectionAlgorithm.Tournament, 5);

            return population.OptimalCreature;
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

        private int GetFitnessThreshold(uint size)
        {
            var sum = 0;
            for (var i = 1; i < size; i++)
                sum += i;
            return sum;
        }
    }
}*/
