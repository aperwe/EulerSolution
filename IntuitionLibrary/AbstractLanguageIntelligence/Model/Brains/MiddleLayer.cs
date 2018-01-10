using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.Brains
{
    /// <summary>
    /// A layer in neural network that is not interfacing with the outside world.
    /// <para/>It can interface with other middle layers or with input layer or with output layer.
    /// </summary>
    public class MiddleLayer : Layer
    {
        /// <summary>
        /// Creates an new middle layer in the specified network.
        /// </summary>
        /// <param name="owner">Network that this layer belongs to.</param>
        protected internal MiddleLayer(Network owner) : base(owner)
        {

        }
    }
}
