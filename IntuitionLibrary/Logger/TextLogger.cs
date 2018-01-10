using QBits.Intuition.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace QBits.Intuition.Logger
{
    /// <summary>
    /// Custom logging facility for Crossword. Do not user in other projects.
    /// </summary>
    public class TextLogger : ILogger, IEventWatcher
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
            logThread.Start();
        }

        #region Text logging thread
        Thread logThread;
        void LogThread()
        {
            StreamWriter sw = new StreamWriter(textLogFileName);
            Thread.Sleep(TimeContants.InMiliseconds_OneSecond); //Wait 1 s
            //if there is an exitEvent set, use it, otherwise continue to 
            if (null != exitEvent)
            {
                while (!exitEvent.WaitOne(TimeContants.InMiliseconds_TenSeconds, false)) //Every 10 s do a forcible flush, unless a new item comes in the meantime.
                {
                    lock (logEntries.Key) //Lock the threaded access.
                    {
                        if (((Queue<string>)logEntries).Count > 0) //Do something only when there is anything to flush to file.
                        {
                            sw.WriteLine(((Queue<string>)logEntries).Dequeue());
                            while (((Queue<string>)logEntries).Count > 30) sw.WriteLine(((Queue<string>)logEntries).Dequeue());
                        }
                        else
                        {
                            sw.Flush();
                        }
                    }
                }
                //App exit has been signalled.
                LoggerSAP.Log("Crossword text logger exitting.");
                lock (logEntries.Key)
                {
                    while (((Queue<string>)logEntries).Count > 0) //Flush everything.
                    {
                        sw.WriteLine(((Queue<string>)logEntries).Dequeue());
                    }
                }
                sw.Flush();
                sw.Close();
            }
            else //No exit event. Just write off
            {
                while (true)
                {
                    lock (logEntries.Key)
                    {
                        while (((Queue<string>)logEntries).Count > 0) //Flush everything.
                        {
                            sw.WriteLine(((Queue<string>)logEntries).Dequeue());
                        }
                    }
                    sw.Flush();
                    Thread.Sleep(TimeContants.InMiliseconds_10Miliseconds);
                }
                sw.Close();
            }
        }

        ThreadSafeObject<Queue<string>> logEntries = new Queue<string>();
        AutoResetEvent newLogEntryEvent = new AutoResetEvent(false);
        EventWaitHandle exitEvent;
        #endregion

        #region ILogger Members

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

    public static class TimeContants
    {
        public static int InMiliseconds_10Miliseconds  = 10;
        public static int InMiliseconds_OneSecond = 1000;

        public static int InMiliseconds_TenSeconds = 10 * InMiliseconds_OneSecond;
    }
}
