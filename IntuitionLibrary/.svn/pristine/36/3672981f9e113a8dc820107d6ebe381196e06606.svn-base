using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Logger
{
    /// <summary>
    /// If you want to implement a custom logger, that receives log entries from <seealso cref="LoggerSAP"/>, implement this interface in your class.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// A new log entry has just arrived. Do something with it.
        /// </summary>
        /// <param name="entry">Log entry being added.</param>
        /// <param name="logLevel">Severity of the message being recorded.</param>
        void AcceptLogEntry(LogLevel logLevel, string entry);
    }
}
