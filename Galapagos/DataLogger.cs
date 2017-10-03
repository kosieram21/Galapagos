using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

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
            var parts = path.Split('\\');
            path = Path.HasExtension(path) ? Path.ChangeExtension(path, "csv") : $"{path}.csv";
            _path = parts.Count() != 1 ? path : Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
        }

        /// <summary>
        /// Logs data.
        /// </summary>
        /// <param name="generation">The generation number.</param>
        /// <param name="fitness">The best fitness in the population.</param>
        public void Log(int generation, double fitness)
        {
            if (!File.Exists(_path))
            {
                //File.Create(_path);
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
}
