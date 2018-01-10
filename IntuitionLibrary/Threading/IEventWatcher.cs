using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace QBits.Intuition.Threading
{
    /// <summary>
    /// Allows watching for an event to synchronize threads.
    /// </summary>
    public interface IEventWatcher
    {
        /// <summary>
        /// Of course, do what you will with this event. You can actually watch it or not.
        /// </summary>
        /// <param name="evt">Event to be watched</param>
        void WatchThisEvent(EventWaitHandle evt);
    }
}
