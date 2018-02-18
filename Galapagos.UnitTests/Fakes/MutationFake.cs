using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.UnitTests.Fakes
{
    public class MutationFake : IMutation
    {
        public double Weight => 1;

        public IChromosome Invoke(IChromosome chromosome)
        {
            throw new NotImplementedException();
        }
    }
}
