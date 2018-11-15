using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.MutationOperators.Shared
{
    /// <summary>
    /// Scramble mutation.
    /// </summary>
    /// <typeparam name="TChromosome">The chromosome type of this mutation.</typeparam>
    public abstract class ScrambleMutation<TChromosome> : Mutation<TChromosome>
        where TChromosome : class, IChromosome
   {
        /// <summary>
        /// Constructs a new instance of the <see cref="ScrambleMutation"/> class.
        /// </summary>
        /// <param name="weigth">The mutation weight.</param>
        protected ScrambleMutation(double weigth = 1)
            : base(weigth) { }

        /// <summary>
        /// Rolls a start and end point--wrapping allowed--and scrambles the elements between.
        /// </summary>
        /// <typeparam name="T">The array data type.</typeparam>
        /// <param name="arry"></param>
        /// <returns>The scrambled array.</returns>
        protected T[] Scramble<T>(T[] arry)
        {
            var N = arry.Count();
            var newArry = new T[N];
            Array.Copy(arry, newArry, N);

            var start = Session.Instance.Stochastic.Next(N);
            var end = Session.Instance.Stochastic.Next(N);
            while(end != start) end = Session.Instance.Stochastic.Next(N);

            var isWrapped = start > end;
            var offset = isWrapped ? end + 1 : 0;

            //If isWrapped (N-start) count back end & (start+1) counts front end.
            var len = end - start + 1 + (isWrapped ? N : 0); 
            var scrambled = new T[len];

            for(var i = 0; i < len; i++)
                scrambled[i] = arry[(start + i) % N];

            scrambled = Session.Instance.Stochastic.Shuffle(scrambled);

            for (var i = 0; i < len; i++)
                newArry[(start + i) % N] = scrambled[i];

            return newArry;
        }
    }
}
