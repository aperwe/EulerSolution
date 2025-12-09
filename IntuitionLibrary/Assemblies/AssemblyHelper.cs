using System;
using System.Reflection;

namespace QBits.Intuition.Assemblies
{
    /// <summary>
    /// Offers convenience methods for typical manipulation involving various Assembly-related data.
    /// </summary>
    public class AssemblyHelper
    {
        /// <summary>
        /// Returns assembly version attribute formatted as a string.
        /// </summary>
        public static string AssemblyVersion(WhichAssembly whichAssembly)
        {
            Assembly assembly = null;
            switch (whichAssembly)
            {
                case WhichAssembly.EntryAssembly: assembly = Assembly.GetEntryAssembly(); break;
                case WhichAssembly.ThisAssembly: assembly = Assembly.GetCallingAssembly(); break;
            }

            // Fix: Check for null before dereferencing
            return assembly?.GetName().Version?.ToString(4) ?? string.Empty;
        }
    }

    /// <summary>
    /// Indicates assembly to perform operations on.
    /// </summary>
    public enum WhichAssembly
    {
        /// <summary>
        /// Assembly which was used to start the current process.
        /// </summary>
        EntryAssembly,
        /// <summary>
        /// Assembly which contains the method making a specific call into Intuition library.
        /// </summary>
        ThisAssembly
    }
}
