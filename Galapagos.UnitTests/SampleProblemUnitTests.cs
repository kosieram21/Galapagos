using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;
using Galapagos.UnitTests.Problems;

namespace Galapagos.UnitTests
{
    [TestClass]
    public class SampleProblemUnitTests
    {
        private const int WAIT_TIME = 5000;

        [TestMethod]
        public void SudokuTest()
        {
            var puzzle = new Sudoku(Sudoku.Boards.Board1);
            var solution = puzzle.Solve();
            Sudoku.PrintBoard(solution);

            Thread.Sleep(WAIT_TIME);
        }

        [TestMethod]
        public void NQueensTest()
        {
            var puzzle = new NQueens();
            var solution = puzzle.Solve();
            NQueens.PrintBoard(solution);

            Thread.Sleep(WAIT_TIME);
        }

        [TestMethod]
        public void CryptarithmeticTest()
        {
            var puzzle = new Cryptarithmetic();
            var solution = puzzle.Solve();
            Cryptarithmetic.PrintMapping(solution);

            Thread.Sleep(WAIT_TIME * 100);
        }
    }
}
