using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.Metadata.Parser;

namespace Galapagos.UnitTests
{
    [TestClass]
    public class MetadataParserUnitTest
    {
        private const string FILE_PATH = @"C:\Users\kosie\Desktop\test.xml";

        [TestMethod]
        public void MetadataParseTest()
        {
            var metadata = MetadataParser.Parse(FILE_PATH);
        }
    }
}
