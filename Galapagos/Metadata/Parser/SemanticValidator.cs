using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.Metadata.Parser
{
    internal static class SemanticValidator
    {
        /// <summary>
        /// Performs semantic validation on the given <see cref="IPopulationMetadata"/>.
        /// </summary>
        /// <param name="populationMetadata">The given <see cref="IPopulationMetadata"/>.</param>
        internal static void Validate(IPopulationMetadata populationMetadata)
        {
            var seen = InitializeSeenTracker(populationMetadata);

            ValidatePopulationProperties(populationMetadata);
            foreach (var chromosomeMetadata in populationMetadata)
            {
                seen(chromosomeMetadata.Name);
                ValidateChromosomeProperties(chromosomeMetadata);
            }
        }

        /// <summary>
        /// Initializes the seen tracker.
        /// </summary>
        /// <param name="populationMetadata">The population metadata to initialize against.</param>
        /// <returns>The seen tracker.</returns>
        private static Action<string> InitializeSeenTracker(IPopulationMetadata populationMetadata)
        {
            var seen = new Dictionary<string, bool>();
            foreach (var chromosomeMetadata in populationMetadata)
                seen[chromosomeMetadata.Name] = false;

            return name =>
            {
                if (seen[name])
                    throw new ArgumentException("Error! Every chromosome must have a unique name.");
                else
                    seen[name] = true;
            };
        }

        /// <summary>
        /// Validates the population metadata properties.
        /// </summary>
        /// <param name="populationMetadata">The population metadata to validate.</param>
        private static void ValidatePopulationProperties(IPopulationMetadata populationMetadata)
        {
            if (populationMetadata.SurvivalRate < 0 || populationMetadata.SurvivalRate > 1)
                throw new ArgumentException("Error! Survival rate must be a value between 0 and 1.");
            if (populationMetadata.DistanceThreshold < 0)
                throw new ArgumentException("Error! Distance threshold must be a positive value.");
        }

        /// <summary>
        /// Validates the chromosome metadata properties.
        /// </summary>
        /// <param name="chromosomeMetadata">The chromosome metatadata to validate.</param>
        private static void ValidateChromosomeProperties(IChromosomeMetadata chromosomeMetadata)
        {
            if (chromosomeMetadata.CrossoverRate < 0 || chromosomeMetadata.CrossoverRate > 1)
                throw new ArgumentException("Error! Crossover rate must be a value between 0 and 1.");
            if (chromosomeMetadata.MutationRate < 0 || chromosomeMetadata.MutationRate > 1)
                throw new ArgumentException("Error! Mutation rate must be a value between 0 and 1.");

            switch (chromosomeMetadata.Type)
            {
                case ChromosomeType.Binary:
                    ValidateBinaryChromosomeProperties(chromosomeMetadata);
                    break;
                case ChromosomeType.Permutation:
                    ValidatePermutationChromosomeProperties(chromosomeMetadata);
                    break;
                case ChromosomeType.Neural:
                    ValidateNeuralChromosomeProperties(chromosomeMetadata);
                    break;
            }
        }

        /// <summary>
        /// Validates the binary chromosome metadata properties.
        /// </summary>
        /// <param name="chromosomeMetadata">The chromosome metatadata to validate.</param>
        private static void ValidateBinaryChromosomeProperties(IChromosomeMetadata chromosomeMetadata)
        {
            ValidatePropertyExists(chromosomeMetadata, "GeneCount");
        }

        /// <summary>
        /// Validates the permutation chromosome metadata properties.
        /// </summary>
        /// <param name="chromosomeMetadata">The chromosome metatadata to validate.</param>
        private static void ValidatePermutationChromosomeProperties(IChromosomeMetadata chromosomeMetadata)
        {
            ValidatePropertyExists(chromosomeMetadata, "GeneCount");
        }

        /// <summary>
        /// Validates the neural chromosome metadata properties.
        /// </summary>
        /// <param name="chromosomeMetadata">The chromosome metatadata to validate.</param>
        private static void ValidateNeuralChromosomeProperties(IChromosomeMetadata chromosomeMetadata)
        {
            ValidatePropertyExists(chromosomeMetadata, "InputSize");
            ValidatePropertyExists(chromosomeMetadata, "OutputSize");
            ValidatePropertyExists(chromosomeMetadata, "C1");
            ValidatePropertyExists(chromosomeMetadata, "C2");
            ValidatePropertyExists(chromosomeMetadata, "C3");

            if (chromosomeMetadata.Properties["C1"] < 0)
                throw new ArgumentException("Error! C1 must be a positive value.");
            if (chromosomeMetadata.Properties["C2"] < 0)
                throw new ArgumentException("Error! C2 must be a positive value.");
            if (chromosomeMetadata.Properties["C3"] < 0)
                throw new ArgumentException("Error! C3 must be a positive value.");
        }

        /// <summary>
        /// Validate that the given property existis in the metadata.
        /// </summary>
        /// <param name="chromosomeMetadata">The chromosome metatadata to validate.</param>
        /// <param name="name">The property name.</param>
        private static void ValidatePropertyExists(IChromosomeMetadata chromosomeMetadata, string name)
        {
            if (!chromosomeMetadata.Properties.ContainsKey(name))
                throw new ArgumentException($"Error! Binary chromosome metadata must contain the {name} property");
        }
    }
}
