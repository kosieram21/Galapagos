using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.UnitTests.Fakes
{
    public class CrossoverFake : ICrossover
    {
        public double Weight => 1;

        public IChromosome Invoke(IChromosome x, IChromosome y)
        {
            throw new NotImplementedException();
        }
    }
}
