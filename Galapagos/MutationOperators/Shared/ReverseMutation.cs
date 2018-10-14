using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;
using Galapagos.API;

namespace Galapagos.MutationOperators.Shared
{
    /// <summary>
    /// Reverse mutation.
    /// </summary>
    /// <typeparam name="TChromosome">The chromosome type of this mutation.</typeparam>
    public abstract class ReverseMutation<TChromosome> : Mutation<TChromosome>
        where TChromosome : class, IChromosome
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ReverseMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        protected ReverseMutation(double weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Preforms a reverse on the given array.
        /// </summary>
        /// <typeparam name="T">The array data type.</typeparam>
        /// <param name="arry"></param>
        /// <returns>The reversed array.</returns>
        protected T[] Reverse<T>(T[] arry)
        {
            var newArry = new T[arry.Count()];
            Array.Copy(arry, newArry, arry.Count());

            var start = Session.Instance.Stochastic.Next(newArry.Length - 1);
            var end = Session.Instance.Stochastic.Next(start + 1, newArry.Length);

            for (var i = 0; i <= (end - start) / 2; i++)
            {
                var temp = newArry[start + i];
                newArry[start + i] = newArry[end - i];
                newArry[end - i] = temp;
            }

            return newArry;
        }
    }
}
