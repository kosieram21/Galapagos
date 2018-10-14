using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos
{

    #region DETERMINISTIC UNIT TESTS

    /// <summary>
    /// An internal interface that allows unit test to implement
    /// their own RNG outcomes and thus evaluate the correctness
    /// of stochastic algorithms in a deterministic way.
    /// </summary>
    internal interface IStochastic
    {
        T[] Shuffle<T>(T[] array);

        bool FlipCoin();

        bool EvaluateProbability(double probability);

        int Next();

        int Next(int maxValue);

        int Next(uint maxValue);

        int Next(double maxValue);

        int Next(int minValue, int maxValue);

        int Next(uint minValue, uint maxValue);

        int Next(double minValue, double maxValue);

        double NextDouble();
    }

    #endregion

    /// <summary>
    /// Utility class for evaluating stochastic functions.
    /// </summary>
    internal class Stochastic : IStochastic
    {
        private readonly Random _rng = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Shuffles the given input array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to shuffle.</param>
        /// <returns>The shuffled array.</returns>
        public T[] Shuffle<T>(T[] array)
        {
            return array.OrderBy(index => _rng.Next()).ToArray();
        }

        /// <summary>
        /// Flips a coin.
        /// </summary>
        /// <returns>A value indicating if the coin landed on heads.</returns>
        public bool FlipCoin()
        {
            return _rng.Next(2) > 0;
        }

        /// <summary>
        /// Evaluates if an event with the given probability occured.
        /// </summary>
        /// <param name="probability">The probability.</param>
        /// <returns>A value indicating if the event occured.</returns>
        public bool EvaluateProbability(double probability)
        {
            if (probability < 0 || probability > 1)
                throw new ArgumentException("Error! probability must be a value between 0 and 1.");

            var R = _rng.NextDouble();
            return R < probability;
        }

        /// <summary>
        /// Returns a nonnegative random integer.
        /// </summary>
        /// <returns>The integer.</returns>
        public int Next()
        {
            return _rng.Next();
        }


        /// <summary>
        /// Returns a nonnegative random integer that is less than the specified maximum.
        /// </summary>
        /// <returns>The integer.</returns>
        public int Next(int maxValue)
        {
            return _rng.Next(maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random integer that is less than the specified maximum.
        /// </summary>
        /// <returns>The integer.</returns>
        public int Next(uint maxValue)
        {
            return Next((int)maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random integer that is less than the specified maximum.
        /// </summary>
        /// <returns>The integer.</returns>
        public int Next(double maxValue)
        {
            return Next((int)maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random integer that is within the specified range.
        /// </summary>
        /// <returns>The integer.</returns>
        public int Next(int minValue, int maxValue)
        {
            return _rng.Next(minValue, maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random integer that is within the specified range.
        /// </summary>
        /// <returns>The integer.</returns>
        public int Next(uint minValue, uint maxValue)
        {
            return Next((int)minValue, (int)maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random integer that is within the specified range.
        /// </summary>
        /// <returns>The integer.</returns>
        public int Next(double minValue, double maxValue)
        {
            return Next((int)minValue, (int)maxValue);
        }

        /// <summary>
        /// Returns a random floating point number between 0 and 1.
        /// </summary>
        /// <returns>The double.</returns>
        public double NextDouble()
        {
            return 1.0 - _rng.NextDouble();
        }
    }
}
