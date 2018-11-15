using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Galapagos.API;
using Galapagos.UnitTests.Problems;


namespace Galapagos.UnitTests
{
    [TestClass]
    public class SampleProblemUnitTests
    {
        private const string WUMPUS_WORLD_ANN_FILE = "Wumpus";

        [TestMethod]
        public void _3SatTest()
        {
            var puzzle = new _3Sat();
            var solution = puzzle.Solve();
            _3Sat.PrintInterpretation(solution);
        }

        [TestMethod]
        public void SudokuTest()
        {
            var puzzle = new Sudoku(Sudoku.Boards.Board1);
            var solution = puzzle.Solve();
            Sudoku.PrintBoard(solution);
        }

        [TestMethod]
        public void NQueensTest()
        {
            var puzzle = new NQueens();
            var solution = puzzle.Solve();
            NQueens.PrintBoard(solution);
        }

        [TestMethod]
        public void CryptarithmeticTest()
        {
            var puzzle = new Cryptarithmetic();
            var solution = puzzle.Solve();
            Cryptarithmetic.PrintMapping(solution);
        }

        [TestMethod]
        public void TspTest()
        {
            var puzzle = new TSP(TSP.Maps.Map1);
            var solution = puzzle.Solve();
            TSP.PrintTour(solution);
        }

        [TestMethod]
        public void VertexCoverTest()
        {
            var puzzle = new VertexCover();
            var solution = puzzle.Solve();
            VertexCover.PrintCover(solution);
        }

        [TestMethod]
        public void WumpusWorldTrainerTest()
        {
            var environment = new WumpusWorld(WumpusWorld.Boards.Board2);
            var trainer = new WumpusWorldTrainer(environment);
            var neuralNetwork = trainer.Train();

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var annFile = neuralNetwork.Save(WUMPUS_WORLD_ANN_FILE);
            annFile.WriteToDisk(path);
        }

        [TestMethod]
        public void WumpusWorldAgentTest()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), WUMPUS_WORLD_ANN_FILE);
            var neuralNetwork = Session.Instance.LoadNeuralNetwork(path);
            var environment = new WumpusWorld(WumpusWorld.Boards.Board2);

            var agent = new WumpusWorldAgent(neuralNetwork);
            agent.Navigate(environment);
        }
    }
}
