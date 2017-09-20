using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Galapagos;

namespace Genetic
{
    public class Sudoku
    {
        private readonly char[,] _initialBoard;
        private readonly IList<IList<char>> _unknownValues = new List<IList<char>>();

        private Population _population;

        private const int POPULATION_SIZE = 7500;

        private const uint FITNESS_THRESHOLD = 18;
        private const int PLATEAU_LENGTH = 100;
        private const int TOURNAMENT_SIZE = 5;

        private const bool ELITISM = true;
        private const double SURVIVAL_RATE = 0.33;

        private const double CROSSOVER_RATE = 1;
        private const PermutationCrossover CROSSOVER_OPERATORS = PermutationCrossover.AlternatingPosition | PermutationCrossover.Order;

        private const double MUTATION_RATE = 0.25;
        private const PermutationMutation MUTATION_OPERATORS = PermutationMutation.Transposition | PermutationMutation.Randomization;

        public Sudoku(char[,] initialBoard)
        {
            _initialBoard = initialBoard;
        }

        public uint[,] Solve()
        {
            while (true)
            {
                var solution = EvolveSolution();
                if (solution.Fitness >= FITNESS_THRESHOLD)
                    return ConstructBoard(solution);
            }
        }

        private Creature EvolveSolution()
        {
            var geneticDescription = ConstructGeneticDescription();
            _population = new Population(POPULATION_SIZE, geneticDescription);

            _population.EnableLogging();
            _population.RegisterTerminationCondition(TerminationCondition.FitnessThreshold, FITNESS_THRESHOLD);
            _population.RegisterTerminationCondition(TerminationCondition.FitnessPlateau, PLATEAU_LENGTH);
            _population.ParallelEvolve(SelectionAlgorithm.Tournament, TOURNAMENT_SIZE, ELITISM, SURVIVAL_RATE);

            return _population.OptimalCreature;
        }

        private GeneticDescription ConstructGeneticDescription()
        {
            var traits = ConstructTraits();
            var fitnessFunction = ConstructFitnessFunction();
            return new GeneticDescription(fitnessFunction, traits);
        }

        private IList<GeneticDescription.Trait> ConstructTraits()
        {
            var traits = new List<GeneticDescription.Trait>();

            _unknownValues.Clear();
            for (var i = 0; i < 9; i++)
            {
                _unknownValues.Add(new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9' });

                for (var j = 0; j < 9; j++)
                {
                    if (_initialBoard[i,j] != '?')
                        _unknownValues[i].Remove(_initialBoard[i,j]);
                }

                traits.Add(new GeneticDescription.PermutationTrait($"Row{i}", (uint)_unknownValues[i].Count, CROSSOVER_RATE, MUTATION_RATE, CROSSOVER_OPERATORS, MUTATION_OPERATORS));
            }

            return traits;
        }

        private Func<Creature, uint> ConstructFitnessFunction()
        {
            return (creature) =>
            {
                uint fitness = 0;

                var board = ConstructBoard(creature);
                for(var i = 0; i < 9; i++)
                {
                    var column = GetColumn(board, i);
                    if (EvaluateRegion(column))
                        fitness++;
                }

                for(var i = 0; i < 3; i++)
                {
                    for(var j = 0; j < 3; j++)
                    {
                        var box = GetBox(board, i, j);
                        if (EvaluateRegion(box))
                            fitness++;
                    }
                }

                return fitness;
            };
        }

        private uint[,] ConstructBoard(Creature creature)
        {
            var board = new uint[9, 9];

            for (var i = 0; i < 9; i++)
            {
                var permutation = creature.GetChromosome<PermutationChromosome>($"Row{i}").Permutation;
                var permutationIndex = 0;
                var row = new uint[9];

                for (var j = 0; j < 9; j++)
                {
                    if (_initialBoard[i,j] == '?')
                    {
                        var index = (int)permutation[permutationIndex];
                        board[i,j] = (uint)(_unknownValues[i][index] - '0');
                        permutationIndex++;
                    }
                    else
                    {
                        board[i,j] = (uint)(_initialBoard[i,j] - '0');
                    }
                }
            }

            return board;
        }

        private uint[] GetColumn(uint[,] board, int columnIndex)
        {
            var column = new uint[9];
            for(var i = 0; i < 9; i++)
                column[i] = board[i, columnIndex];
            return column;
        }

        private uint[] GetBox(uint[,] board, int rowOffset, int columnOffset)
        {
            var box = new uint[9];
            var index = 0;
            for (var i = 3 * rowOffset; i < 3 * rowOffset + 3; i++)
            {
                for (var j = 3 * columnOffset; j < 3 * columnOffset + 3; j++)
                {
                    box[index] = board[i, j];
                    index++;
                }
            }
            return box;
        }

        private bool EvaluateRegion(uint[] region)
        {
            var seen = new bool[9];

            for (var i = 0; i < 9; i++)
            {
                var number = region[i];
                seen[number - 1] = true;
            }

            return seen.All(o => o);
        }
    }
}
