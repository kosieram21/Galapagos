using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;
using Galapagos.UnitTests.Problems;

namespace Galapagos.UnitTests
{
    [TestClass]
    public class SampleProblems
    {
        private const int WAIT_TIME = 5000;

        [TestMethod]
        public void SudokuTest()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var puzzle = new Sudoku(Sudoku.Boards.Board1);
            var solution = puzzle.Solve();
            Sudoku.LogSolution(solution);

            stopwatch.Stop();
            Debug.WriteLine($"Runtime: {stopwatch.Elapsed.Minutes} minutes, {stopwatch.Elapsed.Seconds} seconds, {stopwatch.Elapsed.Milliseconds} milliseconds");

            Thread.Sleep(WAIT_TIME);
        }

        [TestMethod]
        public void NQueensTest()
        {
            var puzzle = new NQueens(8);
            puzzle.Solve();

            Thread.Sleep(WAIT_TIME);
        }

        [TestMethod]
        public void EquationTest()
        {
            Equation.Optimize();

            Thread.Sleep(WAIT_TIME);
        }
    }
}
