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
            uint size = 13;
            var puzzle = new NQueens(size);
            puzzle.Solve();

            var sum = 0;
            for (var i = 1; i < size; i++)
                sum += i;

            Debug.WriteLine($"Best Possible Fitness {sum}.");

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
