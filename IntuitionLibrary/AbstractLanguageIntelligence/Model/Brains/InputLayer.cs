using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.Brains
{
    /// <summary>
    /// Input layer that provides mechanism to give input signals into neurons in this layer.
    /// </summary>
    public class InputLayer : InterfaceLayer
    {
        /// <summary>
        /// Constructs the input layer.
        /// </summary>
        /// <param name="owner">Network that this layer belongs to.</param>
        protected internal InputLayer(Network owner) : base(owner) { }
    }
}
