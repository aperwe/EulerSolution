using QBits.Intuition.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace QBits.Intuition.Logger
{
    /// <summary>
    /// Custom logging facility for Crossword. Do not use in other projects.
    /// <para/>The logger stops, writes the remaining entries and closes its file when the watched exit event is set,
    /// when it is disposed, or when the process exits.
    /// </summary>
    public class TextLogger : ILogger, IEventWatcher, IDisposable
    {
        /// <summary>
        /// Name of the log file for this custom logging facility. Hardcoded.
        /// </summary>
        static public string textLogFileName = "CrosswordLogger.txt";
        /// <summary>
        /// Creates a new instance of custom text logger for Crossword
        /// </summary>
        public TextLogger()
        {
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
            using StreamWriter sw = new StreamWriter(textLogFileName);
            while (!IsExitRequested())
            {
                //Wake up for a new entry or a stop request, or after a short time to check the exit event.
                WaitHandle.WaitAny(new WaitHandle[] { newLogEntryEvent, stopRequested }, ExitCheckIntervalInMiliseconds);
                WriteWaitingEntries(sw);
            }
            LoggerSAP.Log("Crossword text logger exitting.");
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
