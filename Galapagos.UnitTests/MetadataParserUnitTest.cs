using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Galapagos.API;

namespace Galapagos.UnitTests
{
    [TestClass]
    public class MetadataParserUnitTest
    {
        private const string FILE_PATH = @"C:\Users\kosie\Desktop\test.xml";

        [TestMethod]
        public void MetadataParseTest()
        {
            var metadata = Session.Instance.LoadMetadata(FILE_PATH, null);
        }
    }
}
