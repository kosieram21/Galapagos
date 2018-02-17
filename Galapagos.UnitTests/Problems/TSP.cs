using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Galapagos.API;

namespace Galapagos.UnitTests.Problems
{
    class TSP
    {
        #region Maps

        public static class Maps
        {
            public static double[,] Map1 => new double[,]
            {
                {   0, 2451,  713, 1018, 1631, 1374, 2408,  213, 2571,  875, 1420, 2145, 1972}, // New York
                {2451,    0, 1745, 1524,  831, 1240,  959, 2596,  403, 1589, 1374,  357,  579}, // Los Angeles
                { 713, 1745,    0,  355,  920,  803, 1737,  851, 1858,  262,  940, 1453, 1260}, // Chicago
                {1018, 1524,  355,    0,  700,  862, 1395, 1123, 1584,  466, 1056, 1280,  987}, // Minneapolis
                {1631,  831,  920,  700,    0,  663, 1021, 1769,  949,  796,  879,  586,  371}, // Denver
                {1374, 1240,  803,  862,  663,    0, 1681, 1551, 1765,  547,  225,  887,  999}, // Dallas
                {2408,  959, 1737, 1395, 1021, 1681,    0, 2493,  678, 1724, 1891, 1114,  701}, // Seattle
                { 213, 2596,  851, 1123, 1769, 1551, 2493,    0, 2699, 1038, 1605, 2300, 2099}, // Boston
                {2571,  403, 1858, 1584,  949, 1765,  678, 2699,    0, 1744, 1645,  653,  600}, // San Francisco
                { 875, 1589,  262,  466,  796,  547, 1724, 1038, 1744,    0,  679, 1272, 1162}, // St. Louis
                {1420, 1374,  940, 1056,  879,  225, 1891, 1605, 1645,  679,    0, 1017, 1200}, // Houston
                {2145,  357, 1453, 1280,  586,  887, 1114, 2300,  653, 1272, 1017,    0,  504}, // Phoenix
                {1972,  579, 1260,  987,  371,  999,  701, 2099,  600, 1162,  1200,  504,   0}  // Salt Lake City
            };

            public static double[,] Map2 => new double[,]
            {
                {   0,    1,  713, 1018, 1631, 1374, 2408,  213, 2571,  875, 1420, 2145, 1972}, // New York
                {2451,    0,    1, 1524,  831, 1240,  959, 2596,  403, 1589, 1374,  357,  579}, // Los Angeles
                { 713, 1745,    0,    1,  920,  803, 1737,  851, 1858,  262,  940, 1453, 1260}, // Chicago
                {1018, 1524,  355,    0,    1,  862, 1395, 1123, 1584,  466, 1056, 1280,  987}, // Minneapolis
                {1631,  831,  920,  700,    0,    1, 1021, 1769,  949,  796,  879,  586,  371}, // Denver
                {1374, 1240,  803,  862,  663,    0,    1, 1551, 1765,  547,  225,  887,  999}, // Dallas
                {2408,  959, 1737, 1395, 1021, 1681,    0,    1,  678, 1724, 1891, 1114,  701}, // Seattle
                { 213, 2596,  851, 1123, 1769, 1551, 2493,    0,    1, 1038, 1605, 2300, 2099}, // Boston
                {2571,  403, 1858, 1584,  949, 1765,  678, 2699,    0,    1, 1645,  653,  600}, // San Francisco
                { 875, 1589,  262,  466,  796,  547, 1724, 1038, 1744,    0,    1, 1272, 1162}, // St. Louis
                {1420, 1374,  940, 1056,  879,  225, 1891, 1605, 1645,  679,    0,    1, 1200}, // Houston
                {2145,  357, 1453, 1280,  586,  887, 1114, 2300,  653, 1272, 1017,    0,    1}, // Phoenix
                {   1,  579, 1260,  987,  371,  999,  701, 2099,  600, 1162,  1200,  504,   0}  // Salt Lake City
            };
        }

        #endregion

        private readonly double[,] _map;

        public TSP(double[,] map)
        {
            _map = map;
        }

        public uint[] Solve()
        {
            var metadata = Session.Instance.LoadMetadata(Metadata.TSP, FitnessFunction);
            var population = Session.Instance.CreatePopulation(metadata);

            population.EnableLogging();
            population.Evolve();

            return population.OptimalCreature.GetChromosome<IPermutationChromosome>("tour").ToArray();
        }

        private double FitnessFunction(ICreature creature)
        {
            double fitness = 0;

            var tour = creature.GetChromosome<IPermutationChromosome>("tour");
            for (int i = 0, j = 1; i < 13; i++, j++)
                fitness -= _map[tour[i], tour[(j % 13)]];
            
            return fitness;
        }

        public static void PrintTour(uint[] tour)
        {
            var sb = new StringBuilder($"{tour[0]}");
            for (int i = 1; i <= 13; i++)
                sb.Append($"->{tour[(i % 13)]}");

            Debug.WriteLine("TOUR:");
            Debug.WriteLine($" {sb}");
        }
    }
}
