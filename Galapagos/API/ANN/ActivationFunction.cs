using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.API.ANN
{
    public static class ActivationFunctions
    {
        /// <summary>
        /// Gets an activation function.
        /// </summary>
        /// <param name="type">The activation function type.</param>
        /// <returns>The activation function.</returns>
        public static Func<double, double> Get(ActivationFunction type)
        {
            switch(type)
            {
                case ActivationFunction.Identity:
                    return Identity;
                case ActivationFunction.BinaryStep:
                    return BinaryStep;
                case ActivationFunction.Sigmoid:
                    return Sigmoid;
                case ActivationFunction.Tanh:
                    return Tanh;
                case ActivationFunction.ArcTan:
                    return ArcTan;
                case ActivationFunction.Sinusoid:
                    return Sinusoid;
                case ActivationFunction.Softsign:
                    return Softsign;
                case ActivationFunction.ReLu:
                    return ReLu;
                case ActivationFunction.LeakyReLu:
                    return LeakyReLu;
                case ActivationFunction.SoftPlus:
                    return SoftPlus;
                case ActivationFunction.BentIdentity:
                    return BentIdentity;
                case ActivationFunction.Sinc:
                    return Sinc;
                case ActivationFunction.Gaussian:
                    return Gaussian;
                default:
                    throw new ArgumentException($"Error! {type} is not a valid activation function.");
            }
        }

        #region Implementations

        internal static double Identity(double x)
        {
            return x;
        }

        internal static double BinaryStep(double x)
        {
            return x < 0 ? 0 : 1;
        }

        internal static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x));
        }

        internal static double Tanh(double x)
        {
            return Math.Tanh(x);
        }

        internal static double ArcTan(double x)
        {
            return Math.Atan(x);
        }

        internal static double Sinusoid(double x)
        {
            return Math.Sin(x);
        }

        internal static double Softsign(double x)
        {
            return 1 / (1 + Math.Abs(x));
        }

        internal static double ReLu(double x)
        {
            return x < 0 ? 0 : x;
        }

        internal static double LeakyReLu(double x)
        {
            return x < 0 ? 0.01 * x : x;
        }

        internal static double SoftPlus(double x)
        {
            return Math.Log(1 + Math.Pow(Math.E, x));
        }

        internal static double BentIdentity(double x)
        {
            return ((Math.Sqrt(Math.Pow(x, 2) + 1) - 1) / 2) + x;
        }

        internal static double Sinc(double x)
        {
            return x == 0 ? 1 : Math.Sin(x) / x;
        }

        internal static double Gaussian(double x)
        {
            return Math.Pow(Math.E, -Math.Pow(x, 2));
        }

        #endregion
    }
}
