using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QBits.Intuition.Xml
{
    public class NonPersistentXMLDocument : SimpleXmlDocument
    {
        public NonPersistentXMLDocument(string rootNodeName)
            : base(null, rootNodeName)
        {
        }
        public XmlNode CreateNode(string name)
        {
            return CreateChildNode(XmlNodeType.Element, name, null);
        }
        public XmlNode CreateTextNode(XmlNode currentNode, string name, string text)
        {
            XmlNode child = CreateChildNode(currentNode, name);
            child.AppendChild(document.CreateTextNode(text));
            return child;
        }
    }
}
