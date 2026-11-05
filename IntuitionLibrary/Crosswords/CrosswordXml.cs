using System.Xml;

namespace QBits.Intuition.Crosswords
{
    /// <summary>
    /// Helpers for reading crossword data saved to XML.
    /// </summary>
    internal static class CrosswordXml
    {
        /// <summary>
        /// Gets the value of an attribute that crossword data must contain.
        /// </summary>
        /// <param name="node">Node holding the attribute.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>Value of the attribute.</returns>
        /// <exception cref="XmlException">The attribute is missing, so the crossword data is malformed.</exception>
        internal static string RequiredAttribute(XmlNode node, string attributeName)
        {
            XmlAttribute? attribute = node.Attributes?[attributeName];
            if (attribute == null)
            {
                throw new XmlException(string.Format("Crossword data is missing attribute '{0}' on node '{1}'.", attributeName, node.Name));
            }
            return attribute.Value;
        }
    }
}
