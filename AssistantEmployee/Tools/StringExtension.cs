using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace AssistantEmployee.Tools
{
    static class StringExtension
    {
        public static string OnlyNumbersToConvert(this string line)
        {
            char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            if (string.IsNullOrEmpty(line)) return string.Empty;
            bool foundNumber = false;
            bool foundPoint = false;
            StringBuilder result = new StringBuilder();
            foreach (char c in line)
            {
                if ((!foundPoint && result.Length > 28) || (foundPoint && result.Length > 29)) break;
                if (foundNumber && !foundPoint && (c.Equals('.') || c.Equals(',')))
                {
                    foundPoint = true;
                    result.Append(',');
                }
                foreach (char number in numbers)
                {
                    if (c.Equals(number))
                    {
                        if (!foundNumber) foundNumber = true;
                        result.Append(c);
                        break;
                    }
                }
            }
            return result.ToString();
        }

        public static TOutput XamlStringToObject<TOutput>(this string xmlString)
        {
            XamlSchemaContext schemaContext = System.Windows.Markup.XamlReader.GetWpfSchemaContext();
            XamlXmlReaderSettings settings = new XamlXmlReaderSettings()
            {
                LocalAssembly = Assembly.GetExecutingAssembly()
            };
            using (XamlXmlReader reader = new XamlXmlReader(new StringReader(xmlString), schemaContext, settings))
            using (XamlObjectWriter writer = new XamlObjectWriter(reader.SchemaContext))
            {
                while (reader.Read()) writer.WriteNode(reader);
                return (TOutput)writer.Result;
            }
        }
    }
}