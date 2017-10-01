using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos
{
    /// <summary>
    /// Utility class for evaluating stochastic functions.
    /// </summary>
    internal static class Stochastic
    {
        private static readonly Random _rng = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Shuffles the given input array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to shuffle.</param>
        /// <returns>The shuffled array.</returns>
        public static T[] Shuffle<T>(T[] array)
        {
            return array.OrderBy(index => _rng.Next()).ToArray();
        }

        /// <summary>
        /// Flips a coin.
        /// </summary>
        /// <returns>A value indicating if the coin landed on heads.</returns>
        public static bool FlipCoin()
        {
            return _rng.Next(2) > 0;
        }

        /// <summary>
        /// Evaluates if an event with the given probability occured.
        /// </summary>
        /// <param name="probability">The probability.</param>
        /// <returns>A value indicating if the event occured.</returns>
        public static bool EvaluateProbability(double probability)
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
        public static int Next()
        {
            return _rng.Next();
        }


        /// <summary>
        /// Returns a nonnegative random integer that is less than the specified maximum.
        /// </summary>
        /// <returns>The integer.</returns>
        public static int Next(int maxValue)
        {
            return _rng.Next(maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random integer that is less than the specified maximum.
        /// </summary>
        /// <returns>The integer.</returns>
        public static int Next(uint maxValue)
        {
            return Next((int)maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random integer that is less than the specified maximum.
        /// </summary>
        /// <returns>The integer.</returns>
        public static int Next(double maxValue)
        {
            return Next((int)maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random integer that is within the specified range.
        /// </summary>
        /// <returns>The integer.</returns>
        public static int Next(int minValue, int maxValue)
        {
            return _rng.Next(minValue, maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random integer that is within the specified range.
        /// </summary>
        /// <returns>The integer.</returns>
        public static int Next(uint minValue, uint maxValue)
        {
            return Next((int)minValue, (int)maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random integer that is within the specified range.
        /// </summary>
        /// <returns>The integer.</returns>
        public static int Next(double minValue, double maxValue)
        {
            return Next((int)minValue, (int)maxValue);
        }

        /// <summary>
        /// Returns a random floating point number between 0 and 1.
        /// </summary>
        /// <returns>The double.</returns>
        public static double NextDouble()
        {
            return 1.0 - _rng.NextDouble();
        }
    }
}
