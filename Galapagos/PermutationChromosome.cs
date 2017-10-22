using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos
{
    /// <summary>
    /// A permutation genetic property of a creature.
    /// </summary>
    public class PermutationChromosome : IPermutationChromosome
    {
        private readonly uint[] _permutation;

        /// <summary>
        /// Constructs a new instance of the <see cref="PermutationChromosome"/> class.
        /// </summary>
        /// <param name="n">The permutation length.</param>
        internal PermutationChromosome(uint n)
        {
            _permutation = GeneratePermutation(n);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="PermutationChromosome"/> class.
        /// </summary>
        /// <param name="permutation">The initial permutation.</param>
        internal PermutationChromosome(uint[] permutation)
        {
            if (!ValidatePermutation(permutation))
                throw new ArgumentException("Error! The given permutation is invalid.");
            _permutation = permutation;
        }

        /// <summary>
        /// Generates a random permutation.
        /// </summary>
        /// <param name="n">The permutation length.</param>
        /// <returns>A random permutation.</returns>
        private uint[] GeneratePermutation(uint n)
        {
            var permutation = new uint[n];

            for (uint i = 0; i < n; i++)
                permutation[i] = i;

            return Stochastic.Shuffle(permutation);
        }

        /// <summary>
        /// Validates a permutation only contains one of each number.
        /// </summary>
        /// <param name="permutation">The permutation to validate.</param>
        /// <returns>A value indicating if the permutation is valid.</returns>
        private bool ValidatePermutation(uint[] permutation)
        {
            var seen = new bool[permutation.Length];
            for(var i = 0; i < permutation.Length; i++)
            {
                var number = permutation[i];
                if (number >= permutation.Length)
                    return false;
                seen[number] = true;
            }

            return seen.All(o => o);
        }

        /// <summary>
        /// Accesses a gene from the chromosome.
        /// </summary>
        /// <param name="index">The gene index.</param>
        /// <returns>The gene.</returns>
        public uint this[int index]
        {
            get
            {
                if (index >= _permutation.Count())
                    throw new Exception($"Error! {index} is larger than the chromosome size.");
                return _permutation[index];
            }
        }

        /// <summary>
        /// Measures the distance between two chromosome.
        /// </summary>
        /// <remarks>Kendall Tau distance.</remarks>
        /// <param name="other">The other chromosome.</param>
        /// <returns>The distance between the chromosomes.</returns>
        public uint Distance(IChromosome other)
        {
            if (!(other is PermutationChromosome) || (((PermutationChromosome)other).N != N))
                throw new ArgumentException("Error! Incompatible chromosomes.");

            var map = new uint[N];
            for (uint i = 0; i < N; i++)
                map[Permutation[i]] = i;

            var otherPermutation = ((PermutationChromosome)other).Permutation;

            var arry = new uint[N];
            var temp = new uint[N];

            Array.Copy(otherPermutation, arry, N);

            var distance = MergerSort(map, arry, temp, 0, (int)(N - 1));
            return distance;
        }

        /// <summary>
        /// Enhanced merge sort implementation for computing Kendall Tau distance.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="arry">The array to sort.</param>
        /// <param name="temp">The temporary buffer.</param>
        /// <param name="left">The left sub array index.</param>
        /// <param name="right">The right sub array index.</param>
        /// <returns>The number of inversions in the array.</returns>
        private uint MergerSort(uint[] map, uint[] arry, uint[] temp, int left, int right)
        {
            if(right > left)
            {
                var mid = (right + left) / 2;
                var invCount =
                    MergerSort(map, arry, temp, left, mid) +
                    MergerSort(map, arry, temp, mid + 1, right) +
                    Merge(map, arry, temp, left, mid + 1, right);

                return invCount;
            }

            return 0;
        }

        /// <summary>
        /// Enhanced merge subroutine for merge sort.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="arry">The array to sort.</param>
        /// <param name="temp">The temporary buffer.</param>
        /// <param name="left">The left sub array index.</param>
        /// <param name="mid">The mid point index.</param>
        /// <param name="right">The right sub array index.</param>
        /// <returns>The number of inversions in the array.</returns>
        private uint Merge(uint[] map, uint[] arry, uint[] temp, int left, int mid, int right)
        {
            uint invCount = 0;

            var i = left;
            var j = mid;
            var k = left;

            while ((i <= mid - 1) && (j <= right))
            {
                if (map[arry[i]] <= map[arry[j]])
                {
                    temp[k++] = arry[i++];
                }
                else
                { 
                    temp[k++] = arry[j++];
                    invCount += (uint)(mid - i);
                }
            }

            while (i <= mid - 1)
                temp[k++] = arry[i++];

            while (j <= right)
                temp[k++] = arry[j++];

            for (i = left; i <= right; i++)
                arry[i] = temp[i];

            return invCount;
        }

        /// <summary>
        /// Gets the permutation.
        /// </summary>
        public uint[] Permutation => _permutation;

        /// <summary>
        /// Gets the permutation length.
        /// </summary>
        public uint N => (uint)_permutation.Length;

        #region IEnumerable Members

        public IEnumerator<uint> GetEnumerator()
        {
            return _permutation.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
