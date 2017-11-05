using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.Metadata.Parser;

namespace Galapagos.UnitTests
{
    [TestClass]
    public class MetadataParserUnitTest
    {
        private const string FILE_PATH = @"C:\Users\kosie\Desktop\test.meta";

        [TestMethod]
        public void TestMethod1()
        {
            MetadataParser.Parse(FILE_PATH);
        }
    }
}
