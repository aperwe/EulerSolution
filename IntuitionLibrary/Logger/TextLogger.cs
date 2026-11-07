using QBits.Intuition.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace QBits.Intuition.Logger
{
    /// <summary>
    /// Writes log entries to a text file, one entry per line, on a background thread.
    /// <para/>The logger stops, writes the remaining entries and closes its file when the watched exit event is set,
    /// when it is disposed, or when the process exits.
    /// </summary>
    public class TextLogger : ILogger, IEventWatcher, IDisposable
    {
        /// <summary>
        /// Path of the log file used by the parameterless constructor.
        /// </summary>
        static public string textLogFileName = "CrosswordLogger.txt";
        /// <summary>
        /// Creates a new text logger that writes to <see cref="textLogFileName"/>.
        /// </summary>
        public TextLogger() : this(textLogFileName)
        {
        }
        /// <summary>
        /// Creates a new text logger that writes to the given file. An existing file is overwritten.
        /// </summary>
        /// <param name="logFilePath">Path of the log file.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="logFilePath"/> is null, empty or only white space.</exception>
        public TextLogger(string logFilePath)
        {
            if (string.IsNullOrWhiteSpace(logFilePath)) throw new ArgumentException("A log file path is required.", nameof(logFilePath));
            this.logFilePath = logFilePath;
            logThread = new Thread(LogThread);
            logThread.Priority = ThreadPriority.BelowNormal;
            logThread.Name = "TextLogger";
            logThread.IsBackground = true; //Does not keep the process alive. On process exit, OnProcessExit writes the remaining entries.
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            logThread.Start();
        }

        #region Text logging thread
        Thread logThread;
        void LogThread()
        {
            using StreamWriter sw = new StreamWriter(logFilePath);
            while (!IsExitRequested())
            {
                //Wake up for a new entry or a stop request, or after a short time to check the exit event.
                WaitHandle.WaitAny(new WaitHandle[] { newLogEntryEvent, stopRequested }, ExitCheckIntervalInMiliseconds);
                WriteWaitingEntries(sw);
            }
            LoggerSAP.Log("Text logger exiting: " + logFilePath);
            WriteWaitingEntries(sw);
        }

        void WriteWaitingEntries(StreamWriter sw)
        {
            lock (logEntries.Key)
            {
                while (((Queue<string>)logEntries).Count > 0)
                {
                    sw.WriteLine(((Queue<string>)logEntries).Dequeue());
                }
            }
            sw.Flush();
        }

        bool IsExitRequested()
        {
            if (stopRequested.WaitOne(0)) return true;
            try
            {
                return exitEvent?.WaitOne(0) == true;
            }
            catch (ObjectDisposedException)
            {
                return true; //The owner disposed the exit event, so it will never be set.
            }
        }

        void OnProcessExit(object? sender, EventArgs e) => Dispose();

        readonly string logFilePath;
        const int ExitCheckIntervalInMiliseconds = 100;
        ThreadSafeObject<Queue<string>> logEntries = new Queue<string>();
        AutoResetEvent newLogEntryEvent = new AutoResetEvent(false);
        ManualResetEvent stopRequested = new ManualResetEvent(false);
        EventWaitHandle? exitEvent;
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Stops the logger, writes the remaining entries and closes the log file. Returns when the file is closed.
        /// </summary>
        public void Dispose()
        {
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
            stopRequested.Set();
            logThread.Join();
        }
        #endregion

        #region ILogger Members
        /// <summary>
        /// Take new log entry into the logger queue.
        /// </summary>
        /// <param name="logLevel">Logging level.</param>
        /// <param name="entry">String entry.</param>
        public void AcceptLogEntry(LogLevel logLevel, string entry)
        {
            lock (logEntries.Key)
            {
                ((Queue<string>)logEntries).Enqueue(entry);
            }
            newLogEntryEvent.Set();
        }
        #endregion

        #region IEventWatcher Members

        void IEventWatcher.WatchThisEvent(EventWaitHandle evt)
        {
            exitEvent = evt;
        }

        #endregion
    }

    /// <summary>
    /// Named definitions of useful timespans as measured in miliseconds.
    /// </summary>
    public static class TimeContants
    {
        /// <summary>
        /// Name alias for 10 ms.
        /// </summary>
        public static int InMiliseconds_10Miliseconds  = 10;
        /// <summary>
        /// Name alias for 1 s.
        /// </summary>
        public static int InMiliseconds_OneSecond = 1000;
        /// <summary>
        /// Name alias for 10 s.
        /// </summary>
        public static int InMiliseconds_TenSeconds = 10 * InMiliseconds_OneSecond;
    }
}
