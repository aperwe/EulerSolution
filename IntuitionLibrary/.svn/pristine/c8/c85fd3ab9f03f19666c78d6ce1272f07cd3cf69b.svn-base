using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace QBits.Intuition.Logger
{
    /// <summary>
    /// Accessor class for Logger objects.
    /// Thread-safe as of 1.1.0.0.
    /// </summary>
    public static class LoggerSAP
    {
        /// <summary>
        /// Registers a new logger with the SAP. Note: Calling this method twice with the same logger object, will register two instances of it, causing every log entry to be sent to that logger twice.
        /// </summary>
        /// <param name="newLogger">New logger that will receive log entries</param>
        public static void RegisterLogger(ILogger newLogger)
        {
            lock (((ICollection)loggerList).SyncRoot)
            {
                var loggerRegistration = new LoggerWithLevel(newLogger, LogLevel.Default);
                loggerList.Add(loggerRegistration);
            }
        }
        /// <summary>
        /// Registers a new logger with the SAP. Note: Calling this method twice with the same logger object, will register two instances of it, causing every log entry to be sent to that logger twice.
        /// </summary>
        /// <param name="newLogger">New logger that will receive log entries</param>
        /// <param name="maxSeverity">Maximum severity of log entries this logger wants to listen to.</param>
        public static void RegisterLogger(ILogger newLogger, LogLevel maxSeverity)
        {
            lock (((ICollection)loggerList).SyncRoot)
            {
                var loggerRegistration = new LoggerWithLevel(newLogger, maxSeverity);
                loggerList.Add(loggerRegistration);
            }
        }
        /// <summary>
        /// Unregisters an existing logger so that LoggerSAP stops sending log events to this logger.
        /// </summary>
        /// <param name="registeredLogger">Logger previously registered with LoggerSAP</param>
        /// <exception cref="InvalidOperationException">Thrown when the specified logger is not registered.</exception>
        public static void UnregisterLogger(ILogger registeredLogger)
        {
            lock (((ICollection)loggerList).SyncRoot)
            {
                if (!loggerList.Any(l => l.Logger.Equals(registeredLogger))) throw new InvalidOperationException("The specified logger is not registered. Cannot unregister it.");
                loggerList.Remove(loggerList.First(l => l.Logger.Equals(registeredLogger)));
            }
        }
        /// <summary>
        /// A new log entry to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// <para/>This overload creates a new empty-line log entry. Can be used for log formatting purposes.
        /// </summary>
        public static void Log()
        {
            Log(String.Empty);
        }
        /// <summary>
        /// A new log entry to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">String entry to be logged</param>
        public static void Log(string entry)
        {
            Log(LogLevel.Verbose, entry);
        }
        /// <summary>
        /// A new log entry to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">Format string for the log entry</param>
        /// <param name="args">Parameters that will be used with the <paramref name="entry"/> format string to produce a full log entry</param>
        public static void Log(string entry, params object[] args)
        {
            Log(string.Format(entry, args));
        }
        /// <summary>
        /// New API in V2.3. Allows specifying the level of logging.
        /// </summary>
        /// <param name="logLevel">Severity of this log entry. Default is highest, Verbose (equivalent to Diagnostic) is lowest.</param>
        /// <param name="entry">String entry to be logged</param>
        public static void Log(LogLevel logLevel, string entry)
        {
            string threadEntry = string.Format("[{0}] {1}", System.Threading.Thread.CurrentThread.Name, entry);
            lock (((ICollection)loggerList).SyncRoot)
            {
                loggerList
                    .Where(logListener => logListener.MaxSeverity >= logLevel)
                    .Select(logListener => logListener.Logger)
                    .ToList()
                    .ForEach(iLogger => iLogger.AcceptLogEntry(logLevel, threadEntry));
            }
        }
        /// <summary>
        /// New API in V2.3. Allows specifying the log level of logging.
        /// <para/>A new log entry to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">Format string for the log entry</param>
        /// <param name="args">Parameters that will be used with the <paramref name="entry"/> format string to produce a full log entry</param>
        /// <param name="logLevel">Severity of this log entry. Default is highest, Verbose (equivalent to Diagnostic) is lowest.</param>
        public static void Log(LogLevel logLevel, string entry, params object[] args)
        {
            Log(logLevel, string.Format(entry, args));
        }
        /// <summary>
        /// A new log entry with Diagnostic severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">Format string for the log entry</param>
        /// <param name="args">Parameters that will be used with the <paramref name="entry"/> format string to produce a full log entry</param>
        public static void Trace(string entry, params object[] args)
        {
            Log(LogLevel.Diagnostic, string.Format(entry, args));
        }
        /// <summary>
        /// A new log entry with Diagnostic severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">String entry to be logged</param>
        public static void Trace(string entry)
        {
            Log(LogLevel.Diagnostic, entry);
        }
        /// <summary>
        /// A new log entry with Diagnostic severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        public static void Trace()
        {
            Log(LogLevel.Diagnostic, string.Empty);
        }
        /// <summary>
        /// A new log entry with Warning severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">Format string for the log entry</param>
        /// <param name="args">Parameters that will be used with the <paramref name="entry"/> format string to produce a full log entry</param>
        public static void Warning(string entry, params object[] args)
        {
            Log(LogLevel.Warning, string.Format(entry, args));
        }
        /// <summary>
        /// A new log entry with Warning severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">String entry to be logged</param>
        public static void Warning(string entry)
        {
            Log(LogLevel.Warning, entry);
        }
        /// <summary>
        /// A new log entry with Warning severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        public static void Warning()
        {
            Log(LogLevel.Warning, string.Empty);
        }
        /// <summary>
        /// A new log entry with Error severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">Format string for the log entry</param>
        /// <param name="args">Parameters that will be used with the <paramref name="entry"/> format string to produce a full log entry</param>
        public static void Error(string entry, params object[] args)
        {
            Log(LogLevel.Error, string.Format(entry, args));
        }
        /// <summary>
        /// A new log entry with Error severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">String entry to be logged</param>
        public static void Error(string entry)
        {
            Log(LogLevel.Error, entry);
        }
        /// <summary>
        /// A new log entry with Error severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        public static void Error()
        {
            Log(LogLevel.Error, string.Empty);
        }
        /// <summary>
        /// A new log entry with Critical severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">Format string for the log entry</param>
        /// <param name="args">Parameters that will be used with the <paramref name="entry"/> format string to produce a full log entry</param>
        public static void Critical(string entry, params object[] args)
        {
            Log(LogLevel.Critical, string.Format(entry, args));
        }
        /// <summary>
        /// A new log entry with Critical severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">String entry to be logged</param>
        public static void Critical(string entry)
        {
            Log(LogLevel.Critical, entry);
        }
        /// <summary>
        /// A new log entry with Critical severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        public static void Critical()
        {
            Log(LogLevel.Critical, string.Empty);
        }
        /// <summary>
        /// A new log entry with Verbose severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">Format string for the log entry</param>
        /// <param name="args">Parameters that will be used with the <paramref name="entry"/> format string to produce a full log entry</param>
        public static void Verbose(string entry, params object[] args)
        {
            Log(LogLevel.Verbose, string.Format(entry, args));
        }
        /// <summary>
        /// A new log entry with Verbose severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        /// <param name="entry">String entry to be logged</param>
        public static void Verbose(string entry)
        {
            Log(LogLevel.Verbose, entry);
        }
        /// <summary>
        /// A new log entry with Verbose severity to be distributed to registered loggers. If no loggers are registered, the entry is discarded (nothing happens).
        /// </summary>
        public static void Verbose()
        {
            Log(LogLevel.Verbose, string.Empty);
        }
        /// <summary>
        /// List of registered loggers with their levels.
        /// <para>Only registered loggers receive notifications about new log entries.</para>
        /// </summary>
        static List<LoggerWithLevel> loggerList = new List<LoggerWithLevel>();
    }

    /// <summary>
    /// Meta class holding data about registered ILogger objects that also tracks their maximum severity.
    /// </summary>
    internal class LoggerWithLevel
    {
        /// <summary>
        /// Creates a new LoggerWithLevel by specifying the logger and its associated maximum logging severity.
        /// </summary>
        /// <param name="logger">New logger object</param>
        /// <param name="maxSeverity">Maximum severity which this logger wants to listen to.</param>
        internal LoggerWithLevel(ILogger logger, LogLevel maxSeverity) { Logger = logger; MaxSeverity = maxSeverity; }
        /// <summary>
        /// Logger registered with LoggerSAP
        /// </summary>
        internal ILogger Logger;
        /// <summary>
        /// Maximum level of logging to be sent to this logger.
        /// </summary>
        internal LogLevel MaxSeverity;
    }
}
