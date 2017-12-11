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
        private readonly ActivationFunction.Type _activation = ActivationFunction.Type.Sigmoid;
        private readonly Func<double, double> _activationFunction = ActivationFunction.Get(ActivationFunction.Type.Sigmoid);

        private readonly IList<Neuron> _neurons = new List<Neuron>();
        private readonly IList<Connection> _connections = new List<Connection>();

        private readonly IList<Neuron> _inputNeurons = new List<Neuron>();
        private readonly IList<Neuron> _outputNeurons = new List<Neuron>();

        private double[,] _adjacencyMatrix = null;

        /// <summary>
        /// Constructs a new instance of the <see cref="NeuralNetwork"/> class.
        /// </summary>
        /// <param name="path">The .ann file path.</param>
        public NeuralNetwork(string path)
            : this(AnnFile.Open(path)) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="NeuralNetwork"/> class.
        /// </summary>
        /// <param name="annFile">The .ann file.</param>
        public NeuralNetwork(AnnFile annFile)
            : this(annFile.AdjacencyMatrix, annFile.InputNeurons, annFile.OutputNeurons, annFile.Activation) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="NeuralNetwork"/> class.
        /// </summary>
        /// <param name="adjacencyMatrix">The adjacency matrix representation of the network.</param>
        /// <param name="inputNeurons">The input neuron ids.</param>
        /// <param name="outputNeurons">The output neuron ids.</param>
        /// <param name="activationFunction">The activation function.</param>
        public NeuralNetwork(double[,] adjacencyMatrix, IList<uint> inputNeurons, IList<uint> outputNeurons, 
            ActivationFunction.Type activationFunction = ActivationFunction.Type.Sigmoid) //Temp! need to fix chromosome metadata.
        {
            _adjacencyMatrix = adjacencyMatrix;

            _activation = activationFunction;
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
            var neuronMap = new Dictionary<uint, Neuron>();

            foreach(var gene in nodeGenes)
            {
                var neuron = new Neuron(gene.ID);
                _neurons.Add(neuron);
                neuronMap[gene.ID] = neuron;

                if (gene.Type == NeuralChromosome.NodeGene.NodeType.Input)
                    _inputNeurons.Add(neuron);
                else if (gene.Type == NeuralChromosome.NodeGene.NodeType.Output)
                    _outputNeurons.Add(neuron);
            }

            foreach(var gene in edgeGenes)
            {
                if(gene.Enabled)
                {
                    var edge = new Connection(neuronMap[gene.Input.ID], neuronMap[gene.Output.ID], gene.Weight);
                    _connections.Add(edge);
                }
            }
        }

        /// <summary>
        /// Gets the activarion function.
        /// </summary>
        public ActivationFunction.Type Activation => _activation;

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
        /// Gets the adjacency matrix for this neural network.
        /// </summary>
        public double[,] AdjacencyMatrix
        {
            get
            {
                if (_adjacencyMatrix == null)
                    _adjacencyMatrix = ComputeAdjacencyMatrix();
                return _adjacencyMatrix;
            }
        }

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

        /// <summary>
        /// Saves the neural network as a .ann file.
        /// </summary>
        /// <param name="name">The .ann file name.</param>
        /// <returns>The .ann file.</returns>
        public AnnFile Save(string name)
        {
            var map = ComputeNodeIdMap();
            return new AnnFile
            {
                Name = name,
                Activation = Activation,
                InputNeurons = InputNeurons.Select(neuron => (uint)map[neuron.ID]).ToList(),
                OutputNeurons = OutputNeurons.Select(neuron => (uint)map[neuron.ID]).ToList(),
                AdjacencyMatrix = AdjacencyMatrix
            };
        }

        /// <summary>
        /// Computes the adjacency matrix for this neural network.
        /// </summary>
        /// <returns>The adjacency matrix.</returns>
        private double[,] ComputeAdjacencyMatrix()
        {
            var M = Neurons.Count;
            var adjacencyMatrix = new double[M, M];
            var map = ComputeNodeIdMap();

            foreach (var connection in Connections)
            {
                var i = map[connection.Source.ID];
                var j = map[connection.Target.ID];
                adjacencyMatrix[i, j] = connection.Weight;
            }

            return adjacencyMatrix;
        }

        /// <summary>
        /// Computes the node id map for saving .ann files.
        /// </summary>
        /// <returns>The node id map.</returns>
        private int[] ComputeNodeIdMap()
        {
            var N = Neurons.Max(neuron => neuron.ID) + 1;
            var M = Neurons.Count;

            var map = new int[N];
            for (var i = 0; i < M; i++)
                map[Neurons[i].ID] = i;

            return map;
        }
    }
}
