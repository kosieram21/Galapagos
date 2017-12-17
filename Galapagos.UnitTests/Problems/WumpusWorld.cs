using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;
using Galapagos.API.ANN;
using System.Threading;

namespace Galapagos.UnitTests.Problems
{
    public class WumpusWorld
    {
        public const char START = 'S';
        public const char WUMPUS = 'W';
        public const char PIT = 'P';
        public const char GOLD = 'G';
        public const char EMPTY = '*';

        public const uint FORWWARD = 0;
        public const uint TURN_LEFT = 1;
        public const uint TURN_RIGHT = 2;
        public const uint GRAB = 3;
        public const uint SHOOT = 4;
        public const uint CLIMB = 5;

        public static string[] ACTION_MAP = new string[6]
        {
            "FORWWARD",
            "TURN_LEFT",
            "TURN_RIGHT",
            "GRAB",
            "SHOOT",
            "CLIMB"
        };

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

            public static readonly char[,] Board2 = new char[,]
            {
                { EMPTY, EMPTY, EMPTY, PIT },
                { WUMPUS, EMPTY, PIT, EMPTY },
                { EMPTY, EMPTY, EMPTY, EMPTY },
                { START, GOLD, PIT, EMPTY }
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

        private Tuple<int, int> _startPosition;

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

        public Tuple<int, int> StartPosition => _startPosition;

        public Tuple<int, int> Position => _position;

        public bool Done => _done;

        public double Reward => _reward;

        public bool GoldFound => _goldFound;

        public bool WumpusAlive => _wumpusAlive;

        public void Reset()
        {
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    if (_board[i, j] == START)
                    {
                        _position = new Tuple<int, int>(i, j);
                        _startPosition = new Tuple<int, int>(i, j);
                    }
                }
            }

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
            var percepts = new double[]
            {
                Stench(), Breeze(), Gliter(), Bump(), Scream(),
                //Arrow(), Gold(), Orientation(), PosX(), PosY()
            };
            return percepts;
        }

        public string GetPerceptsText()
        {
            var stench = Stench() == 1 ? "STENCH" : "NONE";
            var breeze = Breeze() == 1 ? "BREEZE" : "NONE";
            var gliter = Gliter() == 1 ? "GLITER" : "NONE";
            var bump = Bump() == 1 ? "BUMP" : "NONE";
            var scream = Scream() == 1 ? "SCREAM" : "NONE";

            return $"[{stench},{breeze},{gliter},{bump},{scream}]";
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

        public void Render()
        {
            var boarder = new StringBuilder();
            for (var i = 0; i < ((2 * N) + 2); i++)
                boarder.Append("-");

            System.Diagnostics.Debug.WriteLine(boarder);

            for (var i = 0; i < _board.GetLength(0); i++)
            {
                var sb = new StringBuilder().AppendFormat("-{0}", GetCharAtLoaction(i, 0));
                for (var j = 1; j < _board.GetLength(1); j++)
                    sb.AppendFormat(",{0}", GetCharAtLoaction(i, j));
                System.Diagnostics.Debug.WriteLine(sb.Append("-").ToString());
            }

            System.Diagnostics.Debug.WriteLine(boarder);
            System.Diagnostics.Debug.WriteLine("");
        }

        private char GetCharAtLoaction(int i, int j)
        {
            if (_position.Item1 == i && _position.Item2 == j)
                return 'A';
            return _board[i, j];
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

            if (_collision)
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

        private double Arrow()
        {
            double arrow = _arrowCount == 0 ? 0 : 1;
            return arrow;
        }

        private double Gold()
        {
            double gold = _goldFound ? 1 : 0;
            return gold;
        }

        private double Orientation()
        {
            double orientauin =
                _direction == Direction.NORTH ? 0 :
                _direction == Direction.EAST ? 1 :
                _direction == Direction.SOUTH ? 2 :
                _direction == Direction.WEST ? 3 :
                -1;

            return orientauin;
        }

        private double PosX()
        {
            return _position.Item1;
        }

        private double PosY()
        {
            return _position.Item2;
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
            if (_arrowCount != 0 && _wumpusAlive)
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

            if (_board[i, j] == START)
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
        private const uint MAX_ACTIONS = 20;

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

            var i = 0;
            var j = 0;
            var prev_action = -1;
            while (i < MAX_ACTIONS && !_environment.Done)
            {
                var percepts = _environment.GetPercepts();
                var output = nn.Evaluate(percepts);
                var action = (uint)Array.IndexOf(output, output.Max());

                if (action == prev_action)
                    j++;
                if (j > 5)
                    break;

                prev_action = (int)action;

                _environment.TakeAction(action);
                i++;
            }

            /*var bonus = !_environment.GoldFound ? -100 : 60 - (10 *
                (Math.Abs(_environment.Position.Item1 - _environment.StartPosition.Item1) +
                Math.Abs(_environment.Position.Item2 - _environment.StartPosition.Item2)));*/
            var bonus = 0;

            if (!_environment.WumpusAlive)
                bonus += 10;
            if (_environment.GoldFound)
                bonus += 150;


            //return _environment.Reward + i + bonus;
            return _environment.Reward + bonus;
        }
    }

    public class WumpusWorldAgent
    {
        private const uint MAX_ACTIONS = 50;
        private const int TIMEOUT = 250;

        private readonly NeuralNetwork _network;

        public WumpusWorldAgent(NeuralNetwork network)
        {
            _network = network;
        }

        public void Navigate(WumpusWorld environment)
        {
            environment.Reset();

            var i = 0;
            while (i < MAX_ACTIONS && !environment.Done)
            {
                var percepts = environment.GetPercepts();
                var output = _network.Evaluate(percepts);
                var action = (uint)Array.IndexOf(output, output.Max());

                System.Diagnostics.Debug.WriteLine($"Action Count: {i}");
                System.Diagnostics.Debug.WriteLine($"Percepts: {environment.GetPerceptsText()}");
                System.Diagnostics.Debug.WriteLine($"Action: {WumpusWorld.ACTION_MAP[action]}");
                environment.Render();

                environment.TakeAction(action);
                i++;

                Thread.Sleep(TIMEOUT);
            }
        }
    }
}
