﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Galapagos.API;

namespace Galapagos
{
    /// <summary>
    /// Utility class for logging algorithm data.
    /// </summary>
    internal class DataLogger
    {
        private readonly string _path = string.Empty;

        /// <summary>
        /// Constructs a new instance of the <see cref="DataLogger"/> class.
        /// </summary>
        /// <param name="path">The file path.</param>
        public DataLogger(string path = "")
        {
            if (path != string.Empty)
            {
                var parts = path.Split('\\');
                path = Path.HasExtension(path) ? Path.ChangeExtension(path, "csv") : $"{path}.csv";
                _path = parts.Count() != 1 ? path : Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
            }
            else
            {
                _path = path;
            }
        }

        /// <summary>
        /// Logs data.
        /// </summary>
        /// <param name="generation">The generation number.</param>
        /// <param name="fitness">The best fitness in the population.</param>
        public void Log(int generation, double fitness)
        {
            if (Session.Instance.LogToConsole)
            {
                var msg = $"Generation: {generation}, Fitness: {fitness}";
                Console.WriteLine(msg);
                System.Diagnostics.Debug.WriteLine(msg);
            }

            if (_path != string.Empty)
            {
                if (!File.Exists(_path))
                {
                    using (StreamWriter file = new StreamWriter(_path, true))
                    {
                        file.WriteLine("Generation,Fitness");
                        file.Close();
                    }
                }

                using (StreamWriter file = new StreamWriter(_path, true))
                {
                    file.WriteLine($"{generation},{fitness}");
                    file.Close();
                }
            }
        }

        /// <summary>
        /// Computes the niche size gap.
        /// </summary>
        /// <param name="niches">The niches.</param>
        /// <returns>The niche size gap.</returns>
        private double ComputeNicheSizeGap(IReadOnlyList<Niche> niches)
        {
            if (niches.Count == 0)
                return 0;

            var max = niches.Max(niche => niche.Count());
            var min = niches.Min(niche => niche.Count());

            return Math.Abs(max - min);
        }
    }
}
