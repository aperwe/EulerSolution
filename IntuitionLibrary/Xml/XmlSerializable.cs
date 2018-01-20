using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QBits.Intuition.Xml
{
    /// <summary>
    /// Definition of an object that is able to be serialized/deserialized to XML persistent storage.
    /// </summary>
    public interface XmlSerializable
    {
        /// <summary>Serializes this object to the specified XML node.</summary>
        /// <param name="myNode">Node to which to serialize.</param>
        /// <param name="nodeOwner">Parent document owning the node.</param>
        void SerializeToNode(XmlNode myNode, SimpleXmlDocument nodeOwner);
        /// <summary>Reconstructs the specified node from persistent storage.</summary>
        /// <param name="myNode">Node to deserialize.</param>
        void DeserializeFromNode(XmlNode myNode);
    }
}
