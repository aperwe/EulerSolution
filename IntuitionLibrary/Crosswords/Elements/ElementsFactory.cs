using System;
using System.Collections.Generic;
using QBits.Intuition.DesignPatterns.Factory;

namespace QBits.Intuition.Crosswords.Elements
{
    /// <summary>
    /// A factory class that can produce various crossword element types.
    /// </summary>
    public class ElementsFactory : UniversalFactory<string, BaseCrosswordElement>
    {
        public new delegate BaseCrosswordElement ctor(Crossword crossword, int column, int row);
        public void RegisterConstructor(string type, ctor constructor)
        {
            if (objectCreators.ContainsKey(type)) throw new ExecutionEngineException("Constructor already registered");
            objectCreators.Add(type, constructor);
        }
        public new static ElementsFactory SAP
        {
            get
            {
                if (_SAP == null) _SAP = new ElementsFactory();
                return _SAP;
            }
        }
        public BaseCrosswordElement CreateObject(string type, Crossword crossword, int column, int row)
        {
            return objectCreators[type](crossword, column, row);
        }
        static ElementsFactory _SAP;
        protected new Dictionary<string, ctor> objectCreators = new Dictionary<string, ctor>();
    }
}
