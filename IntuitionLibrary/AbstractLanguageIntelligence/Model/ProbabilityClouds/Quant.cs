using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.ProbabilityClouds
{
    /// <summary>
    /// Basic part of a probability cloud - quant.
    /// Characterized by probability measure - Weight.
    /// The quantitized object that can exist - with probability equal to Weight - is Particle.
    /// </summary>
    public class Quant
    {
        /// <summary>
        /// Weight associated with this quant
        /// </summary>
        public double Weight;
        /// <summary>
        /// Quant object - could be a wave function, representation of a physical particle, a macroscopic object, etc.
        /// </summary>
        public object Particle;
    }
}
