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
        void DeserializeFromNode(XmlNode myNode);
    }
}
