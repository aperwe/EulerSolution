using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.DesignPatterns.Factory
{
    /// <summary>
    /// Interface type for types that can be instantiated by a factory.
    /// </summary>
    public interface IFactorableByString
    {
        /// <summary>
        /// An object type that is known to its factory.
        /// I.e. factory can recreate the required object type based on this value.
        /// This should match what you place in factory registrar.
        /// Used for deserialization.
        /// </summary>
        string GetObjectType();
    }
}
