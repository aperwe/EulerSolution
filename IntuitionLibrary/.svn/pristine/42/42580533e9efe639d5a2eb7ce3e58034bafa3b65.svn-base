using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Logger
{
    /// <summary>
    /// Level of log entry severity supported by a given ILogger.
    /// <para/>Determines - from highest to lowest, what a specific ILogger wants to receive.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Highest level of log severity.
        /// <para/>Important message, such as about a processing error of failed operation. Indicates that this log entry is critically important to a person debugging your application.
        /// </summary>
        Critical,
        /// <summary>
        /// Less severe than <see cref="Critical"/>, reports a non-critical processing error in the application.
        /// <para/>Error > Critical.
        /// </summary>
        Error,
        /// <summary>
        /// Less severe than <see cref="Error"/>, warning about some major incident in the application.
        /// <para/>Warning > Error.
        /// </summary>
        Warning,
        /// <summary>
        /// Informative message, lowest priority, provides verbose detail level. Used to trace execution of methods for debugging purposes.
        /// <para/>Diagnostic > Warning.
        /// </summary>
        Diagnostic,
        /// <summary>
        /// Synonym of Diagnostic but more detailed. Provides most verbose level of log output.
        /// <para/>Verbose > Diagnostic.
        /// </summary>
        Verbose,
        /// <summary>
        /// Lowest level of log entry. Provided for compatibility with pre 2.3 versions.
        /// If no log level is specified by a client that creates a log entry, this Default level will be used, so that any logger will accept it (this was the default behavior in 2.2 and earlier versions of Logger.dll).
        /// <para/>Default > Verbose.
        /// </summary>
        Default,
    }
}
