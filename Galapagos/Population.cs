using System;
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
        private DataLogger _logger = null;

        private bool _nichesEnabled = false;
        private IList<Niche> _niches = new List<Niche>();
        private uint _distanceThreshold;

        /// <summary>
        /// Constructs a new instance of the <see cref="Population"/> class.
        /// </summary>
        /// <param name="creatureMetadata">The gmetadata of creatures belonging to the population.</param>
        public Population(CreatureMetadata creatureMetadata)
            : this(1000, creatureMetadata) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="Population"/> class.
        /// </summary>
        /// <param name="size">The population size.</param>
        /// <param name="creatureMetadata">The metadata of creatures belonging to the population.</param>
        public Population(uint size, CreatureMetadata creatureMetadata)
        {
            _creatures = new Creature[size];
            for (var i = 0; i < _creatures.Count(); i++)
                _creatures[i] = new Creature(creatureMetadata);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Population"/> class.
        /// </summary>
        /// <remarks>Python friendly constructor.</remarks>
        /// <param name="size">The population size.</param>
        /// <param name="creatureMetadata">The metadata of creatures belonging to the population.</param>
        public Population(int size, CreatureMetadata creatureMetadata)
            : this((uint)size, creatureMetadata)
        {
            if (size <= 0)
                throw new ArgumentException("Error! Gene count must be a positive value.");
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
            if (!_terminationConditions.ContainsKey($"{condition}"))
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
        /// <param name="path">The path to log data to.</param>
        public void EnableLogging(string path = "")
        {
            if (path != string.Empty) _logger = new DataLogger(path);
            _loggingEnabled = true;
        }

        /// <summary>
        /// Disables fitness logging.
        /// </summary>
        public void DisableLogging()
        {
            if (_logger != null) _logger = null;
            _loggingEnabled = false;
        }

        /// <summary>
        /// Enables niches in the population.
        /// </summary>
        /// <param name="distanceThreshold">The distance threshold for the niches.</param>
        public void EnableNiches(int distanceThreshold)
        {
            if (!_nichesEnabled)
                _nichesEnabled = true;
            _distanceThreshold = (uint)distanceThreshold;
        }

        /// <summary>
        /// Disables niches in the population.
        /// </summary>
        public void DisableNiches()
        {
            if (_nichesEnabled)
                _nichesEnabled = false;
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
        public void Evolve(SelectionAlgorithm selectionAlgorithm, object param, bool elitism = false, double survivalRate = 0.25)
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
        public void ParallelEvolve(SelectionAlgorithm selectionAlgorithm, object param, bool elitism = false, double survivalRate = 0.25)
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
                if (_nichesEnabled)
                    ClearNiches();

                var selection = GeneticFactory.ConstructSelectionAlgorithm(_creatures, selectionAlgorithm, param);
                BreedNewGeneration(selection, elitism, survivalRate);

                _generation++;
                _optimalCreature = FindOptimalCreature();

                if (_loggingEnabled)
                {
                    //Temp until better logging infrastructure is established.
                    var msg = $"Generation: {_generation}, Fitness: {OptimalCreature.Fitness}";
                    Console.WriteLine(msg);
                    System.Diagnostics.Debug.WriteLine(msg);
                    if (_logger != null) _logger.Log(_generation, OptimalCreature.Fitness);
                }

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

            if (_nichesEnabled)
                AssignNiches();
        }

        /// <summary>
        /// Assigns each creature in the population to a niche.
        /// </summary>
        private void AssignNiches()
        {
            foreach (var creature in _creatures)
            {
                var candidate = _niches.FirstOrDefault(niche => niche.Compatible(creature));
                if (candidate != null)
                    candidate.Add(creature);
                else
                    _niches.Add(new Niche(creature, _distanceThreshold));
            }

            var activeNiches = new List<Niche>();
            foreach(var niche in _niches)
            {
                if (niche.Size > 0)
                    activeNiches.Add(niche);
            }

            _niches = activeNiches;
        }

        /// <summary>
        /// Clears all niches of creatures.
        /// </summary>
        private void ClearNiches()
        {
            foreach (var niche in _niches)
                niche.Clear();
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
