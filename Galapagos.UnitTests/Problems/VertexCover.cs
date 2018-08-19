using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.UnitTests.Problems
{
    class VertexCover
    {
        #region Graphs
        private static double[,] Graph => new double[,] //Petersen Graph
        {
            {0,0,1,1,0,1,0,0,0,0},
            {0,0,0,1,1,0,1,0,0,0},
            {1,0,0,0,1,0,0,1,0,0},
            {1,1,0,0,0,0,0,0,1,0},
            {0,1,1,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,1,0,0,1},
            {0,1,0,0,0,1,0,1,0,0},
            {0,0,1,0,0,0,1,0,1,0},
            {0,0,0,1,0,0,0,1,0,1},
            {0,0,0,0,1,1,0,0,1,0}
        };
        #endregion

        private static IPopulation _population;

        public bool[] Solve()
        {
            var metadata = Session.Instance.LoadMetadata(Metadata.VertexCover, FitnessFunction);
            var cover_metadata = metadata[0];
            cover_metadata.Properties["GeneCount"] = Graph.GetLength(0);

            _population = Session.Instance.CreatePopulation(metadata);

            _population.EnableLogging();
            _population.Evolve();

            return _population.OptimalCreature.GetChromosome<IBinaryChromosome>("cover").ToArray();
        }

        private double FitnessFunction(ICreature creature)
        {
            double fitness = 0;

            var cover = creature.GetChromosome<IBinaryChromosome>("cover");
            int size = cover.Count();

            //Checking edge coverage
            bool[,] seen = new bool[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (cover[i] && !seen[i, j])
                    {
                        fitness += 2*Graph[i, j];
                        seen[i, j] = seen[j, i] = true;
                    }
                }
            }

            //Checking number of covered vertices
            for (int i = 0; i < size; i++)
                if (cover[i]) fitness -= 1;

            return fitness;
        }

        public static void PrintCover(bool[] cover)
        {
            int size = cover.Count();

            var sb = new StringBuilder($"{cover[0]}");
            for (int i = 1; i < size; i++)
                sb.Append($", {cover[i]}");

            Debug.WriteLine("COVER:");
            Debug.WriteLine($" {sb}");
            Debug.WriteLine("Fitness");
            Debug.WriteLine(_population.OptimalCreature.Fitness.ToString());
        }
    }
}