using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;
using System.Diagnostics;

namespace Galapagos.UnitTests.Problems
{
    // Cryptarithmetic Puzzle. Each letter must take on a unqiue value such that
    // the following equation is true. S and M can not take on the value 0.
    //   S E N D
    // + M O R E
    // ---------
    // M O N E Y

    class Cryptarithmetic
    {
        public uint[] Solve()
        {
            var metadata = Session.Instance.LoadMetadata(Metadata.Cryptarithmetic, FitnessFunction);
            var population = Session.Instance.CreatePopulation(metadata);

            population.EnableLogging();
            population.Evolve();

            return population.OptimalCreature.GetChromosome<IPermutationChromosome>("ordering").Take(8).ToArray();
        }

        private double FitnessFunction(ICreature creature)
        {
            var ordering = creature.GetChromosome<IPermutationChromosome>("ordering").Take(8).ToArray();
            var fitness = 0;

            var S = GetS(ordering);
            var E = GetE(ordering);
            var N = GetN(ordering);
            var D = GetD(ordering);
            var M = GetM(ordering);
            var O = GetO(ordering);
            var R = GetR(ordering);
            var Y = GetY(ordering);
            var C10 = GetCarryOver(D, E);
            var C100 = GetCarryOver(N, R);
            var C1000 = GetCarryOver(E, O);
            var C10000 = GetCarryOver(S, M);

            if (M == (C10000 % 10)) fitness++;
            if (O == ((S + M + C1000) % 10)) fitness++;
            if (N == ((E + O + C100) % 10)) fitness++;
            if (E == ((N + R + C10) % 10)) fitness++;
            if (Y == ((D + E) % 10)) fitness++;
            if (S != 0) fitness++;
            if (M != 0) fitness++;

            return fitness;
        }

        private static uint GetCarryOver(uint x, uint y)
        {
            return (((x + y) / 10) % 10);
        }

        private static uint GetS(uint[] ordering)
        {
            return ordering[0];
        }

        private static uint GetE(uint[] ordering)
        {
            return ordering[1];
        }

        private static uint GetN(uint[] ordering)
        {
            return ordering[2];
        }

        private static uint GetD(uint[] ordering)
        {
            return ordering[3];
        }

        private static uint GetM(uint[] ordering)
        {
            return ordering[4];
        }

        private static uint GetO(uint[] ordering)
        {
            return ordering[5];
        }

        public static uint GetR(uint[] ordering)
        {
            return ordering[6];
        }

        private static uint GetY(uint[] ordering)
        {
            return ordering[7];
        }

        public static void PrintMapping(uint[] ordering)
        {
            var S = GetS(ordering);
            var E = GetE(ordering);
            var N = GetN(ordering);
            var D = GetD(ordering);
            var M = GetM(ordering);
            var O = GetO(ordering);
            var R = GetR(ordering);
            var Y = GetY(ordering);
            var C10 = GetCarryOver(D, E);
            var C100 = GetCarryOver(N, R);
            var C1000 = GetCarryOver(E, O);
            var C10000 = GetCarryOver(S, M);

            Debug.WriteLine(M == (C10000 % 10));
            Debug.WriteLine(O == ((S + M + C1000) % 10));
            Debug.WriteLine(N == ((E + O + C100) % 10));
            Debug.WriteLine(E == ((N + R + C10) % 10));
            Debug.WriteLine(Y == ((D + E) % 10));
            Debug.WriteLine(S != 0);
            Debug.WriteLine(M != 0);

            Debug.WriteLine($"S:{S},E:{E},N:{N},D:{D},M:{M},O:{O},R:{R},Y:{Y};C10:{C10},C100:{C100},C1000:{C1000},C10000:{C10000}");
        }
    }
}
