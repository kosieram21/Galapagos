using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.UnitTests.Problems;
using System.Diagnostics;

namespace Galapagos.UnitTests
{
    [TestClass]
    public class SampleProblems
    {
        [TestMethod]
        public void SudokuTest()
        {
            var puzzle = new Sudoku(Sudoku.Boards.Board1);
            var solution = puzzle.Solve();
        }
    }
}
