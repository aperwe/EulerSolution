using QBits.Intuition.DesignPatterns.Factory;
using System;
using System.Xml;

namespace QBits.Intuition.Crosswords
{
    /// <summary>
    /// Helper class for managing crossword document files.
    /// </summary>
    public static class CrosswordLoader
    {
        /// <summary>
        /// Helper method for deserializing a crossword saved to an XML file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Crossword LoadCrosswordFromXml(XmlDocument file)
        {
            Crossword loadedObject = null;
            XmlNode root = file.DocumentElement;
            string type = root.Attributes["type"].Value;
            loadedObject = UniversalFactory<string, Crossword>.SAP.CreateObject(type);
            loadedObject.DeserializeFromNode(root);
            return loadedObject;
        }

        public static string persistentCrosswordFileName = "Crossword.xml";
        public static string xmlRootNode = "crossword";
    }
}
