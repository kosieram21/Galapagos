using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;
using Galapagos.API.ANN;

namespace Galapagos.Chromosomes
{
    /// <summary>
    /// A TWEANN encoded in a chromosome.
    /// </summary>
    public class NeuralChromosome : Chromosome, INeuralChromosome
    {
        #region Gene Definitions

        public class NodeGene
        {
            public enum NodeType
            {
                Input,
                Hidden,
                Output
            }

            public readonly uint ID;
            public readonly NodeType Type;

            internal NodeGene(uint id, NodeType type)
            {
                ID = id;
                Type = type;
            }
        }

        public class EdgeGene
        {
            public readonly uint ID;

            public readonly NodeGene Input;
            public readonly NodeGene Output;

            public double Weight;
            public bool Enabled;

            internal EdgeGene(uint id, NodeGene input, NodeGene output, double weight, bool enabled)
            {
                ID = id;

                Input = input;
                Output = output;

                Weight = weight;
                Enabled = enabled;
            }
        }

        #endregion

        #region Innovation Tracker

        private static IDictionary<string, InnovationTracker> _innovationTrackers = new Dictionary<string, InnovationTracker>();

        internal class InnovationTracker
        {
            private readonly uint _initialeNodeCount;

            private uint _nodeInnovationNumber;
            private uint _edgeInnovationNumber;

            public InnovationTracker(uint initialeNodeCount = 0)
            {
                _initialeNodeCount = initialeNodeCount;
                Reset();
            }

            public uint GetNextNodeInnovationNumber()
            {
                return _nodeInnovationNumber++;
            }

            public uint GetNextEdgeInnovationNumber()
            {
                return _edgeInnovationNumber++;
            }

            public void Reset()
            {
                _nodeInnovationNumber = _initialeNodeCount;
                _edgeInnovationNumber = 0;
            }
        }

        /// <summary>
        /// Creates a new innovation traker.
        /// </summary>
        /// <param name="name">The innovation tracker name.</param>
        /// <param name="initialNodeCount">The initial node count.</param>
        internal static void CreateInnovationTracker(string name, uint initialNodeCount)
        {
            if (!_innovationTrackers.ContainsKey(name))
                _innovationTrackers[name] = new InnovationTracker(initialNodeCount);
        }

        /// <summary>
        /// Deletes an existing innovation tracker.
        /// </summary>
        /// <param name="name">The innovation tracker name.</param>
        internal static void DeleteInnovationTracker(string name)
        {
            if (_innovationTrackers.ContainsKey(name))
                _innovationTrackers.Remove(name);
        }

        /// <summary>
        /// Gets an existing innovation tracker.
        /// </summary>
        /// <param name="name">The innovation tracker name.</param>
        /// <returns>The innovation tracker.</returns>
        internal static InnovationTracker GetInnovationTracker(string name)
        {
            if (!_innovationTrackers.ContainsKey(name))
                throw new ArgumentException($"Error! No innovation tracker named {name} exists");
            return _innovationTrackers[name];
        }

        #endregion

        private readonly uint _inputSize = 0;
        private readonly uint _outputSize = 0;

        private readonly double _c1 = 1;
        private readonly double _c2 = 1;
        private readonly double _c3 = 1;

        private readonly string _innovationTrackerName;

        private readonly IList<NodeGene> _nodeGenes = new List<NodeGene>();
        private readonly IList<EdgeGene> _edgeGenes = new List<EdgeGene>();

        private NeuralNetwork _network = null;

        /// <summary>
        /// Constructs a new instance of the <see cref="BinaryChromosome"/> class.
        /// </summary>
        /// <param name="inputSize">The input size.</param>
        /// <param name="outputSize">The output size.</param>
        /// <param name="innovationTrackerName">The innovation tracker name.</param>
        /// <param name="c1">C1 NEAT niching weight.</param>
        /// <param name="c2">C2 NEAT niching weight.</param>
        /// <param name="c3">C3 NEAT niching weight.</param>
        internal NeuralChromosome(uint inputSize, uint outputSize, string innovationTrackerName,
            double c1 = 1, double c2 = 1, double c3 = 1)
        {
            _inputSize = inputSize;
            _outputSize = outputSize;

            _c1 = c1;
            _c2 = c2;
            _c3 = c3;

            if (_inputSize == 0)
                throw new ArgumentException("Error! Input size cannot be 0.");
            if (_outputSize == 0)
                throw new ArgumentException("Error! Output size cannot be 0.");

            _innovationTrackerName = innovationTrackerName;
            CreateInnovationTracker(innovationTrackerName, inputSize + outputSize);

            uint j = 0;
            for (var i = 0; i < _inputSize; i++)
                _nodeGenes.Add(new NodeGene(j++, NodeGene.NodeType.Input));

            for (var i = 0; i < _outputSize; i++)
                _nodeGenes.Add(new NodeGene(j++, NodeGene.NodeType.Output));
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BinaryChromosome"/> class.
        /// </summary>
        /// <param name="nodeGenes">The node genes associated with this chromosome.</param>
        /// <param name="edgeGenes">The edge genes asspciated with this chromosome.</param>
        /// <param name="innovationTrackerName">The innovation tracker name.</param>
        /// <param name="c1">C1 NEAT niching weight.</param>
        /// <param name="c2">C2 NEAT niching weight.</param>
        /// <param name="c3">C3 NEAT niching weight.</param>
        internal NeuralChromosome(IList<NodeGene> nodeGenes, IList<EdgeGene> edgeGenes, string innovationTrackerName,
            double c1, double c2, double c3)
        {
            _nodeGenes = nodeGenes;
            _edgeGenes = edgeGenes;

            _c1 = c1;
            _c2 = c2;
            _c3 = c3;

            _innovationTrackerName = innovationTrackerName;

            foreach (var gene in _nodeGenes)
            {
                if (gene.Type == NodeGene.NodeType.Input)
                    _inputSize++;
                else if (gene.Type == NodeGene.NodeType.Output)
                    _outputSize++;
            }

            if (_inputSize == 0)
                throw new ArgumentException("Error! Input size cannot be 0.");
            if (_outputSize == 0)
                throw new ArgumentException("Error! Output size cannot be 0.");
        }

        /// <summary>
        /// Measures the distance between two chromosome.
        /// </summary>
        /// <param name="other">The other chromosome.</param>
        /// <returns>The distance between the chromosomes.</returns>
        public override double Distance(IChromosome other)
        {
            if (!(other is NeuralChromosome))
                throw new ArgumentException("Error! Incompatible chromosome.");

            if (InnovationTrackerName != ((NeuralChromosome)other).InnovationTrackerName)
                throw new ArgumentException("Error! Incompatible chromosome.");

            var thisGeneQueue = GetGeneQueue(this);
            var otherGeneQueue = GetGeneQueue(other as NeuralChromosome);

            var matchingGeneCount = 0;

            double E = 0;
            double D = 0;
            double W = 0;
            double N = 0;

            while(thisGeneQueue.Any() && otherGeneQueue.Any())
            {
                var currentGene = thisGeneQueue.Peek();
                var otherGene = otherGeneQueue.Peek();

                if (currentGene.ID == otherGene.ID)
                {
                    W += currentGene.ID - otherGene.ID;
                    matchingGeneCount++;

                    thisGeneQueue.Dequeue();
                    otherGeneQueue.Dequeue();
                }
                else if(currentGene.ID < otherGene.ID)
                {
                    D++;
                    thisGeneQueue.Dequeue();
                }
                else
                {
                    D++;
                    otherGeneQueue.Dequeue();
                }
            }

            E = thisGeneQueue.Any() ? thisGeneQueue.Count : otherGeneQueue.Count;
            W = W / matchingGeneCount;
            N = thisGeneQueue.Any() ? EdgeGenes.Count() : ((NeuralChromosome)other).EdgeGenes.Count();

            return ((C1 * E) / N) + ((C2 * D) / N) + (C3 * W);
        }

        /// <summary>
        /// Gets a queue of edge genes sorted by ID for the given chromosome.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <returns>The gene queue.</returns>
        private Queue<EdgeGene> GetGeneQueue(NeuralChromosome chromosome)
        {
            var sortedGenes = chromosome.EdgeGenes.OrderByDescending(gene => gene.ID).Reverse();
            var geneQueue = new Queue<EdgeGene>(sortedGenes);

            return geneQueue;
        }

        /// <summary>
        /// Gets the innovation tracker associated with this chromosome.
        /// </summary>
        internal string InnovationTrackerName => _innovationTrackerName;

        /// <summary>
        /// Gets the C1 NEAT niching weight.
        /// </summary>
        internal double C1 => _c1;

        /// <summary>
        /// Gets the C2 NEAT niching weight.
        /// </summary>
        internal double C2 => _c2;

        /// <summary>
        /// Gets the C3 NEAT niching weight.
        /// </summary>
        internal double C3 => _c3;

        /// <summary>
        /// Gets the node genes for this chromosome.
        /// </summary>
        public IReadOnlyList<NodeGene> NodeGenes => _nodeGenes as IReadOnlyList<NodeGene>;

        /// <summary>
        /// Gets the edge genes for this chromosome.
        /// </summary>
        public IReadOnlyList<EdgeGene> EdgeGenes => _edgeGenes as IReadOnlyList<EdgeGene>;

        /// <summary>
        /// Gets the neural chromosome's input size.
        /// </summary>
        public uint InputSize => _inputSize;

        /// <summary>
        /// Gets the neural chromosome's output size.
        /// </summary>
        public uint OutputSize => _outputSize;

        /// <summary>
        /// Evaluates the neural network.
        /// </summary>
        /// <param name="inputs">The input data.</param>
        /// <returns>The output data.</returns>
        public double[] Evaluate(double[] inputs)
        {
            if (_network == null)
                _network = new NeuralNetwork(_nodeGenes, _edgeGenes);
            return _network.Evaluate(inputs);
        }

        /// <summary>
        /// Clones the genes of this chromosome.
        /// </summary>
        /// <returns>The clonded genes.</returns>
        internal Tuple<List<NodeGene>, List<EdgeGene>> CloneGenes()
        {
            var nodeGenes = new List<NodeGene>();
            var edgeGenes = new List<EdgeGene>();

            var geneMap = new Dictionary<uint, NodeGene>();

            foreach (var gene in _nodeGenes)
            {
                var clone = new NodeGene(gene.ID, gene.Type);
                nodeGenes.Add(clone);
                geneMap[gene.ID] = clone;
            }

            foreach (var gene in _edgeGenes)
            {
                var clone = new EdgeGene(gene.ID, geneMap[gene.Input.ID], geneMap[gene.Output.ID], gene.Weight, gene.Enabled);
                edgeGenes.Add(clone);
            }

            return new Tuple<List<NodeGene>, List<EdgeGene>>(nodeGenes, edgeGenes);
        }
    }
}
