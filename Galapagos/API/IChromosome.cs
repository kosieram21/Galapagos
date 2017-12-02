using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.API
{
    public interface IChromosome
    {
        /// <summary>
        /// Measures the distance between two chromosome.
        /// </summary>
        /// <param name="other">The other chromosome.</param>
        /// <returns>The distance between the chromosomes.</returns>
        uint Distance(IChromosome other);
    }

    public interface IChromosome<T> : IChromosome, IEnumerable<T>
    {
        /// <summary>
        /// Accesses a gene from the chromosome.
        /// </summary>
        /// <param name="index">The gene index.</param>
        /// <returns>The gene.</returns>
        T this[int index] { get; }
    }

    public interface IBinaryChromosome : IChromosome<bool>
    {
        /// <summary>
        /// Converts the binary chromosome to an uint.
        /// </summary>
        /// <param name="lower">The lower bound.</param>
        /// <param name="upper">The upper bound.</param>
        /// <returns>The uint.</returns>
        uint ToUInt(uint lower = uint.MinValue, uint upper = uint.MaxValue);

        /// <summary>
        /// Converts the binary chromosome to an int.
        /// </summary>
        /// <param name="lower">The lower bound.</param>
        /// <param name="upper">The upper bound.</param>
        /// <returns>The int.</returns>
        int ToInt(int lower = int.MinValue, int upper = int.MaxValue);

        /// <summary>
        /// Converts the binary chromosome to an double.
        /// </summary>
        /// <param name="lower">The lower bound.</param>
        /// <param name="upper">The upper bound.</param>
        /// <returns>The double.</returns>
        double ToDouble(double lower = double.MinValue, double upper = double.MaxValue);
    }

    public interface IPermutationChromosome : IChromosome<uint> { }

    public interface INeuralChromosome : IChromosome
    {
        /// <summary>
        /// Gets the neural chromosome's input size.
        /// </summary>
        uint InputSize { get; }

        /// <summary>
        /// Gets the neural chromosome's output size.
        /// </summary>
        uint OutputSize { get; }

        /// <summary>
        /// Evaluates the neural network.
        /// </summary>
        /// <param name="inputs">The input data.</param>
        /// <returns>The output data.</returns>
        double[] Evaluate(double[] inputs);
    }
}
