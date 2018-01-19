using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.DesignPatterns.Factory
{
    /// <summary>
    /// This class is not thread-safe. If you want to use this class in multithreaded environment,
    /// you have to derive your own thread-safe implementation of:
    /// SAP, RegisterConstructor(), CreateObject()
    /// </summary>
    /// <typeparam name="classKey"></typeparam>
    /// <typeparam name="classBase"></typeparam>
    public class UniversalFactory<classKey, classBase>
    {
        /// <summary>
        /// Signature of a method that can create instances of a specified object.
        /// </summary>
        /// <returns></returns>
        public delegate classBase ctor();
        /// <summary>
        /// Singleton instance of <see cref="UniversalFactory{classKey, classBase}"/>
        /// </summary>
        public static UniversalFactory<classKey, classBase> SAP
        {
            get
            {
                if (_SAP == null) _SAP = new UniversalFactory<classKey, classBase>();
                return _SAP;
            }
        }
        static UniversalFactory<classKey, classBase> _SAP;
        /// <summary>
        /// Creates an instance of the specified class type.
        /// </summary>
        /// <param name="type">Type name to create instance of.</param>
        /// <returns>Instance of the specified type.</returns>
        public classBase CreateObject(classKey type)
        {
            return objectCreators[type]();
        }
        /// <summary>
        /// Registers a constructor of the given class type.
        /// </summary>
        /// <param name="type">Type name this constructor creates.</param>
        /// <param name="constructor">Constructor method that can create a type instance.</param>
        public void RegisterConstructor(classKey type, ctor constructor)
        {
            if (objectCreators.ContainsKey(type)) throw new ExecutionEngineException("Constructor already registered");
            objectCreators.Add(type, constructor);
        }
        protected Dictionary<classKey, ctor> objectCreators = new Dictionary<classKey, ctor>();
    }
}
