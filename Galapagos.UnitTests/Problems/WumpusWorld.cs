using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.UnitTests.Problems
{
    public class WumpusWorld
    {
        #region Boards

        private const char START = 'S';
        private const char WUMPUS = 'W';
        private const char PIT = 'P';
        private const char GOLD = 'G';
        private const char EMPTY = 'E';

        public static readonly char[,] Board1 = new char[,]
            {
                { EMPTY, EMPTY, EMPTY, PIT },
                { WUMPUS, GOLD, PIT, EMPTY },
                { EMPTY, EMPTY, EMPTY, EMPTY },
                { START, EMPTY, PIT, EMPTY }
            };

        #endregion
    }
}
