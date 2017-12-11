using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.Chromosomes;

namespace Galapagos.API.ANN
{
    public class NeuralNetwork
    {
        private readonly IList<Neuron> _neurons = new List<Neuron>();
        private readonly IList<Connection> _connections = new List<Connection>();

        private readonly IList<Neuron> _inputNeurons = new List<Neuron>();
        private readonly IList<Neuron> _outputNeurons = new List<Neuron>();

        private readonly Func<double, double> _activationFunction = ActivationFunction.Get(ActivationFunction.Type.Identity);

        /// <summary>
        /// Constructs a new instance of the <see cref="NeuralNetwork"/> class.
        /// </summary>
        /// <param name="annFile">The .ann file.</param>
        public NeuralNetwork(AnnFile annFile)
            : this(annFile.AdjacencyMatrix, annFile.InputNeurons, annFile.OutputNeurons, annFile.ActivationFunction) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="NeuralNetwork"/> class.
        /// </summary>
        /// <param name="adjacencyMatrix">The adjacency matrix representation of the network.</param>
        /// <param name="inputNeurons">The input neuron ids.</param>
        /// <param name="outputNeurons">The output neuron ids.</param>
        /// <param name="activationFunction">The activation function.</param>
        public NeuralNetwork(double[,] adjacencyMatrix, IList<uint> inputNeurons, IList<uint> outputNeurons, 
            ActivationFunction.Type activationFunction = ActivationFunction.Type.Identity)
        {
            _activationFunction = ActivationFunction.Get(activationFunction);

            var n = adjacencyMatrix.GetLength(0);
            var m = adjacencyMatrix.GetLength(1);

            if (n == 0 || m == 0)
                throw new ArgumentException("Error! Adjacency matrix cannot contain a 0 dimension.");

            if (n != m)
                throw new ArgumentException("Error! Adjacency matrix must be a square matrix.");

            if (inputNeurons.Any(index => index >= n))
                throw new ArgumentException("Error! Input neuron index out of bounds.");

            if (outputNeurons.Any(index => index >= n))
                throw new ArgumentException("Error! Ouput neuron index out of bounds.");

            if (inputNeurons.Intersect(outputNeurons).Count() != 0)
                throw new ArgumentException("Error! Input neurons cannot also be output neuron.");

            for (var i = 0; i < n; i++)
                _neurons.Add(new Neuron((uint)i));

            for (var i = 0; i < n; i++)
                for (var j = 0; j < n; j++)
                    if (adjacencyMatrix[i, j] != 0)
                        _connections.Add(new Connection(_neurons[i], _neurons[j], adjacencyMatrix[i, j]));

            foreach (var index in inputNeurons)
                _inputNeurons.Add(_neurons[(int)index]);

            foreach (var index in outputNeurons)
                _outputNeurons.Add(_neurons[(int)index]);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="NeuralNetwork"/> class.
        /// </summary>
        /// <param name="nodeGenes">The list of node genes.</param>
        /// <param name="edgeGenes">The list of edge genes.</param>
        internal NeuralNetwork(IList<NeuralChromosome.NodeGene> nodeGenes, IList<NeuralChromosome.EdgeGene> edgeGenes)
        {
            var neuronMap = new Dictionary<NeuralChromosome.NodeGene, Neuron>();

            foreach(var gene in nodeGenes)
            {
                var neuron = new Neuron(gene.ID);
                _neurons.Add(neuron);
                neuronMap[gene] = neuron;

                if (gene.Type == NeuralChromosome.NodeGene.NodeType.Input)
                    _inputNeurons.Add(neuron);
                else if (gene.Type == NeuralChromosome.NodeGene.NodeType.Output)
                    _outputNeurons.Add(neuron);
            }

            foreach(var gene in edgeGenes)
            {
                if(gene.Enabled)
                {
                    var edge = new Connection(neuronMap[gene.Input], neuronMap[gene.Output], gene.Weight);
                    _connections.Add(edge);
                }
            }
        }

        /// <summary>
        /// Gets the neurons.
        /// </summary>
        public IReadOnlyList<Neuron> Neurons => _neurons as IReadOnlyList<Neuron>;

        /// <summary>
        /// Gets the connections.
        /// </summary>
        public IReadOnlyList<Connection> Connections => _connections as IReadOnlyList<Connection>;

        /// <summary>
        /// Gets the input neurons.
        /// </summary>
        public IReadOnlyList<Neuron> InputNeurons => _inputNeurons as IReadOnlyList<Neuron>;

        /// <summary>
        /// Gets the output neurons.
        /// </summary>
        public IReadOnlyList<Neuron> OutputNeurons => _outputNeurons as IReadOnlyList<Neuron>;

        /// <summary>
        /// Evaluates the neural network.
        /// </summary>
        /// <param name="inputs">The input data.</param>
        /// <returns>The output data.</returns>
        public double[] Evaluate(double[] inputs)
        {
            if (inputs.Count() != _inputNeurons.Count)
                throw new ArgumentException($"Error! Expected {_inputNeurons.Count} inputs but recieved {inputs.Count()}");

            for (var i = 0; i < _inputNeurons.Count; i++)
                _inputNeurons[i].Value = inputs[i];

            for(var i = 0; i < _neurons.Count; i++)
            {
                var neuron = _neurons[i];
                double sum = 0;

                foreach (var input in neuron.Inputs)
                    sum += (input.Weight * input.Source.Value);

                if(neuron.Inputs.Count > 0)
                    neuron.Value = _activationFunction(sum);
            }

            var outputs = new double[_outputNeurons.Count];
            for (var i = 0; i < _outputNeurons.Count; i++)
                outputs[i] = _outputNeurons[i].Value;

            return outputs;
        }
    }
}
