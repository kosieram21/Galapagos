using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Reflection;
using System.IO;

namespace Galapagos.Metadata.Parser
{
    internal static class SyntacticValidator
    {
        /// <summary>
        /// Performs syntactic validation on the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="xDoc">The <see cref="XDocument"/> to validate.</param>
        internal static void Validate(XDocument xDoc)
        {
            var schema = LoadEmbeddedSchema();
            Validate(xDoc, schema);
        }

        /// <summary>
        /// Performs syntactic validation on the given <see cref="XDocument"/> 
        /// angainst the given <see cref="XmlSchema"/>.
        /// </summary>
        /// <param name="xDoc">The <see cref="XDocument"/> to validate.</param>
        /// <param name="schema">The <see cref="XmlSchema"/> to validate against.</param>
        private static void Validate(XDocument xDoc, XmlSchema schema)
        {
            var set = new XmlSchemaSet();
            set.Add(schema);
            xDoc.Validate(set, OnValidationEvent);
        }

        /// <summary>
        /// Loads the embedded XSD schema for Galapagos metadata.
        /// </summary>
        /// <returns>The embedded XSD schema.</returns>
        private static XmlSchema LoadEmbeddedSchema()
        {
            var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream(@"Galapagos.Metadata.Parser.Schema.xsd");
            var schema = XmlSchema.Read(stream, OnValidationEvent);

            return schema;
        }

        /// <summary>
        /// Handles the [Validation Event].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private static void OnValidationEvent(object sender, ValidationEventArgs e)
        {
            if (e.Exception != null)
                throw e.Exception;
        }
    }
}
