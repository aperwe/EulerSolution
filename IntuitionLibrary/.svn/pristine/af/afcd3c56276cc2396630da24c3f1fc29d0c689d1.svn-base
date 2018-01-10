using System;

namespace QBits.Intuition.Logger
{
    /// <summary>
    /// Logs output to standard output of console windows.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        #region ILogger Members
        /// <summary>
        /// Writes the text entry to console.
        /// </summary>
        /// <param name="entry">Log entry to process.</param>
        /// <param name="logLevel">Severity of the incoming message.</param>
        public void AcceptLogEntry(LogLevel logLevel, string entry)
        {
            if (ShowSeverity)
            {
                Console.WriteLine("{0}: {1}", logLevel, entry);
            }
            else
            {
                Console.WriteLine(entry);
            }
        }
        #endregion
        /// <summary>
        /// Set to true to prepend the message in the console with severity information.
        /// <para/>If false, only the bare string entry will be output to console.
        /// </summary>
        public bool ShowSeverity { get; set; }
    }
}
