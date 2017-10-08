using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    var index = i - 1;

                    while(index >= 0)
                    {
                        if (ordering[i] - 1 != index) //not attacking
                            fitness++;

                        index--;
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

        public void Solve()
        {
            _population.Evolve(SelectionAlgorithm.Tournament, 5, false, 0.10);
        }
    }
}
