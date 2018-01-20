using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Threading
{
    /// <summary>
    /// An object, access to which is safe between threads.
    /// </summary>
    /// <typeparam name="SafeObject">Object wrapped around by this ThreadSafeObject</typeparam>
    public class ThreadSafeObject<SafeObject>
    {
        #region Private members
        object _key = new object();
        SafeObject theObject;
        #endregion
        /// <summary>
        /// Converts implicitly thread-safe object into its contained type object.
        /// </summary>
        /// <param name="r">Thread-safe object to "unbox".</param>
        /// <returns>Unboxed, not thread-safe value contained by r.</returns>
        public static implicit operator SafeObject(ThreadSafeObject<SafeObject> r)
        {
            return r.theObject;
        }
        /// <summary>
        /// Boxes our value into thread-safe object.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static implicit operator ThreadSafeObject<SafeObject>(SafeObject r)
        {
            ThreadSafeObject<SafeObject> n = new ThreadSafeObject<SafeObject>();
            n.theObject = r;
            return n;
        }
        /// <summary>
        /// Key object used to lock access to this <see cref="ThreadSafeObject"/>
        /// </summary>
        public object Key { get { return _key; } }
        public override string ToString()
        {
            return theObject.ToString();
        }
        public override int GetHashCode()
        {
            return theObject.GetHashCode();
        }
    }
}
