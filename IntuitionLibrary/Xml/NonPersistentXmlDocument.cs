using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QBits.Intuition.Xml
{
    /// <summary>Class for manipulating non-persistence in XML</summary>
    public class NonPersistentXMLDocument : SimpleXmlDocument
    {
        /// <summary>Default constructor.</summary>
        /// <param name="rootNodeName"></param>
        public NonPersistentXMLDocument(string rootNodeName)
            : base(null, rootNodeName)
        {
        }
        /// <summary>Creates the non-persistent node of the specified name.</summary>
        public XmlNode CreateNode(string name)
        {
            return CreateChildNode(XmlNodeType.Element, name, null);
        }
        /// <summary>Creates a node under <paramref name="currentNode"/>.</summary>
        public XmlNode CreateTextNode(XmlNode currentNode, string name, string text)
        {
            XmlNode child = CreateChildNode(currentNode, name);
            child.AppendChild(Document.CreateTextNode(text));
            return child;
        }
    }
}
