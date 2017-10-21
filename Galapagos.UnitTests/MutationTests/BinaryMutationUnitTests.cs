using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos;
using Galapagos.MutationOperators;

namespace Galapagos.UnitTests.MutationTests
{
    [TestClass]
    public class BinaryMutationUnitTests
    {
        private const int GENE_COUNT = 10;

        private BinaryChromosome GetChromosome()
        {
            return GeneticFactory.ConstructChromosome(new BinaryChromosomeMetadata("test", GENE_COUNT)) 
                as BinaryChromosome;
        }

        private BinaryChromosome GetMutation(BinaryChromosome chromosome, BinaryMutation mutation)
        {
            var rator = GeneticFactory.ConstructBinaryMutationOperators(mutation).First();
            return rator.Invoke(chromosome) as BinaryChromosome;
        }

        [TestMethod]
        public void CyclicShiftMutationTest()
        {
            var chromosome = GetChromosome();
            var mutation = GetMutation(chromosome, BinaryMutation.CyclicShift);

            for(var i = 0; i < GENE_COUNT; i++)
            {
                if(mutation.Bits[i] != chromosome.Bits[i])
                {
                    while(mutation.Bits[i] != chromosome.Bits[i])
                    {
                        if (i >= GENE_COUNT)
                            break;

                        Assert.IsTrue(mutation.Bits[i] == chromosome.Bits[i + 1]);

                        i++;
                    }
                }
            }
        }
    }
}
