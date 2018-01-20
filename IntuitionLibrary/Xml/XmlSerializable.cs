using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QBits.Intuition.Xml
{
    public interface XmlSerializable
    {
        void SerializeToNode(XmlNode myNode, SimpleXmlDocument nodeOwner);
        /// <summary>Reconstructs the specified node from persistent storage.</summary>
        /// <param name="myNode">Node to deserialize.</param>
        void DeserializeFromNode(XmlNode myNode);
    }
}
