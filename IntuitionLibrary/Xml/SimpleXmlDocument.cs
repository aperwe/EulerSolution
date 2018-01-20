using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QBits.Intuition.Xml
{
    /// <summary>Simple representation of a persistence XML file.</summary>
    public class SimpleXmlDocument
    {
        /// <summary>Default constructor.</summary>
        public SimpleXmlDocument(string fileName)
        {
            BasicConstructor(fileName);
        }
        /// <summary>Constructs the document based on the given file and root name.</summary>
        /// <param name="fileName"></param>
        /// <param name="rootNodeName"></param>
        public SimpleXmlDocument(string fileName, string rootNodeName)
        {
            BasicConstructor(fileName);
            CreateRootNode(rootNodeName);
        }
        private void BasicConstructor(string fileName)
        {
            docFileName = fileName;
            doc = new XmlDocument();
            InitDocumentStructure(doc);
        }
        /// <summary>Persists the current XML implementation to the peristent storage.</summary>
        public void Save()
        {
            doc.Save(docFileName);
        }
        /// <summary>
        /// Adds an attribute to the XML document.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <param name="currentNode"></param>
        public void AddAttribute(string attributeName, string attributeValue, XmlNode currentNode)
        {
            XmlAttribute fName = doc.CreateAttribute(attributeName);
            fName.Value = attributeValue;
            currentNode.Attributes.Append(fName);
        }
        /// <summary>Creates a new child node in the document.</summary>
        /// <param name="type">Node type to create.</param>
        /// <param name="name">Node name.</param>
        /// <param name="uri">Node URI.</param>
        /// <returns>Reference to the newly created node.</returns>
        protected XmlNode CreateChildNode(XmlNodeType type, string name, string uri)
        {
            XmlNode newNode = doc.CreateNode(type, name, uri);
            return newNode;
        }
        /// <summary>Creates a child node under specified node.</summary>
        /// <param name="currentNode">Parent node to create the specified node under.</param>
        /// <param name="name">Child node name.</param>
        /// <returns>Child node name.</returns>
        public XmlNode CreateChildNode(XmlNode currentNode, string name)
        {
            return currentNode.AppendChild(CreateChildNode(XmlNodeType.Element, name, null));
        }
        /// <summary>Gets the root node of the XML document.</summary>
        public XmlNode RootNode => docRootNode;
        /// <summary>Gets the XML document.</summary>
        public XmlDocument Document
        {
            get { return doc; }
        }
        private void InitDocumentStructure(XmlDocument xmlFile)
        {
            XmlNode encodingNode = xmlFile.CreateNode(XmlNodeType.XmlDeclaration, null, null);
            xmlFile.AppendChild(encodingNode);
        }

        private void CreateRootNode(string nodeName)
        {
            docRootNode = doc.CreateNode(XmlNodeType.Element, nodeName, null);
            doc.AppendChild(docRootNode);
        }
        string docFileName;
        XmlDocument doc;
        XmlNode docRootNode;
    }
}
