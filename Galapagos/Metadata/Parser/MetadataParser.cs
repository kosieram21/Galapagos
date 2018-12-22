using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Galapagos.API;
using Galapagos.API.Factory;

namespace Galapagos.Metadata.Parser
{
    internal static class MetadataParser
    {
        /// <summary>
        /// Parses the given population metadata file.
        /// </summary>
        /// <param name="xDoc">The metadata document.</param>
        /// <returns>The parsed population metadata.</returns>
        public static IPopulationMetadata Parse(XDocument xDoc)
        {
            SyntacticValidator.Validate(xDoc);
            var metadata = ParsePopulation(xDoc.Root);
            SemanticValidator.Validate(metadata);

            return metadata;
        }

        /// <summary>
        /// Parses a Population element.
        /// </summary>
        /// <param name="element">The Population element.</param>
        /// <returns>The parsed population metadata.</returns>
        private static IPopulationMetadata ParsePopulation(XElement element)
        {
            var populationMetadata = new PopulationMetadata();

            ParsePopulationAttributes(ref populationMetadata, element);
            ParsePopulationChildren(ref populationMetadata, element);

            return populationMetadata;
        }

        /// <summary>
        /// Parses the population attributes.
        /// </summary>
        /// <param name="populationMetadata">The population metadata.</param>
        /// <param name="element">The population element.</param>
        private static void ParsePopulationAttributes(ref PopulationMetadata populationMetadata, XElement element)
        {
            foreach (var attribute in element.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "Size":
                        populationMetadata.Size = UInt32.Parse(attribute.Value);
                        break;
                    case "SurvivalRate":
                        populationMetadata.SurvivalRate = Double.Parse(attribute.Value);
                        break;
                    case "DistanceThreshold":
                        populationMetadata.DistanceThreshold = Double.Parse(attribute.Value);
                        break;
                    case "CooperativeCoevolution":
                        populationMetadata.CooperativeCoevolution = Boolean.Parse(attribute.Value);
                        break;
                    case "GroupCount":
                        populationMetadata.GroupCount = UInt32.Parse(attribute.Value);
                        break;
                    case "GroupIter":
                        populationMetadata.GroupIter = UInt32.Parse(attribute.Value);
                        break;
                }
            }
        }

        /// <summary>
        /// Parses the population children.
        /// </summary>
        /// <param name="populationMetadata">The population metadata.</param>
        /// <param name="element">The population element.</param>
        private static void ParsePopulationChildren(ref PopulationMetadata populationMetadata, XElement element)
        {
            foreach (var child in element.Elements())
            {
                switch (child.Name.LocalName)
                {
                    case "SelectionAlgorithm":
                        populationMetadata.SelectionAlgorithm = ParseSelectionAlgorithm(child);
                        break;
                    case "TerminationConditions":
                        populationMetadata.AddTerminationCondition(ParseTerminationConditions(child));
                        break;
                    case "Chromosomes":
                        populationMetadata.AddChromosomeMetadata(ParseChromosomes(child));
                        break;
                }
            }
        }

        /// <summary>
        /// Parses a SelectionAlgorithm element.
        /// </summary>
        /// <param name="element">The SelectionAlgorithm element.</param>
        /// <returns>The parsed selection algorithm.</returns>
        private static ISelectionAlgorithm ParseSelectionAlgorithm(XElement element)
        {
            var attributes = element.Attributes();
            var typeAttribute = attributes.FirstOrDefault(attribute => attribute.Name.LocalName == "Type");
            var argAttribute = attributes.FirstOrDefault(attribute => attribute.Name.LocalName == "Arg");

            var algorithm = (SelectionAlgorithm)Enum.Parse(typeof(SelectionAlgorithm), typeAttribute.Value);

            return GeneticFactory.ConstructSelectionAlgorithm(algorithm, argAttribute?.Value);
        }

        /// <summary>
        /// Parses a TerminationConditions element.
        /// </summary>
        /// <param name="element">The TerminationConditions element.</param>
        /// <returns>The parsed termination conditions.</returns>
        private static IList<ITerminationCondition> ParseTerminationConditions(XElement element)
        {
            var terminationConditions = new List<ITerminationCondition>();
            foreach (var child in element.Elements())
                terminationConditions.Add(ParseTerminationCondition(child));

            return terminationConditions;
        }

        /// <summary>
        /// Parses a TerminationCondition element.
        /// </summary>
        /// <param name="element">The TerminationConition element.</param>
        /// <returns>The parsed termination condition.</returns>
        private static ITerminationCondition ParseTerminationCondition(XElement element)
        {
            var attributes = element.Attributes();
            var typeAttribute = attributes.FirstOrDefault(attribute => attribute.Name.LocalName == "Type");
            var argAttribute = attributes.FirstOrDefault(attribute => attribute.Name.LocalName == "Arg");

            var condition = (TerminationCondition)Enum.Parse(typeof(TerminationCondition), typeAttribute.Value);

            return GeneticFactory.ConstructTerminationCondition(condition, argAttribute?.Value);
        }

        /// <summary>
        /// Parses a Chromosomes element.
        /// </summary>
        /// <param name="element">The Chromosomes element.</param>
        /// <returns>The parsed chromosomes.</returns>
        private static IList<IChromosomeMetadata> ParseChromosomes(XElement element)
        {
            var chromosomes = new List<IChromosomeMetadata>();

            foreach (var child in element.Elements())
                chromosomes.AddRange(ParseChromosome(child));

            return chromosomes;
        }

        /// <summary>
        /// Parses a Chromosome element.
        /// </summary>
        /// <param name="element">The Chromosome element.</param>
        /// <returns>The parsed chromosome.</returns>
        private static IList<IChromosomeMetadata> ParseChromosome(XElement element)
        {
            var attributes = element.Attributes();
            var repeatAttribute = attributes.FirstOrDefault(attribute => attribute.Name.LocalName == "Repeat");
            var repeat = repeatAttribute == null ? 1 : Int32.Parse(repeatAttribute.Value);

            var chromosomeMetadataList = new List<IChromosomeMetadata>();

            for (var i = 0; i < repeat; i++)
            {
                IChromosomeMetadata chromosomeMetadata = null;
                switch (element.Name.LocalName)
                {
                    case "BinaryChromosome":
                        chromosomeMetadata = ParseBinaryChromosome(element);
                        break;
                    case "PermutationChromosome":
                        chromosomeMetadata = ParsePermutationChromosome(element);
                        break;
                    case "NeuralChromosome":
                        chromosomeMetadata = ParseNeuralChromosome(element);
                        break;
                }

                ParseChromosomeAttributes(ref chromosomeMetadata, element, i);

                chromosomeMetadataList.Add(chromosomeMetadata);
            }

             return chromosomeMetadataList;
        }

        /// <summary>
        /// Parses a BinaryChromosome element.
        /// </summary>
        /// <param name="element">The BinaryChromosome element.</param>
        /// <returns>The parsed binary chromosome.</returns>
        private static IChromosomeMetadata ParseBinaryChromosome(XElement element)
        {
            var chromosomeMetadata = new BinaryChromosomeMetadata() as IChromosomeMetadata;

            ParseBinaryChromosomeAttributes(ref chromosomeMetadata, element);
            ParseOperators<BinaryCrossover, BinaryMutation>(ref chromosomeMetadata, element);

            return chromosomeMetadata;
        }

        // <summary>
        /// Parses a BinaryChromosome attributes.
        /// </summary>
        /// <param name="chromosomeMetadata">The chromosome metadata.</param>
        /// <param name="element">The BinaryChromosome element.</param>
        private static void ParseBinaryChromosomeAttributes(ref IChromosomeMetadata chromosomeMetadata, XElement element)
        {
            foreach (var attribute in element.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "GeneCount":
                        chromosomeMetadata.Properties["GeneCount"] = UInt32.Parse(attribute.Value);
                        break;
                }
            }
        }

        /// <summary>
        /// Parses a PermutationChromosome element.
        /// </summary>
        /// <param name="element">The PermutationChromosome element.</param>
        /// <returns>The parsed permutation chromosome.</returns>
        private static IChromosomeMetadata ParsePermutationChromosome(XElement element)
        {
            var chromosomeMetadata = new PermutationChromosomeMetadata() as IChromosomeMetadata;

            ParsePermutationChromosomeAttributes(ref chromosomeMetadata, element);
            ParseOperators<PermutationCrossover, PermutationMutation>(ref chromosomeMetadata, element);

            return chromosomeMetadata;
        }

        // <summary>
        /// Parses a PermutationChromosome attributes.
        /// </summary>
        /// <param name="chromosomeMetadata">The chromosome metadata.</param>
        /// <param name="element">The PermutationChromosome element.</param>
        private static void ParsePermutationChromosomeAttributes(ref IChromosomeMetadata chromosomeMetadata, XElement element)
        {
            foreach (var attribute in element.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "GeneCount":
                        chromosomeMetadata.Properties["GeneCount"] = UInt32.Parse(attribute.Value);
                        break;
                }
            }
        }

        /// <summary>
        /// Parses a BinaryChromosome element.
        /// </summary>
        /// <param name="element">The BinaryChromosome element.</param>
        /// <returns>The parsed neural chromosome.</returns>
        private static IChromosomeMetadata ParseNeuralChromosome(XElement element)
        {
            var chromosomeMetadata = new NeuralChromosomeMetadata() as IChromosomeMetadata;

            ParseNeuralChromosomeAttributes(ref chromosomeMetadata, element);
            ParseOperators<NeuralCrossover, NeuralMutation>(ref chromosomeMetadata, element);

            return chromosomeMetadata;
        }

        // <summary>
        /// Parses a NeuralChromosome attributes.
        /// </summary>
        /// <param name="chromosomeMetadata">The chromosome metadata.</param>
        /// <param name="element">The NeuralChromosome element.</param>
        private static void ParseNeuralChromosomeAttributes(ref IChromosomeMetadata chromosomeMetadata, XElement element)
        {
            foreach (var attribute in element.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "InputSize":
                        chromosomeMetadata.Properties["InputSize"] = UInt32.Parse(attribute.Value);
                        break;
                    case "OutputSize":
                        chromosomeMetadata.Properties["OutputSize"] = UInt32.Parse(attribute.Value);
                        break;
                    case "C1":
                        chromosomeMetadata.Properties["C1"] = Double.Parse(attribute.Value);
                        break;
                    case "C2":
                        chromosomeMetadata.Properties["C2"] = Double.Parse(attribute.Value);
                        break;
                    case "C3":
                        chromosomeMetadata.Properties["C3"] = Double.Parse(attribute.Value);
                        break;
                    case "ActivationFunction":
                        chromosomeMetadata.Properties["ActivationFunction"] = (int)Enum.Parse(typeof(ActivationFunction), attribute.Value);
                        break;
                }
            }
        }

        /// <summary>
        /// Parses the chromosome attributes.
        /// </summary>
        /// <param name="chromosomeMetadata">The chromosome metadata.</param>
        /// <param name="element">The chromosome element.</param>
        /// <param name="i">The current repeat count.</param>
        private static void ParseChromosomeAttributes(ref IChromosomeMetadata chromosomeMetadata, XElement element, int i)
        {
            foreach (var attribute in element.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "Name":
                        chromosomeMetadata.Name = Regex.Replace(attribute.Value, "%i", $"{i}");
                        break;
                    case "CrossoverRate":
                        chromosomeMetadata.CrossoverRate = Double.Parse(attribute.Value);
                        break;
                    case "MutationRate":
                        chromosomeMetadata.MutationRate = Double.Parse(attribute.Value);
                        break;
                }
            }
        }

        /// <summary>
        /// Parses the chromosome genetic operators.
        /// </summary>
        /// <typeparam name="TCrossover">The crossover operator type.</typeparam>
        /// <typeparam name="TMutation">The mutation operator type.</typeparam>
        /// <param name="chromosomeMetadata">The chromosome metadata.</param>
        /// <param name="element">The chromosome element.</param>
        private static void ParseOperators<TCrossover, TMutation>(ref IChromosomeMetadata chromosomeMetadata, XElement element)
        {
            foreach (var child in element.Elements())
            {
                switch (child.Name.LocalName)
                {
                    case "Crossovers":
                        ParseCrossoverOperators<TCrossover, TMutation>(ref chromosomeMetadata, child);
                        break;
                    case "Mutations":
                        ParseMutationOperators<TCrossover, TMutation>(ref chromosomeMetadata, child);
                        break;
                }
            }
        }

        /// <summary>
        /// Parses the chromosome crossover operators.
        /// </summary>
        /// <typeparam name="TCrossover">The crossover operator type.</typeparam>
        /// <typeparam name="TMutation">The mutation operator type.</typeparam>
        /// <param name="chromosomeMetadata">The chromosome metadata.</param>
        /// <param name="element">The chromosome element.</param>
        private static void ParseCrossoverOperators<TCrossover, TMutation>(ref IChromosomeMetadata chromosomeMetadata, XElement element)
        {
            foreach (var child in element.Elements())
            {
                var weightAttribute = child.Attributes().FirstOrDefault(attribute => attribute.Name.LocalName == "Weight");
                var weight = weightAttribute == null ? 1 : Double.Parse(weightAttribute.Value);

                var crossovers = ParseOperator<TCrossover>(child);

                var metadata = (ChromosomeMetadata<TCrossover, TMutation>)chromosomeMetadata;
                metadata.AddCrossoverOperators(crossovers, weight);
            }
        }

        /// <summary>
        /// Parses the chromosome mutation operators.
        /// </summary>
        /// <typeparam name="TCrossover">The crossover operator type.</typeparam>
        /// <typeparam name="TMutation">The mutation operator type.</typeparam>
        /// <param name="chromosomeMetadata">The chromosome metadata.</param>
        /// <param name="element">The chromosome element.</param>
        private static void ParseMutationOperators<TCrossover, TMutation>(ref IChromosomeMetadata chromosomeMetadata, XElement element)
        {
            foreach (var child in element.Elements())
            {
                var weightAttribute = child.Attributes().FirstOrDefault(attribute => attribute.Name.LocalName == "Weight");
                var weight = weightAttribute == null ? 1 : Double.Parse(weightAttribute.Value);

                var mutations = ParseOperator<TMutation>(child);

                var metadata = (ChromosomeMetadata<TCrossover, TMutation>)chromosomeMetadata;
                metadata.AddMutationOperators(mutations, weight);
            }
        }

        /// <summary>
        /// Parses chromosome operator elements.
        /// </summary>
        /// <typeparam name="TOperator">The operator type.</typeparam>
        /// <param name="element">The operator element.</param>
        /// <returns>The parsed operator.</returns>
        private static TOperator ParseOperator<TOperator>(XElement element)
        {
            var attributes = element.Attributes();
            var typeAttribute = attributes.FirstOrDefault(attribute => attribute.Name.LocalName == "Type");
            var type = Regex.Replace(typeAttribute.Value, " ", ", ");

            var operators = (TOperator)Enum.Parse(typeof(TOperator), type);

            return operators;
        }
    }
}
