﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.SelectionAlgorithms;
using Galapagos.TerminationConditions;

namespace Galapagos
{
    /// <summary>
    /// A population of creatures.
    /// </summary>
    public class Population : IEnumerable<Creature>
    {
        private Creature _optimalCreature;
        private readonly Creature[] _creatures;

        private int _generation = 0;

        private readonly IDictionary<string, ITerminationCondition> _terminationConditions = new Dictionary<string, ITerminationCondition>();

        private bool _loggingEnabled = false;

        /// <summary>
        /// Constructs a new instance of the <see cref="Population"/> class.
        /// </summary>
        /// <param name="creatureDescription">The genetic description of creatures belonging to the population.</param>
        public Population(GeneticDescription creatureDescription)
            : this(1000, creatureDescription) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="Population"/> class.
        /// </summary>
        /// <param name="size">The population size.</param>
        /// <param name="creatureDescription">The genetic description of creatures belonging to the population.</param>
        public Population(uint size, GeneticDescription creatureDescription)
        {
            _creatures = new Creature[size];
            for (var i = 0; i < _creatures.Count(); i++)
                _creatures[i] = new Creature(creatureDescription);
        }

        /// <summary>
        /// Gets the population size.
        /// </summary>
        public int Size => _creatures.Count();

        /// <summary>
        /// Gets the population generation.
        /// </summary>
        public int Generation => _generation;

        /// <summary>
        /// Accesses a creature from the population.
        /// </summary>
        /// <param name="index">The creature index.</param>
        /// <returns>The creature.</returns>
        public Creature this[int index]
        {
            get
            {
                if (index >= Size)
                    throw new Exception($"Error! {index} is larger than the population size.");
                return _creatures[index];
            }
        }

        /// <summary>
        /// Gets or sets the creature best suited to solve the propblem.
        /// </summary>
        public Creature OptimalCreature
        {
            get
            {
                if (_optimalCreature == null)
                    _optimalCreature = FindOptimalCreature();
                return _optimalCreature;
            }
            private set
            {
                if (_optimalCreature == null || value.Fitness > _optimalCreature.Fitness)
                    _optimalCreature = value;
            }
        }

        /// <summary>
        /// Finds the optimal creature.
        /// </summary>
        private Creature FindOptimalCreature()
        {
            var optimalCreature = _creatures[0];

            for (var i = 1; i < Size; i++)
            {
                if (_creatures[i].Fitness > optimalCreature.Fitness)
                    optimalCreature = _creatures[i];
            }

            return optimalCreature;
        }

        /// <summary>
        /// Registers an evolution termination condition.
        /// </summary>
        /// <param name="condition">The termination condition to set.</param>
        /// <param name="param">The termination condition parameter.</param>
        public void RegisterTerminationCondition(TerminationCondition condition, object param)
        {
            _terminationConditions[$"{condition}"] = GeneticFactory.ConstructTerminationCondition(this, condition, param);
        }

        /// <summary>
        /// Unregisters an evolution termination condition.
        /// </summary>
        /// <param name="condition">The termination condition to set.</param>
        public void UnregisterTerminationCondition(TerminationCondition condition)
        {
            if (_terminationConditions.ContainsKey($"{condition}"))
                _terminationConditions.Remove($"{condition}");
        }

        /// <summary>
        /// Enables fitness logging.
        /// </summary>
        public void EnableLogging()
        {
            _loggingEnabled = true;
        }

        /// <summary>
        /// Disables fitness logging.
        /// </summary>
        public void DisableLogging()
        {
            _loggingEnabled = false;
        }

        /// <summary>
        /// Optimizes the population.
        /// </summary>
        /// <param name="selectionAlgorithm">The selection algorithm to use.</param>
        /// <param name="elitism">A value indicating if elitism should be used.</param>
        /// <param name="survivalRate">The percentage of the population to carry over is elitism is enabled.</param>
        public void Evolve(SelectionAlgorithm selectionAlgorithm = SelectionAlgorithm.FitnessProportionate, bool elitism = false, double survivalRate = 0.25)
        {
            Evolve(selectionAlgorithm, null, elitism, survivalRate);
        }

        /// <summary>
        /// Optimizes the population.
        /// </summary>
        /// <param name="selectionAlgorithm">The selection algorithm to use.</param>
        /// <param name="param">The selection algorithm parameter.</param>
        /// <param name="elitism">A value indicating if elitism should be used.</param>
        /// <param name="survivalRate">The percentage of the population to carry over is elitism is enabled.</param>
        public void Evolve(SelectionAlgorithm selectionAlgorithm, object param, bool elitism, double survivalRate)
        {
            Action evaluateFitness = () => { foreach (var creature in _creatures) { creature.EvaluateFitness(); } };
            RunEvolution(selectionAlgorithm, param, elitism, survivalRate, evaluateFitness);
        }

        /// <summary>
        /// Optimizes the population. Evaluates creature fitness values in parallel.
        /// </summary>
        /// <param name="selectionAlgorithm">The selection algorithm to use.</param>
        /// <param name="elitism">A value indicating if elitism should be used.</param>
        /// <param name="survivalRate">The percentage of the population to carry over is elitism is enabled.</param>
        public void ParallelEvolve(SelectionAlgorithm selectionAlgorithm = SelectionAlgorithm.FitnessProportionate, bool elitism = false, double survivalRate = 0.25)
        {
            ParallelEvolve(selectionAlgorithm, null, elitism, survivalRate);
        }

        /// <summary>
        /// Optimizes the population. Evaluates creature fitness values in parallel.
        /// </summary>
        /// <param name="selectionAlgorithm">The selection algorithm to use.</param>
        /// <param name="param">The selection algorithm parameter.</param>
        /// <param name="elitism">A value indicating if elitism should be used.</param>
        /// <param name="survivalRate">The percentage of the population to carry over is elitism is enabled.</param>
        public void ParallelEvolve(SelectionAlgorithm selectionAlgorithm, object param, bool elitism, double survivalRate)
        {
            Action evaluateFitness = () => { Parallel.ForEach(_creatures, (creature) => { creature.EvaluateFitness(); }); };
            RunEvolution(selectionAlgorithm, param, elitism, survivalRate, evaluateFitness);
        }

        /// <summary>
        /// Runs the evolution process.
        /// </summary>
        /// <param name="selectionAlgorithm">The selection algorithm to use.</param>
        /// <param name="param">The selection algorithm parameter.</param>
        /// <param name="elitism">A value indicating if elitism should be used.</param>
        /// <param name="survivalRate">The percentage of the population to carry over is elitism is enabled.</param>
        /// <param name="evaluateFitness">A delegate that evaluates creature fitness.</param>
        private void RunEvolution(SelectionAlgorithm selectionAlgorithm, object param, bool elitism, double survivalRate, Action evaluateFitness)
        {
            if (survivalRate < 0 || survivalRate > 1)
                throw new ArgumentException("Error! Survival rate must be a value between 0 and 1.");

            if (_terminationConditions.Count == 0)
                _terminationConditions[$"{TerminationCondition.GenerationThreshold}"] = new GenerationThreshold(this, 1000);

            while (true)
            {
                evaluateFitness();
                var selection = GeneticFactory.ConstructSelectionAlgorithm(_creatures, selectionAlgorithm, param);
                BreedNewGeneration(selection, elitism, survivalRate);

                _generation++;
                _optimalCreature = FindOptimalCreature();

                if (_loggingEnabled)
                    Console.WriteLine($"Generation: {_generation}, Fitness: {OptimalCreature.Fitness}");

                if (_terminationConditions.Any(kvp => kvp.Value.Check()))
                    break;
            }
        }

        /// <summary>
        /// Breeds a new generation of creatures.
        /// </summary>
        /// <param name="selection">The selection algorithm.</param>
        /// <param name="elitism">A value indicating if elitism should be used.</param>
        /// <param name="survivalRate">The percentage of the population to carry over is elitism is enabled.</param>
        private void BreedNewGeneration(ISelectionAlgorithm selection, bool elitism, double survivalRate)
        {
            var newGeneration = new Creature[Size];

            var i = 0;
            if(elitism)
            {
                var sortedCreatures = _creatures.OrderByDescending(creature => creature.Fitness).ToArray();
                for (var j = 0; j < Size * survivalRate; j++)
                {
                    newGeneration[j] = sortedCreatures[j];
                    i++;
                }
            }

            while (i < Size)
            {
                var parentX = selection.Invoke();
                var parentY = selection.Invoke();
                newGeneration[i] = parentX.Breed(parentY);
                i++;
            }

            Array.Copy(newGeneration, _creatures, Size);
        }

        #region IEnumerable Members

        public IEnumerator<Creature> GetEnumerator()
        {
            return _creatures.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
