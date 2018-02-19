using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using Galapagos.Metadata;
using Galapagos.Metadata.Parser;
using Galapagos.API.ANN;

namespace Galapagos.API
{
    public class Session
    {
        /// <summary>
        /// Gets an instance of the Galapagos session.
        /// </summary>
        public static Session Instance => _session.Value;
        private static readonly Lazy<Session> _session =
            new Lazy<Session>(() => new Session());

        /// <summary>
        /// Constructs a new instance of the <see cref="Session"/> class.
        /// </summary>
        private Session()
        {
            LogToConsole = true;
        }

        /// <summary>
        /// Gets or sets a value indicating if console logging should be enabled.
        /// File logging is still available even if console logging is disabled.
        /// </summary>
        public bool LogToConsole { get; set; }

        /// <summary>
        /// Loads a neural network from a .ann file.
        /// </summary>
        /// <param name="path">The .ann file path.</param>
        /// <returns>The neural network.</returns>
        public NeuralNetwork LoadNeuralNetwork(string path)
        {
            var annFile = AnnFile.Open(path);
            var network = LoadNeuralNetwork(annFile);

            return network;
        }

        /// <summary>
        /// Loads a neural network from a .ann file.
        /// </summary>
        /// <param name="annFile">The .ann file/</param>
        /// <returns>The neural network.</returns>
        public NeuralNetwork LoadNeuralNetwork(AnnFile annFile)
        {
            var network = new NeuralNetwork(annFile);

            return network;
        }

        /// <summary>
        /// Loads the metadata XML file at the given location.
        /// </summary>
        /// <param name="path">The file path to the metadata XML file.</param>
        /// <returns>The parsed metadata.</returns>
        public IPopulationMetadata LoadMetadata(string path, Func<ICreature, double> fitnessFunction)
        {
            var parts = path.Split('\\');
            path = Path.HasExtension(path) ? Path.ChangeExtension(path, "xml") : $"{path}.xml";
            path = parts.Count() != 1 ? path : Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);

            var xDoc = XDocument.Load(path);

            var metadata = MetadataParser.Parse(xDoc) as PopulationMetadata;
            metadata.FitnessFunction = fitnessFunction;

            return metadata as IPopulationMetadata;
        }

        /// <summary>
        /// Loads the metadata XML file from the given stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="fitnessFunction">The fitness function.</param>
        /// <returns>The parsed metadata.</returns>
        public IPopulationMetadata LoadMetadata(Stream stream, Func<ICreature, double> fitnessFunction)
        {
            var xDoc = XDocument.Load(stream);

            var metadata = MetadataParser.Parse(xDoc) as PopulationMetadata;
            metadata.FitnessFunction = fitnessFunction;

            return metadata as IPopulationMetadata;
        }

        /// <summary>
        /// Creates a population object.
        /// </summary>
        /// <param name="metadata">The population metadata.</param>
        /// <returns>The constructed population.</returns>
        public IPopulation CreatePopulation(IPopulationMetadata metadata)
        {
            SemanticValidator.Validate(metadata);
            return new Population(metadata) as IPopulation;
        }
    }
}
