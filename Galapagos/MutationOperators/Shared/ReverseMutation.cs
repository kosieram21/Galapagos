using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.MutationOperators.Shared
{
    /// <summary>
    /// Reverse mutation.
    /// </summary>
    /// <typeparam name="TChromosome">The chromosome type of this mutation.</typeparam>
    internal abstract class ReverseMutation<TChromosome> : Mutation<TChromosome>
        where TChromosome : class, IChromosome
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ReverseMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        protected ReverseMutation(uint weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Preforms a reverse on the given array.
        /// </summary>
        /// <typeparam name="T">The array data type.</typeparam>
        /// <param name="arry"></param>
        /// <returns>The reversed array.</returns>
        protected T[] Reverse<T>(T[] arry)
        {
            var start = Stochastic.Next(arry.Length - 1);
            var end = Stochastic.Next(start + 1, arry.Length);

            for (var i = 0; i <= (end - start) / 2; i++)
            {
                var temp = arry[start + i];
                arry[start + i] = arry[end - i];
                arry[end - i] = temp;
            }

            return arry;
        }
    }
}
