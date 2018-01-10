using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QBits.Intuition.Xml
{
    class SpecializedConfigXml : SimpleXmlDocument
    {
        public SpecializedConfigXml(string rootNodeName)
            : base("", rootNodeName)
        {
            Params = new _Params(this);
            StringParams = new _StringParams(this);
        }

        public XmlNode CreateParam(string name, string value)
        {
            XmlNode configParam = CreateChildNode(rootNode, "param");
            AddAttribute("name", name, configParam);
            AddAttribute("value", value, configParam);
            return configParam;
        }
        public _Params Params;
        public _StringParams StringParams;

        internal class _Params
        {
            public _Params(SpecializedConfigXml myParent)
            {
                Params = myParent;
            }
            SpecializedConfigXml Params;
            /// <summary>
            /// Returns parameter node of specified name.
            /// </summary>
            /// <param name="paramName">Name of config node to look up in the document.</param>
            /// <returns>Looked up XmlNode of the specified name or null.</returns>
            public XmlNode this[string paramName]
            {
                get
                {
                    string xpath = string.Format("/config/param[@name='{0}']", paramName);
                    return Params.document.SelectSingleNode(xpath);
                }
            }
        }
        internal class _StringParams
        {
            public _StringParams(SpecializedConfigXml myParent)
            {
                Params = myParent;
            }
            SpecializedConfigXml Params;
            /// <summary>
            /// Returns string value of "value" attribute of the parameter with the specified name.
            /// </summary>
            /// <remarks>This is a safe accessor. If the node with specified value does not exist, no exception is thrown, but a value of null is returned.</remarks>
            /// <param name="paramName">Name of config node to look up in the document.</param>
            /// <returns>Value of 'value' attribute with the specified name or null.</returns>
            public string this[string paramName]
            {
                get
                {
                    string retVal = null;
                    string xpath = string.Format("/config/param[@name='{0}']", paramName);
                    try
                    {
                        retVal = Params.document.SelectSingleNode(xpath).Attributes["value"].Value;
                    }
                    catch { }
                    return retVal;
                }
            }
        }
    }
}
