using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QBits.Intuition.Xml
{
    public class SimpleXmlDocument
    {
        public SimpleXmlDocument(string fileName)
        {
            BasicConstructor(fileName);
        }
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
        public void Save()
        {
            doc.Save(docFileName);
        }
        public void AddAttribute(string attributeName, string attributeValue, XmlNode currentNode)
        {
            XmlAttribute fName = doc.CreateAttribute(attributeName);
            fName.Value = attributeValue;
            currentNode.Attributes.Append(fName);
        }
        protected XmlNode CreateChildNode(XmlNodeType type, string name, string uri)
        {
            XmlNode newNode = doc.CreateNode(type, name, uri);
            return newNode;
        }
        public XmlNode CreateChildNode(XmlNode currentNode, string name)
        {
            return currentNode.AppendChild(CreateChildNode(XmlNodeType.Element, name, null));
        }
        public XmlNode rootNode
        {
            get { return docRootNode; }
        }
        public XmlDocument document
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
