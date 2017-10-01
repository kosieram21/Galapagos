using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Galapagos;

namespace Galapagos.UnitTests.Problems
{
    public class Sudoku
    {
        #region Boards

        public static class Boards
        {
            public static readonly char[,] Board1 = new char[,]
            {
                { '2', '8', '9', '?', '?', '7', '3', '?', '?' },
                { '?', '7', '6', '?', '?', '?', '?', '?', '?' },
                { '?', '?', '?', '1', '6', '?', '9', '?', '?' },
                { '5', '?', '1', '2', '?', '?', '8', '9', '?' },
                { '4', '?', '?', '8', '3', '6', '?', '?', '5' },
                { '?', '6', '2', '?', '?', '5', '4', '?', '7' },
                { '?', '?', '8', '?', '2', '9', '?', '?', '?' },
                { '?', '?', '?', '?', '?', '?', '6', '4', '?' },
                { '?', '?', '5', '4', '?', '?', '7', '2', '9' }
            };

            public static readonly char[,] Board2 = new char[,]
            {
                { '?', '?', '4', '8', '?', '?', '?', '1', '7' },
                { '6', '7', '?', '9', '?', '?', '?', '?', '?' },
                { '5', '?', '8', '?', '3', '?', '?', '?', '4' },
                { '3', '?', '?', '7', '4', '?', '1', '?', '?' },
                { '?', '6', '9', '?', '?', '?', '7', '8', '?' },
                { '?', '?', '1', '?', '6', '9', '?', '?', '5' },
                { '1', '?', '?', '?', '8', '?', '3', '?', '6' },
                { '?', '?', '?', '?', '?', '6', '?', '9', '1' },
                { '2', '4', '?', '?', '?', '1', '5', '?', '?' }
            };

            public static readonly char[,] Board3 = new char[,]
            {
                { '?', '?', '?', '?', '?', '?', '?', '?', '?' },
                { '?', '?', '?', '?', '?', '?', '?', '?', '?' },
                { '?', '?', '?', '?', '?', '?', '?', '?', '?' },
                { '?', '?', '?', '?', '?', '?', '?', '?', '?' },
                { '?', '?', '?', '?', '?', '?', '?', '?', '?' },
                { '?', '?', '?', '?', '?', '?', '?', '?', '?' },
                { '?', '?', '?', '?', '?', '?', '?', '?', '?' },
                { '?', '?', '?', '?', '?', '?', '?', '?', '?' },
                { '?', '?', '?', '?', '?', '?', '?', '?', '?' }
            };

            public static readonly char[,] Board4 = new char[,]
            {
                { '3', '4', '?', '?', '?', '8', '7', '?', '2' },
                { '?', '?', '?', '?', '?', '?', '6', '?', '?' },
                { '?', '?', '?', '9', '?', '2', '4', '?', '5' },
                { '6', '?', '?', '8', '?', '?', '3', '?', '?' },
                { '9', '?', '?', '4', '2', '1', '?', '?', '8' },
                { '?', '?', '2', '?', '?', '3', '?', '?', '9' },
                { '4', '?', '1', '3', '?', '9', '?', '?', '?' },
                { '?', '?', '6', '?', '?', '?', '?', '?', '?' },
                { '5', '?', '3', '2', '?', '?', '?', '1', '4' }
            };

            public static readonly char[,] Board5 = new char[,]
            {
                { '8', '?', '?', '?', '?', '?', '?', '?', '?' },
                { '?', '?', '3', '6', '?', '?', '?', '?', '?' },
                { '?', '7', '?', '?', '9', '?', '2', '?', '?' },
                { '?', '5', '?', '?', '?', '7', '?', '?', '?' },
                { '?', '?', '?', '?', '4', '5', '7', '?', '?' },
                { '?', '?', '?', '1', '?', '?', '?', '3', '?' },
                { '?', '?', '1', '?', '?', '?', '?', '6', '8' },
                { '?', '?', '8', '5', '?', '?', '?', '1', '?' },
                { '?', '9', '?', '?', '?', '?', '4', '?', '?' }
            };
        }

        #endregion

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
            var geneticDescription = ConstructCreatureMetadata();
            _population = new Population(POPULATION_SIZE, geneticDescription);

            _population.RegisterTerminationCondition(TerminationCondition.FitnessThreshold, FITNESS_THRESHOLD);
            _population.RegisterTerminationCondition(TerminationCondition.FitnessPlateau, PLATEAU_LENGTH);
            _population.ParallelEvolve(SelectionAlgorithm.Tournament, TOURNAMENT_SIZE, ELITISM, SURVIVAL_RATE);

            return _population.OptimalCreature;
        }

        private CreatureMetadata ConstructCreatureMetadata()
        {
            var chromosomeMetadata = ConstructChromosomeMetadata();
            var fitnessFunction = ConstructFitnessFunction();
            return new CreatureMetadata(fitnessFunction, chromosomeMetadata);
        }

        private IList<ChromosomeMetadata> ConstructChromosomeMetadata()
        {
            var metadata = new List<ChromosomeMetadata>();

            _unknownValues.Clear();
            for (var i = 0; i < 9; i++)
            {
                _unknownValues.Add(new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9' });

                for (var j = 0; j < 9; j++)
                {
                    if (_initialBoard[i, j] != '?')
                        _unknownValues[i].Remove(_initialBoard[i, j]);
                }

                metadata.Add(new ChromosomeMetadata($"Row{i}", (uint)_unknownValues[i].Count, ChromosomeType.Permutation, CROSSOVER_RATE, MUTATION_RATE, CROSSOVER_OPERATORS, MUTATION_OPERATORS));
            }

            return metadata;
        }

        private Func<Creature, double> ConstructFitnessFunction()
        {
            return (creature) =>
            {
                uint fitness = 0;

                var board = ConstructBoard(creature);
                for (var i = 0; i < 9; i++)
                {
                    var column = GetColumn(board, i);
                    if (EvaluateRegion(column))
                        fitness++;
                }

                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < 3; j++)
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
                    if (_initialBoard[i, j] == '?')
                    {
                        var index = (int)permutation[permutationIndex];
                        board[i, j] = (uint)(_unknownValues[i][index] - '0');
                        permutationIndex++;
                    }
                    else
                    {
                        board[i, j] = (uint)(_initialBoard[i, j] - '0');
                    }
                }
            }

            return board;
        }

        private uint[] GetColumn(uint[,] board, int columnIndex)
        {
            var column = new uint[9];
            for (var i = 0; i < 9; i++)
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
