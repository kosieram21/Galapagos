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
    /// Cyclic shift mutation.
    /// </summary>
    /// <typeparam name="TChromosome">The chromosome type of this mutation.</typeparam>
    public abstract class CyclicShiftMutation<TChromosome> : Mutation<TChromosome>
        where TChromosome : class, IChromosome
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="CyclicShiftMutation"/> class.
        /// </summary>
        /// <param name="weigth">The crossover weight.</param>
        protected CyclicShiftMutation(double weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Preforms a cyclic shift on the given array.
        /// </summary>
        /// <typeparam name="T">The array data type.</typeparam>
        /// <param name="arry"></param>
        /// <returns>The shifted array.</returns>
        protected T[] CyclicShift<T>(T[] arry)
        {
            var newArry = new T[arry.Count()];
            Array.Copy(arry, newArry, arry.Count());

            var start = Stochastic.Next(newArry.Length - 1);
            var end = Stochastic.Next(start + 1, newArry.Length);

            var first = newArry[start];
            for (var i = start + 1; i <= end; i++)
                newArry[i - 1] = newArry[i];
            newArry[end] = first;

            return newArry;
        }
    }
}
