using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Galapagos.API.ANN
{
    public class AnnFile
    {
        /// <summary>
        /// Gets or sets the name of the .ann file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the activation function.
        /// </summary>
        public ActivationFunction.Type ActivationFunction { get; set; }

        /// <summary>
        /// Gets or sets the input neurons.
        /// </summary>
        public IList<uint> InputNeurons { get; set; }

        /// <summary>
        /// Gets or sets the output neurons.
        /// </summary>
        public IList<uint> OutputNeurons { get; set; }

        /// <summary>
        /// Gets or sets the adjacency matrix.
        /// </summary>
        public double[,] AdjacencyMatrix { get; set; }

        /// <summary>
        /// Writes the .ann file to the disk in the given directory.
        /// </summary>
        /// <param name="directory">The directory to write the .ann file to.</param>
        public void WriteToDisk(string directory = "")
        {
            if (directory == "")
                directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var path = Path.Combine(directory, $"{Name}.ann");
            using (StreamWriter writer = new StreamWriter(path))
            {
                WriteActivationFunction(writer, ActivationFunction);
                WriteUIntArray(writer, InputNeurons);
                WriteUIntArray(writer, OutputNeurons);
                WriteAdjacencyMatrix(writer, AdjacencyMatrix);
            }
        }

        /// <summary>
        /// Opens the .ann file at the given location.
        /// </summary>
        /// <param name="path">The .ann file path.</param>
        /// <returns>The opened ANN (artifical neural network) file.</returns>
        public static AnnFile Open(string path)
        {
            var parts = path.Split('\\');
            path = Path.HasExtension(path) ? Path.ChangeExtension(path, "ann") : $"{path}.ann";
            path = parts.Count() != 1 ? path : Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);

            AnnFile annFile;
            using (StreamReader reader = new StreamReader(path))
            {
                annFile = new AnnFile
                {
                    Name = Regex.Replace(parts.Last(), @"\..*", ""),
                    ActivationFunction = ReadActivationFunction(reader),
                    InputNeurons = ReadUIntArray(reader),
                    OutputNeurons = ReadUIntArray(reader),
                    AdjacencyMatrix = ReadAdjacencyMatrix(reader)
                };
            }

            return annFile;
        }

        #region I/O

        /// <summary>
        /// Reads the activation function.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The activation function.</returns>
        private static ActivationFunction.Type ReadActivationFunction(StreamReader reader)
        {
            ActivationFunction.Type activationFunction;

            var line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("Error! Corrupt .ann file!");

            var success = Enum.TryParse(line, out activationFunction);
            if(!success)
                throw new ArgumentException("Error! Corrupt .ann file!");

            return activationFunction;
        }

        /// <summary>
        /// Writes the activation function.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="activationFunction">The activation function.</param>
        private static void WriteActivationFunction(StreamWriter writer, ActivationFunction.Type activationFunction)
        {
            var line = Enum.GetName(typeof(ActivationFunction.Type), activationFunction);
            writer.WriteLine(line);
        }

        /// <summary>
        /// Reads a uint array.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The uint array.</returns>
        private static IList<uint> ReadUIntArray(StreamReader reader)
        {
            var uintArray = new List<uint>();

            var line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("Error! Corrupt .ann file!");

            var parts = line.Split(',');
            if(parts.Count() == 0)
                throw new ArgumentException("Error! Corrupt .ann file!");

            foreach(var part in parts)
            {
                uint index;

                var success = UInt32.TryParse(part, out index);
                if (!success)
                    throw new ArgumentException("Error! Corrupt .ann file!");

                uintArray.Add(index);
            }

            return uintArray;
        }

        /// <summary>
        /// Writes a uint array.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="uintyArray">The uint array.</param>
        private static void WriteUIntArray(StreamWriter writer, IList<uint> uintyArray)
        {
            var line = SerializeList(uintyArray);
            writer.WriteLine(line);
        }

        /// <summary>
        /// Reads the adjacency matrix.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The adjacency matrix.</returns>
        private static double[,] ReadAdjacencyMatrix(StreamReader reader)
        {
            var lines = new List<string>();
            var line = reader.ReadLine();
            while(line != null)
            {
                lines.Add(line);
                line = reader.ReadLine();
            }

            var n = lines.Count;
            if(n < 2)
                throw new ArgumentException("Error! Corrupt .ann file!");

            var adjacencyMatrix = new double[n, n];
            for(var  i = 0; i < n; i++)
            {
                var parts = lines[i].Split(',');
                if(parts.Count() != n)
                    throw new ArgumentException("Error! Corrupt .ann file!");

                for(var j = 0; j < n; j++)
                {
                    double weight;

                    var success = Double.TryParse(parts[j], out weight);
                    if (!success)
                        throw new ArgumentException("Error! Corrupt .ann file!");

                    adjacencyMatrix[i, j] = weight;
                }
            }

            return adjacencyMatrix;
        }

        /// <summary>
        /// Writes the adjacency matrix.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="adjacencyMatrix">The adjacency matrix.</param>
        private static void WriteAdjacencyMatrix(StreamWriter writer, double[,] adjacencyMatrix)
        {
            var lines = new List<string>();

            var n = adjacencyMatrix.GetLength(0);
            for (var i = 0; i < n; i++)
            {
                var data = new List<double>();
                for (var j = 0; j < n; j++)
                    data.Add(adjacencyMatrix[i, j]);

                var line = SerializeList(data);
                lines.Add(line);
            }

            foreach (var line in lines)
                writer.WriteLine(line);
        }

        /// <summary>
        /// Setializes the given list.
        /// </summary>
        /// <typeparam name="T">The data type of the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>The serialized list.</returns>
        private static string SerializeList<T>(IList<T> list)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("{0}", list[0]);
            for (var i = 1; i < list.Count; i++)
                sb.AppendFormat(",{0}", list[i]);

            return sb.ToString();
        }

        #endregion
    }
}
