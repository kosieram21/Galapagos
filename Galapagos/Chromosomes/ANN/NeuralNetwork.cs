using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.Chromosomes.ANN
{
    public class NeuralNetwork
    {
        private readonly IList<Node> _nodes = new List<Node>();
        private readonly IList<Edge> _edges = new List<Edge>();

        private readonly IList<Node> _inputNodes = new List<Node>();
        private readonly IList<Node> _outputNodes = new List<Node>();

        /// <summary>
        /// Constructs a new instance of the <see cref="NeuralNetwork"/> class.
        /// </summary>
        /// <param name="nodeGenes">The list of node genes.</param>
        /// <param name="edgeGenes">The list of edge genes.</param>
        internal NeuralNetwork(IList<NeuralChromosome.NodeGene> nodeGenes, IList<NeuralChromosome.EdgeGene> edgeGenes)
        {
            var nodeMap = new Dictionary<NeuralChromosome.NodeGene, Node>();

            foreach(var gene in nodeGenes)
            {
                var node = new Node(gene.ID);
                _nodes.Add(node);
                nodeMap[gene] = node;

                if (gene.Type == NeuralChromosome.NodeGene.NodeType.Input)
                    _inputNodes.Add(node);
                else if (gene.Type == NeuralChromosome.NodeGene.NodeType.Output)
                    _outputNodes.Add(node);
            }

            foreach(var gene in edgeGenes)
            {
                if(gene.Enabled)
                {
                    var edge = new Edge(nodeMap[gene.Input], nodeMap[gene.Output], gene.Weight);
                    _edges.Add(edge);
                }
            }
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="NeuralNetwork"/> class.
        /// </summary>
        /// <param name="adjacencyMatrix">The adjacency matrix representation of the network.</param>
        /// <param name="inputNodes">The input node ids.</param>
        /// <param name="outputNodes">The output node ids.</param>
        public NeuralNetwork(double[,] adjacencyMatrix, IList<uint> inputNodes, IList<uint> outputNodes)
        {
            var n = adjacencyMatrix.GetLength(0);
            var m = adjacencyMatrix.GetLength(1);

            if (n == 0)
                throw new ArgumentException("Error! Adjacency matrix cannot contain a 0 dimension.");

            if (n != m)
                throw new ArgumentException("Error! Adjacency matrix must be a square matrix.");

            if (inputNodes.Any(index => index >= n))
                throw new ArgumentException("Error! Input node index out of bounds.");

            if (outputNodes.Any(index => index >= n))
                throw new ArgumentException("Error! Ouput node index out of bounds.");

            if (inputNodes.Intersect(outputNodes).Count() != 0)
                throw new ArgumentException("Error! Input nodes cannot also be output nodes.");

            for (var i = 0; i < n; i++)
                _nodes.Add(new Node((uint)i));

            for (var i = 0; i < n; i++)
                for (var j = 0; j < n; j++)
                    if(adjacencyMatrix[i,j] != 0)
                        _edges.Add(new Edge(_nodes[i], _nodes[j], adjacencyMatrix[i,j]));

            foreach(var index in inputNodes)
                _inputNodes.Add(_nodes[(int)index]);

            foreach (var index in outputNodes)
                _outputNodes.Add(_nodes[(int)index]);
        }

        /// <summary>
        /// Evaluates the neural network.
        /// </summary>
        /// <param name="inputs">The input data.</param>
        /// <returns>The output data.</returns>
        public double[] Evaluate(double[] inputs)
        {
            if (inputs.Count() != _inputNodes.Count)
                throw new ArgumentException($"Error! Expected {_inputNodes.Count} inputs but recieved {inputs.Count()}");

            for (var i = 0; i < _inputNodes.Count; i++)
                _inputNodes[i].Value = inputs[i];

            for(var i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                double sum = 0;

                foreach (var input in node.Inputs)
                    sum += (input.Weight * input.Source.Value);

                if(node.Inputs.Count > 0)
                    node.Value = sum;
            }

            var outputs = new double[_outputNodes.Count];
            for (var i = 0; i < _outputNodes.Count; i++)
                outputs[i] = _outputNodes[i].Value;

            return outputs;
        }
    }
}
