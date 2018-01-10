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
        public delegate classBase ctor();
        public static UniversalFactory<classKey, classBase> SAP
        {
            get
            {
                if (_SAP == null) _SAP = new UniversalFactory<classKey, classBase>();
                return _SAP;
            }
        }
        static UniversalFactory<classKey, classBase> _SAP;
        public classBase CreateObject(classKey type)
        {
            return objectCreators[type]();
        }
        public void RegisterConstructor(classKey type, ctor constructor)
        {
            if (objectCreators.ContainsKey(type)) throw new ExecutionEngineException("Constructor already registered");
            objectCreators.Add(type, constructor);
        }
        protected Dictionary<classKey, ctor> objectCreators = new Dictionary<classKey, ctor>();
    }
}
