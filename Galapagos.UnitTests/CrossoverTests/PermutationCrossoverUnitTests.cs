/*using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.Chromosomes;
using Galapagos.API;
using Galapagos.API.Factory;

namespace Galapagos.UnitTests.CrossoverTests
{
    [TestClass]
    public class PermutationCrossoverUnitTests
    {
        private const int GENE_COUNT = 5000;

        private PermutationChromosome GetChromosome()
        {
            return GeneticFactory.ConstructChromosome(new PermutationChromosomeMetadata("NULL", GENE_COUNT))
                as PermutationChromosome;
        }

        private PermutationChromosome GetCrossover(PermutationChromosome x, PermutationChromosome y, PermutationCrossover crossover)
        {
            var rator = GeneticFactory.ConstructPermutationCrossoverOperators(crossover).First();
            return rator.Invoke(x, y) as PermutationChromosome;
        }

        private void VerifyChangeOccured(PermutationChromosome x, PermutationChromosome y)
        {
            var diff = 0;
            for (var i = 0; i < GENE_COUNT; i++)
            {
                if (x.Permutation[i] != y.Permutation[i])
                    diff++;
            }

            Assert.IsTrue(diff > 0, "Mutation failed to change chromosome data.");
        }

        [TestMethod]
        public void AlternatingPositionCorossoverTest()
        {
            var x = GetChromosome();
            var y = GetChromosome();
            var crossover = GetCrossover(x, y, PermutationCrossover.AlternatingPosition);

            VerifyChangeOccured(x, crossover);
            VerifyChangeOccured(y, crossover);

            var seen = new bool[GENE_COUNT];

            var j = 0;
            for(var i = 0; i < GENE_COUNT; i++)
            {
                if (!seen[x.Permutation[i]] && crossover.Permutation[j] == x.Permutation[i])
                {
                    seen[x.Permutation[i]] = true;
                    j++;
                }

                if (!seen[y.Permutation[i]] && crossover.Permutation[j] == y.Permutation[i])
                {
                    seen[y.Permutation[i]] = true;
                    j++;
                }

                var consistent = seen[x.Permutation[i]] || seen[y.Permutation[i]];

                Assert.IsTrue(consistent, "Inccorrect implementation.");
            }
        }

        [TestMethod]
        public void OrderCrossoverTest()
        {
            var x = GetChromosome();
            var y = GetChromosome();
            var crossover = GetCrossover(x, y, PermutationCrossover.Order);

            VerifyChangeOccured(x, crossover);
            VerifyChangeOccured(y, crossover);

            var consistent = false;
            var regions = FindIdenticalRegions(x, crossover);
            foreach(var region in regions)
            {
                if(TestRegion(y, crossover, region))
                {
                    consistent = true;
                    break;
                }
            }

            Assert.IsTrue(consistent, "Inccorrect implementation.");
        }

        private IList<Tuple<int, int>> FindIdenticalRegions(PermutationChromosome x, PermutationChromosome crossover)
        {
            var regions = new List<Tuple<int, int>>();

            var same = new bool[GENE_COUNT];
            for (var i = 0; i < GENE_COUNT; i++)
                same[i] = crossover.Permutation[i] == x.Permutation[i];

            for(var i = 0; i < GENE_COUNT; i++)
            {
                if(same[i])
                {
                    var start = i;
                    while (same[i]) i++;
                    var end = i;

                    regions.Add(new Tuple<int, int>(start, end));
                }
            }

            return regions;
        }

        private bool TestRegion(PermutationChromosome y, PermutationChromosome crossover, Tuple<int, int> region)
        {
            var seen = new bool[GENE_COUNT];

            for (var i = region.Item1; i <= region.Item2; i++)
                seen[crossover.Permutation[i]] = true;

            for (var i = 1; i < GENE_COUNT; i++)
            {
                var index = (region.Item2 + 1) % GENE_COUNT;
                if (!seen[y.Permutation[index]] && (y.Permutation[index] != crossover.Permutation[index]))
                    return false;
                seen[y.Permutation[index]] = true;
            }

            return true;
        }
    }
}*/
