using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galapagos.API;

namespace Galapagos.UnitTests
{
    public abstract class StochasticUnitTestBase : IStochastic
    {
        private readonly static Stochastic _stochastic = new Stochastic();

        public StochasticUnitTestBase()
        {
            Session.Instance.Stochastic = this;
        }

        protected void ExecuteUnitTest(Action ut)
        {
            ut();
            Reset();
        }

        private void Reset()
        {
            _EvaluateProbability = (probability) => _stochastic.EvaluateProbability(probability);
            _FlipCoin = () => _stochastic.FlipCoin();
            _Next = () => _stochastic.Next();
            _NextMax = (maxValue) => _stochastic.Next(maxValue);
            _NextMinMax = (minValue, maxValue) => _stochastic.Next(minValue, maxValue);
            _NextDouble = () => _stochastic.NextDouble();
        }

        protected Func<double, bool> _EvaluateProbability { get; set; } =
            (probability) => _stochastic.EvaluateProbability(probability);
        public bool EvaluateProbability(double probability) { return _EvaluateProbability(probability); }

        protected Func<bool> _FlipCoin { get; set; } =
            () => _stochastic.FlipCoin();
        public bool FlipCoin() { return _FlipCoin(); }


        protected Func<int> _Next { get; set; } =
            () => _stochastic.Next();
        public int Next() { return _Next(); }
        
        protected Func<int, int> _NextMax { get; set; } =
            (maxValue) => _stochastic.Next(maxValue);
        public int Next(uint maxValue) { return _NextMax((int)maxValue); }
        public int Next(double maxValue) { return _NextMax((int)maxValue); }
        public int Next(int maxValue) { return _NextMax((int)maxValue); }

        protected Func<int, int, int> _NextMinMax { get; set; } =
            (minValue, maxValue) => _stochastic.Next(minValue, maxValue);
        public int Next(double minValue, double maxValue) { return _NextMinMax((int)minValue, (int)maxValue); }
        public int Next(uint minValue, uint maxValue) { return _NextMinMax((int)minValue, (int)maxValue); }
        public int Next(int minValue, int maxValue) { return _NextMinMax((int)minValue, (int)maxValue); }

        protected Func<double> _NextDouble =
            () => _stochastic.NextDouble();
        public double NextDouble() { return _NextDouble(); }

        public T[] Shuffle<T>(T[] array)
        {
            return _stochastic.Shuffle<T>(array);
        }

        public T[] Shuffle<T>(T[] A, T[] B)
        {
            return _stochastic.Shuffle(A, B);
        }
    }
}
