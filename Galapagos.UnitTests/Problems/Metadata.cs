using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Galapagos.UnitTests.Problems
{
    public static class Metadata
    {
        private const string NAMESPACE = @"Galapagos.UnitTests.Problems.Metadata";

        private static Stream LoadEmbeddedResource(string name)
        {
            var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream($"{NAMESPACE}.{name}.xml");

            return stream;
        }

        public static Stream NQueens => LoadEmbeddedResource(@"NQueens");

        public static Stream Sudoku => LoadEmbeddedResource(@"Sudoku");

        public static Stream Cryptarithmetic => LoadEmbeddedResource(@"Cryptarithmetic");

        public static Stream WumpusWorld => LoadEmbeddedResource(@"WumpusWorld");
    }
}
