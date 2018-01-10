using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.Brains
{
    /// <summary>
    /// A layer interfacing with the external world. It can either be an input or output layer.
    /// </summary>
    public abstract class InterfaceLayer : Layer
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="owner"></param>
        protected internal InterfaceLayer(Network owner) : base(owner) { }
    }
}
