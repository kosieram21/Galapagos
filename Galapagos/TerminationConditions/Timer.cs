using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galapagos.TerminationConditions
{
    /// <summary>
    /// Timer termination condition.
    /// </summary>
    internal class Timer : ITerminationCondition
    {
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private readonly TimeSpan _stopTime;

        /// <summary>
        /// Constructs a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="stopTime">The stop time.</param>
        public Timer(TimeSpan stopTime)
        {
            _stopTime = stopTime;
        }

        /// <summary>
        /// Destructs an instance of the <see cref="Timer"/> class.
        /// </summary>
        ~Timer()
        {
            if (_stopWatch.IsRunning)
                _stopWatch.Stop();
        }

        /// <summary>
        /// Checks the termination condition.
        /// </summary>
        /// <returns>A value indicating if evolution should terminate.</returns>
        public bool Check()
        {
            if (!_stopWatch.IsRunning)
                _stopWatch.Start();
            return _stopWatch.Elapsed > _stopTime;
        }
    }
}
