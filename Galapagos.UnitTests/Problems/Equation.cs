using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Galapagos;

namespace Galapagos.UnitTests.Problems
{
    class Equation
    {
        public static void Optimize()
        {
            //-------------------
            //--HyperPerameters--
            //-------------------
            uint N_population = 100; //uint can't be negative

            //--------------------
            //--CreatureMetadata--
            //--------------------
            //CreatureMetadata(fitnessFunction, chromosomeMetaData)
            var creatureMetadata = new CreatureMetadata(creature =>
            {       //lambda exspression that implements fitness function
                var x = creature.GetChromosome<BinaryChromosome>("x").ToDouble();
                var y = creature.GetChromosome<BinaryChromosome>("y").ToDouble();
                var z = creature.GetChromosome<BinaryChromosome>("z").ToDouble();
                return (x * x) - (2 * y) + z;
            });


            //* each chromosome gets its own meta data
            //* using NAME: lets you skip to specific arg
            //* can chain crossovers and mutations with bitwise or |
            //ChromosomeMetadata(NAME, size, Type, crossoverRate, mutationRate, crossoverOperators, mutationOperators)
            creatureMetadata.Add(new BinaryChromosomeMetadata("x", 16, 1, 0.25,
                BinaryCrossover.SinglePoint,
                BinaryMutation.SingleBit | BinaryMutation.CyclicShift));

            creatureMetadata.Add(new BinaryChromosomeMetadata("y", 16, 1, 0.25,
                BinaryCrossover.SinglePoint,
                BinaryMutation.All));

            creatureMetadata.Add(new BinaryChromosomeMetadata("z", 16, 1, 0.25,
                BinaryCrossover.NoOp,
                BinaryMutation.CyclicShift));

            //--------------
            //--Population--
            //--------------
            //Population(size, GeneticDiscription)
            var myPop = new Population(N_population, creatureMetadata);
            myPop.EnableLogging();

            //RegisterTerminationCondition(Type, arg)
            myPop.RegisterTerminationCondition(TerminationCondition.FitnessPlateau, 50);
            myPop.RegisterTerminationCondition(TerminationCondition.GenerationThreshold, 1000);

            var hrs = 0; var mins = 10; var secs = 0;
            myPop.RegisterTerminationCondition(TerminationCondition.Timer, new TimeSpan(hrs, mins, secs));

            //Evolve(Type, arg, bool elitisim, survivial rate)
            //Evolve runs the GA until a termination condition is met
            myPop.Evolve(SelectionAlgorithm.Tournament, 100, true, .3);
            var optimalCreature = myPop.OptimalCreature;
            var optimal_x = optimalCreature.GetChromosome<BinaryChromosome>("x").ToDouble();
            var optimal_y = optimalCreature.GetChromosome<BinaryChromosome>("y").ToDouble();
            var optimal_z = optimalCreature.GetChromosome<BinaryChromosome>("z").ToDouble();
            Debug.WriteLine($"{optimal_x}, {optimal_y}, { optimal_z}"); //Prints to Console; $ operator preforms string interpolation
        }
    }
}
