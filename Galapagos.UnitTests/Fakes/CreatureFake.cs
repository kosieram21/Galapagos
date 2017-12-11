using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.UnitTests.Fakes
{
    public class CreatureFake : ICreature
    {
        public CreatureFake(double fitness)
        {
            Fitness = fitness;
        }

        public double Fitness { get; private set; }

        TChromosomeType ICreature.GetChromosome<TChromosomeType>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
