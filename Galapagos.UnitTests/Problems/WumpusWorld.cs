using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;
using Galapagos.API.ANN;

namespace Galapagos.UnitTests.Problems
{
    public class WumpusWorld
    {
        public const char START = 'S';
        public const char WUMPUS = 'W';
        public const char PIT = 'P';
        public const char GOLD = 'G';
        public const char EMPTY = 'E';

        public const uint FORWWARD = 0;
        public const uint TURN_LEFT = 1;
        public const uint TURN_RIGHT = 2;
        public const uint GRAB = 3;
        public const uint SHOOT = 4;
        public const uint CLIMB = 5;

        #region Boards

        public static class Boards
        {
            public static readonly char[,] Board1 = new char[,]
                {
                { EMPTY, EMPTY, EMPTY, PIT },
                { WUMPUS, GOLD, PIT, EMPTY },
                { EMPTY, EMPTY, EMPTY, EMPTY },
                { START, EMPTY, PIT, EMPTY }
                };
        }

        #endregion

        private enum Direction
        {
            NORTH,
            SOUTH,
            EAST,
            WEST
        }

        private readonly char[,] _board;
        private readonly int N;
        private readonly int M;

        private Tuple<int, int> _position;
        private Direction _direction;
        private int _arrowCount;
        private double _reward;

        private bool _wumpusAlive;
        private bool _goldFound;
        private bool _collision;
        private bool _done;

        public WumpusWorld(char[,] board)
        {
            _board = board;
            N = _board.GetLength(0);
            M = _board.GetLength(1);

            Reset();
        }

        public bool Done => _done;

        public double Reward => _reward;

        public void Reset()
        {
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    if (_board[i, j] == START)
                    {
                        _position = new Tuple<int, int>(i, j);
                        goto END;
                    }
                }
            }

        END:
            _direction = Direction.EAST;
            _arrowCount = 1;
            _reward = 0;

            _wumpusAlive = true;
            _goldFound = false;
            _collision = false;
            _done = false;
        }

        public double[] GetPercepts()
        {
            var percepts = new double[] { Stench(), Breeze(), Gliter(), Bump(), Scream() };
            return percepts;
        }

        public void TakeAction(uint action)
        {
            if (!_done)
            {
                switch (action)
                {
                    case FORWWARD:
                        Forward();
                        break;
                    case TURN_LEFT:
                        TurnLeft();
                        break;
                    case TURN_RIGHT:
                        TurnRight();
                        break;
                    case GRAB:
                        Grab();
                        break;
                    case SHOOT:
                        Shoot();
                        break;
                    case CLIMB:
                        Climb();
                        break;
                    default:
                        throw new ArgumentException($"Error! {action} is not a valid action. Action must be a value in the range [0, 5].");
                }

                _reward--;
            }
        }

        #region Percept Helpers

        private double Stench()
        {
            var stench = AdjacentToGameObject(WUMPUS);
            return stench;
        }

        private double Breeze()
        {
            var breeze = AdjacentToGameObject(PIT);
            return breeze;
        }

        private double Gliter()
        {
            var gliter = AdjacentToGameObject(GOLD);
            return gliter;
        }

        private double Bump()
        {
            double bump = 0;

            if(_collision)
            {
                bump = 1;
                _collision = false;
            }

            return bump;
        }

        private double Scream()
        {
            double scream = _wumpusAlive ? 0 : 1;
            return scream;
        }

        private double AdjacentToGameObject(char obj)
        {
            var i = _position.Item1;
            var j = _position.Item2;

            if (i != 0 && _board[i - 1, j] == obj)
                return 1;
            if (i != (N - 1) && _board[i + 1, j] == obj)
                return 1;
            if (j != 0 && _board[i, j - 1] == obj)
                return 1;
            if (j != (M - 1) && _board[i, j + 1] == obj)
                return 1;

            return 0;
        }

        #endregion

        #region Action Helpers

        private void Forward()
        {
            var i = _position.Item1;
            var j = _position.Item2;

            switch (_direction)
            {
                case Direction.NORTH:
                    if (i == 0)
                        _collision = true;
                    else
                        i--;
                    break;
                case Direction.SOUTH:
                    if (i == (N - 1))
                        _collision = true;
                    else
                        i++;
                    break;
                case Direction.EAST:
                    if (j == (M - 1))
                        _collision = true;
                    else
                        j++;
                    break;
                case Direction.WEST:
                    if (j == 0)
                        _collision = true;
                    else
                        j--;
                    break;
            }

            _position = new Tuple<int, int>(i, j);
            _done = (_board[i, j] == WUMPUS || _board[i, j] == PIT);

            if (_done)
                _reward -= 1000;
        }

        private void TurnLeft()
        {
            var directions = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };
            var index = Array.IndexOf(directions, _direction);

            _direction = directions[mod(index - 1, 4)];
        }

        private void TurnRight()
        {
            var directions = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };
            var index = Array.IndexOf(directions, _direction);

            _direction = directions[mod(index + 1, 4)];
        }

        private void Grab()
        {
            var i = _position.Item1;
            var j = _position.Item2;

            if (!_goldFound && _board[i, j] == GOLD)
                _goldFound = true;
        }

        private void Shoot()
        {
            if(_arrowCount != 0 && _wumpusAlive)
            {
                var i = _position.Item1;
                var j = _position.Item2;

                switch (_direction)
                {
                    case Direction.NORTH:
                        for (var k = i; k >= 0; k--)
                            if (_board[k, j] == WUMPUS)
                                _wumpusAlive = false;
                        break;
                    case Direction.SOUTH:
                        for (var k = i; k < N; k++)
                            if (_board[k, j] == WUMPUS)
                                _wumpusAlive = false;
                        break;
                    case Direction.EAST:
                        for (var k = j; k < N; k++)
                            if (_board[i, k] == WUMPUS)
                                _wumpusAlive = false;
                        break;
                    case Direction.WEST:
                        for (var k = j; k >= 0; k--)
                            if (_board[i, k] == WUMPUS)
                                _wumpusAlive = false;
                        break;
                }

                _arrowCount--;
                _reward -= 10;
            }
        }

        private void Climb()
        {
            var i = _position.Item1;
            var j = _position.Item2;

            if(_board[i, j] == START)
            {
                _done = true;

                if (_goldFound)
                    _reward += 1000;
            }
        }

        private int mod(int a, int n)
        {
            return ((a % n) + n) % n;
        }

        #endregion
    }

    public class WumpusWorldTrainer
    {
        private const uint EPISODE_LENGTH = 100;

        private readonly WumpusWorld _environment;

        public WumpusWorldTrainer(WumpusWorld environment)
        {
            _environment = environment;
        }

        public NeuralNetwork Train()
        {
            var metadata = Session.Instance.LoadMetadata(Metadata.WumpusWorld, FitnessFunction);
            var population = Session.Instance.CreatePopulation(metadata);

            population.EnableLogging();
            population.Evolve();

            return population.OptimalCreature.GetChromosome<INeuralChromosome>("nn").ToNeuralNetwork();
        }

        private double FitnessFunction(ICreature creature)
        {
            var nn = creature.GetChromosome<INeuralChromosome>("nn");

            _environment.Reset();

            var episode = 0;
            while(episode < EPISODE_LENGTH && !_environment.Done)
            {
                var percepts = _environment.GetPercepts();
                var output = nn.Evaluate(percepts);
                var action = (uint)Array.IndexOf(output, output.Max());

                _environment.TakeAction(action);
                episode++;
            }

            return _environment.Reward;
        }
    }
}
