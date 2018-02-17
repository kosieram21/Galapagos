using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Galapagos.API;

namespace Galapagos.UnitTests.Problems
{
    class _3Sat
    {
        public bool[] Solve()
        {
            var metadata = Session.Instance.LoadMetadata(Metadata._3Sat, FitnessFunction);
            var population = Session.Instance.CreatePopulation(metadata);

            population.EnableLogging();
            population.Evolve();

            return population.OptimalCreature.GetChromosome<IBinaryChromosome>("interpretation").ToArray();
        }

        private double FitnessFunction(ICreature creature)
        {
            var fitness = 0;
            var interpretation = GetInterpretation(creature);

            if (interpretation["X1"] || interpretation["Y1"] || interpretation["Z1"]) fitness++;
            if (interpretation["X2"] || interpretation["Y2"] || interpretation["Z2"]) fitness++;
            if (interpretation["X3"] || interpretation["Y3"] || interpretation["Z3"]) fitness++;

            if (!interpretation["X1"] || !interpretation["X2"] || interpretation["X3"]) fitness++;
            if (!interpretation["X1"] || interpretation["X2"] || !interpretation["X3"]) fitness++;

            if (!interpretation["Y1"] || !interpretation["Y2"] || interpretation["Y3"]) fitness++;
            if (interpretation["Y1"] || !interpretation["Y2"] || !interpretation["Y3"]) fitness++;

            if (!interpretation["Z1"] || interpretation["Z2"] || !interpretation["Z3"]) fitness++;
            if (interpretation["Z1"] || !interpretation["Z2"] || !interpretation["Z3"]) fitness++;

            return fitness;
        }

        private IDictionary<string, bool> GetInterpretation(ICreature creature)
        {
            var interpretation = creature.GetChromosome<IBinaryChromosome>("interpretation");
            return GetInterpretation(interpretation.ToArray());
        }

        private static IDictionary<string, bool> GetInterpretation(bool[] assignment)
        {
            return new Dictionary<string, bool>
            {
                { "X1", assignment[0] },
                { "X2", assignment[1] },
                { "X3", assignment[2] },

                { "Y1", assignment[3] },
                { "Y2", assignment[4] },
                { "Y3", assignment[5] },

                { "Z1", assignment[6] },
                { "Z2", assignment[7] },
                { "Z3", assignment[8] },
            };
        }

        public static void PrintInterpretation(bool[] assignment)
        {
            var interpretation = GetInterpretation(assignment);
            var formula =
                " (  X1 |  Y1 |  Z1 ) &" + Environment.NewLine + 
                " (  X2 |  Y2 |  Z2 ) &" + Environment.NewLine + 
                " (  X3 |  Y3 |  Z3 ) & " + Environment.NewLine +
                " ( !X1 | !X2 |  X3 ) &" + Environment.NewLine +
                " ( !X1 |  X2 | !X3 ) &" + Environment.NewLine +
                " ( !Y1 | !Y2 |  Y3 ) &" + Environment.NewLine +
                " (  Y1 | !Y2 | !Y3 ) &" + Environment.NewLine +
                " ( !Z1 |  Z2 | !Z3 ) &" + Environment.NewLine +
                " (  Z1 | !Z2 | !Z3 )";

            Debug.WriteLine("FORMULA:");
            Debug.WriteLine(formula);

            Debug.WriteLine("INTERPRETATION:");
            Debug.WriteLine($" X1: {interpretation["X1"]}");
            Debug.WriteLine($" X2: {interpretation["X2"]}");
            Debug.WriteLine($" X3: {interpretation["X3"]}");
            Debug.WriteLine("");
            Debug.WriteLine($" Y1: {interpretation["Y1"]}");
            Debug.WriteLine($" Y2: {interpretation["Y2"]}");
            Debug.WriteLine($" Y3: {interpretation["Y3"]}");
            Debug.WriteLine("");
            Debug.WriteLine($" Z1: {interpretation["Z1"]}");
            Debug.WriteLine($" Z2: {interpretation["Z2"]}");
            Debug.WriteLine($" Z3: {interpretation["Z3"]}");
        }
    }
}
