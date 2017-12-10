using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Galapagos.API;
using Galapagos.API.Factory;
using System.Text.RegularExpressions;

namespace Galapagos.Metadata.Parser
{
    //TODO: Clean up chromosome metadata parse logic.

    internal static class MetadataParser
    {
        /// <summary>
        /// Parses the given population metadata file.
        /// </summary>
        /// <param name="xDoc">The metadata document.</param>
        /// <returns>The parsed population metadata.</returns>
        public static IPopulationMetadata Parse(XDocument xDoc)
        {
            var root = XElement.Parse(xDoc.ToString());

            if (root.Name.ToString() != "Population")
                throw new ArgumentException("Error! Population must be the root metadata element.");

            return ParsePopulationElement(root);
        }

        /// <summary>
        /// Parses a Population element.
        /// </summary>
        /// <param name="element">The Population element.</param>
        private static IPopulationMetadata ParsePopulationElement(XElement element)
        {
            var populationMetadata = new PopulationMetadata();

            foreach(var attribute in element.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "Size":
                        populationMetadata.Size = UInt32.Parse(attribute.Value);
                        break;
                    case "SurvivalRate":
                        populationMetadata.SurvivalRate = Double.Parse(attribute.Value);
                        break;
                    case "DistanceThreshold":
                        populationMetadata.DistanceThreshold = UInt32.Parse(attribute.Value);
                        break;
                    case "CooperativeCoevolution":
                        populationMetadata.CooperativeCoevolution = Boolean.Parse(attribute.Value);
                        break;
                    default:
                        throw new ArgumentException($"Error! {attribute.Name} is not a valid Population attribute.");
                }
            }

            foreach(var child in element.Elements())
            {
                switch(child.Name.ToString())
                {
                    case "SelectionAlgorithm":
                        populationMetadata.SelectionAlgorithm = ParseSelectionAlgorithm(child);
                        break;
                    case "TerminationConditions":
                        var terminationConditions = ParseTerminationConditions(child);
                        foreach (var condition in terminationConditions)
                            populationMetadata.AddTerminationCondition(condition);
                        break;
                    case "Chromosomes":
                        var chromosomes = ParseChromosomes(child);
                        foreach (var chromosome in chromosomes)
                            populationMetadata.AddChromosomeMetadata(chromosome);
                        break;
                    default:
                        throw new AggregateException($"Error! {child.Name} is not a valid child element of Population.");
                }
            }

            return populationMetadata;
        }

        /// <summary>
        /// Parses a SelectionAlgorithm element.
        /// </summary>
        /// <param name="element">The SelectionAlgorithm element.</param>
        /// <returns>The parsed selection algorithm.</returns>
        private static ISelectionAlgorithm ParseSelectionAlgorithm(XElement element)
        {
            var attributes = element.Attributes();
            var typeAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Type");
            var argAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Arg");

            if (typeAttribute == null)
                throw new ArgumentException("Error! SelectionAlgorithm element must contain a Type attribute.");

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
            {
                if (child.Name.ToString() != "TerminationCondition")
                    throw new ArgumentException($"Error! {child.Name} is not a valid child element of TerminationConditions.");

                terminationConditions.Add(ParseTerminationCondition(child));
            }

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
            var typeAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Type");
            var argAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Arg");

            if (typeAttribute == null)
                throw new ArgumentException("Error! TerminationCondition element must contain a Type attribute.");

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
            {
                if (child.Name.ToString() != "Chromosome")
                    throw new ArgumentException($"Error! {child.Name} is not a valid child element of Chromosomes.");

                chromosomes.AddRange(ParseChromosome(child));
            }

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
            var nameAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Name");
            var typeAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Type");
            var repeatAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Repeat");

            if (nameAttribute == null)
                throw new ArgumentException("Error! Chromosome element must contain a Name attribute.");

            if (typeAttribute == null)
                throw new ArgumentException("Error! Chromosome element must contain a Type attribute.");

            var type = (ChromosomeType)Enum.Parse(typeof(ChromosomeType), typeAttribute.Value);
            var repeat = repeatAttribute == null ? 1 : Int32.Parse(repeatAttribute.Value);

            if (repeat < 0)
                throw new ArgumentException("Error! Repeat attribute must be a positive value.");

            var chromosomeMetadataList = new List<IChromosomeMetadata>();

            for (var i = 0; i < repeat; i++)
            {

                ChromosomeMetadata chromosomeMetadata = 
                    type == ChromosomeType.Binary ? new BinaryChromosomeMetadata() as ChromosomeMetadata :
                    type == ChromosomeType.Permutation ? new PermutationChromosomeMetadata() as ChromosomeMetadata :
                    type == ChromosomeType.Neural ? new NeuralChromosomeMetadata() as NeuralChromosomeMetadata :
                    null;

                if (chromosomeMetadata == null)
                    throw new ArgumentException("Error! Invalid chromosome type.");

                foreach (var attribute in element.Attributes())
                {
                    switch (attribute.Name.ToString())
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
                        case "GeneCount":
                            chromosomeMetadata.Properties["GeneCount"] = UInt32.Parse(attribute.Value);
                            break;
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
                        case "Type":
                        case "Repeat":
                            break;
                        default:
                            throw new ArgumentException($"Error! {attribute.Name} is not a valid Chromosome attribute.");
                    }
                }

                foreach (var child in element.Elements())
                {
                    switch (child.Name.ToString())
                    {
                        case "Crossovers":
                            if (type == ChromosomeType.Binary)
                                ParseBinaryCrossovers(chromosomeMetadata, child);
                            else if (type == ChromosomeType.Permutation)
                                ParsePermutationCrossovers(chromosomeMetadata, child);
                            else if (type == ChromosomeType.Neural)
                                ParseNeuralCrossovers(chromosomeMetadata, child);
                            break;
                        case "Mutations":
                            if (type == ChromosomeType.Binary)
                                ParseBinaryMutations(chromosomeMetadata, child);
                            else if (type == ChromosomeType.Permutation)
                                ParsePermutationMutations(chromosomeMetadata, child);
                            else if (type == ChromosomeType.Neural)
                                ParseNeuralMutations(chromosomeMetadata, child);
                            break;
                        default:
                            throw new AggregateException($"Error! {child.Name} is not a valid child element of Chromosome.");
                    }
                }

                chromosomeMetadataList.Add(chromosomeMetadata as IChromosomeMetadata);
            }

            return chromosomeMetadataList;
        }

        /// <summary>
        /// Parses a binary cromosome Crossover elements.
        /// </summary>
        /// <param name="metadata">The chromosome metadata.</param>
        /// <param name="element">The Crossovers element.</param>
        private static void ParseBinaryCrossovers(IChromosomeMetadata metadata, XElement element)
        {
            foreach(var child in element.Elements())
            {
                if (child.Name.ToString() != "Crossover")
                    throw new ArgumentException($"Error! {child.Name} is not a valid child element of Crossovers.");

                var attributes = child.Attributes();
                var typeAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Type");
                var weightAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Weight");

                if (typeAttribute == null)
                    throw new ArgumentException($"Error! Crossover element must contain a type attribute");

                var crossovers = (BinaryCrossover)Enum.Parse(typeof(BinaryCrossover), typeAttribute.Value);

                if(weightAttribute != null)
                {
                    var weight = UInt32.Parse(weightAttribute.Value);
                    ((BinaryChromosomeMetadata)metadata).AddCrossoverOperators(crossovers, weight);
                }
                else
                {
                    ((BinaryChromosomeMetadata)metadata).AddCrossoverOperators(crossovers);
                }
            }
        }

        /// <summary>
        /// Parses a permutation cromosome Crossover elements.
        /// </summary>
        /// <param name="metadata">The chromosome metadata.</param>
        /// <param name="element">The Crossovers element.</param>
        private static void ParsePermutationCrossovers(IChromosomeMetadata metadata, XElement element)
        {
            foreach (var child in element.Elements())
            {
                if (child.Name.ToString() != "Crossover")
                    throw new ArgumentException($"Error! {child.Name} is not a valid child element of Crossovers.");

                var attributes = child.Attributes();
                var typeAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Type");
                var weightAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Weight");

                if (typeAttribute == null)
                    throw new ArgumentException($"Error! Crossover element must contain a type attribute");

                var crossovers = (PermutationCrossover)Enum.Parse(typeof(PermutationCrossover), typeAttribute.Value);

                if (weightAttribute != null)
                {
                    var weight = UInt32.Parse(weightAttribute.Value);
                    ((PermutationChromosomeMetadata)metadata).AddCrossoverOperators(crossovers, weight);
                }
                else
                {
                    ((PermutationChromosomeMetadata)metadata).AddCrossoverOperators(crossovers);
                }
            }
        }

        /// <summary>
        /// Parses a neural cromosome Crossover elements.
        /// </summary>
        /// <param name="metadata">The chromosome metadata.</param>
        /// <param name="element">The Crossovers element.</param>
        private static void ParseNeuralCrossovers(IChromosomeMetadata metadata, XElement element)
        {
            foreach (var child in element.Elements())
            {
                if (child.Name.ToString() != "Crossover")
                    throw new ArgumentException($"Error! {child.Name} is not a valid child element of Crossovers.");

                var attributes = child.Attributes();
                var typeAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Type");
                var weightAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Weight");

                if (typeAttribute == null)
                    throw new ArgumentException($"Error! Crossover element must contain a type attribute");

                var crossovers = (NeuralCrossover)Enum.Parse(typeof(NeuralCrossover), typeAttribute.Value);

                if (weightAttribute != null)
                {
                    var weight = UInt32.Parse(weightAttribute.Value);
                    ((NeuralChromosomeMetadata)metadata).AddCrossoverOperators(crossovers, weight);
                }
                else
                {
                    ((NeuralChromosomeMetadata)metadata).AddCrossoverOperators(crossovers);
                }
            }
        }

        /// <summary>
        /// Parses a binary cromosome Mutation elements.
        /// </summary>
        /// <param name="metadata">The chromosome metadata.</param>
        /// <param name="element">The Mutations element.</param>
        private static void ParseBinaryMutations(IChromosomeMetadata metadata, XElement element)
        {
            foreach (var child in element.Elements())
            {
                if (child.Name.ToString() != "Mutation")
                    throw new ArgumentException($"Error! {child.Name} is not a valid child element of Mutations.");

                var attributes = child.Attributes();
                var typeAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Type");
                var weightAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Weight");

                if (typeAttribute == null)
                    throw new ArgumentException($"Error! Mutation element must contain a type attribute");

                var mutations = (BinaryMutation)Enum.Parse(typeof(BinaryMutation), typeAttribute.Value);

                if (weightAttribute != null)
                {
                    var weight = UInt32.Parse(weightAttribute.Value);
                    ((BinaryChromosomeMetadata)metadata).AddMutationOperators(mutations, weight);
                }
                else
                {
                    ((BinaryChromosomeMetadata)metadata).AddMutationOperators(mutations);
                }
            }
        }

        /// <summary>
        /// Parses a permutation cromosome Mutation elements.
        /// </summary>
        /// <param name="metadata">The chromosome metadata.</param>
        /// <param name="element">The Mutations element.</param>
        private static void ParsePermutationMutations(IChromosomeMetadata metadata, XElement element)
        {
            foreach (var child in element.Elements())
            {
                if (child.Name.ToString() != "Mutation")
                    throw new ArgumentException($"Error! {child.Name} is not a valid child element of Mutations.");

                var attributes = child.Attributes();
                var typeAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Type");
                var weightAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Weight");

                if (typeAttribute == null)
                    throw new ArgumentException($"Error! Mutation element must contain a type attribute");

                var mutations = (PermutationMutation)Enum.Parse(typeof(PermutationMutation), typeAttribute.Value);

                if (weightAttribute != null)
                {
                    var weight = UInt32.Parse(weightAttribute.Value);
                    ((PermutationChromosomeMetadata)metadata).AddMutationOperators(mutations, weight);
                }
                else
                {
                    ((PermutationChromosomeMetadata)metadata).AddMutationOperators(mutations);
                }
            }
        }

        /// <summary>
        /// Parses a neural cromosome Mutation elements.
        /// </summary>
        /// <param name="metadata">The chromosome metadata.</param>
        /// <param name="element">The Mutations element.</param>
        private static void ParseNeuralMutations(IChromosomeMetadata metadata, XElement element)
        {
            foreach (var child in element.Elements())
            {
                if (child.Name.ToString() != "Mutation")
                    throw new ArgumentException($"Error! {child.Name} is not a valid child element of Mutations.");

                var attributes = child.Attributes();
                var typeAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Type");
                var weightAttribute = attributes.FirstOrDefault(attribute => attribute.Name.ToString() == "Weight");

                if (typeAttribute == null)
                    throw new ArgumentException($"Error! Mutation element must contain a type attribute");

                var mutations = (NeuralMutation)Enum.Parse(typeof(NeuralMutation), typeAttribute.Value);

                if (weightAttribute != null)
                {
                    var weight = UInt32.Parse(weightAttribute.Value);
                    ((NeuralChromosomeMetadata)metadata).AddMutationOperators(mutations, weight);
                }
                else
                {
                    ((NeuralChromosomeMetadata)metadata).AddMutationOperators(mutations);
                }
            }
        }
    }
}
