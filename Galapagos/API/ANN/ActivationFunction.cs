using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.API.ANN
{
    public static class ActivationFunction
    {
        public enum Type
        {
            Identity,
            BinaryStep,
            Sigmoid,
            Tanh,
            ArcTan,
            Sinusoid,
            Softsign,
            ReLu,
            LeakyReLu,
            SoftPlus,
            BentIdentity,
            Sinc,
            Gaussian
        }

        /// <summary>
        /// Gets an activation function.
        /// </summary>
        /// <param name="type">The activation function type.</param>
        /// <returns>The activation function.</returns>
        public static Func<double, double> Get(Type type)
        {
            switch(type)
            {
                case Type.Identity:
                    return Identity;
                case Type.BinaryStep:
                    return BinaryStep;
                case Type.Sigmoid:
                    return Sigmoid;
                case Type.Tanh:
                    return Tanh;
                case Type.ArcTan:
                    return ArcTan;
                case Type.Sinusoid:
                    return Sinusoid;
                case Type.Softsign:
                    return Softsign;
                case Type.ReLu:
                    return ReLu;
                case Type.LeakyReLu:
                    return LeakyReLu;
                case Type.SoftPlus:
                    return SoftPlus;
                case Type.BentIdentity:
                    return BentIdentity;
                case Type.Sinc:
                    return Sinc;
                case Type.Gaussian:
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
            return Math.Tan(x);
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
            return ((Math.Sqrt(Math.Pow(x, 2) + 1) - 1) / 2) +x;
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
