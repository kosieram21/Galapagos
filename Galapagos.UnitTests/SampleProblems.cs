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

            Thread.Sleep(5000);
        }

        [TestMethod]
        public void EquationTest()
        {
            Equation.Optimize();

            Thread.Sleep(5000);
        }
    }
}
