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
        /// <summary>
        /// Registers the constructor of the specified type of the crossword instance.
        /// </summary>
        /// <param name="type">Class type name.</param>
        /// <param name="constructor">Class constructor of the crossword element.</param>
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
        /// <summary>
        /// Call this to create the crossword element of the specified type within the given crossword.
        /// </summary>
        /// <param name="type">Element type.</param>
        /// <param name="crossword">Parent crossword.</param>
        /// <param name="column">Location of the crossword element.</param>
        /// <param name="row">Location of the crossword element.</param>
        /// <returns></returns>
        public BaseCrosswordElement CreateObject(string type, Crossword crossword, int column, int row)
        {
            return objectCreators[type](crossword, column, row);
        }
        static ElementsFactory _SAP;
        /// <summary>
        /// Collection or constructors of crossword elements.
        /// </summary>
        protected new Dictionary<string, ctor> objectCreators = new Dictionary<string, ctor>();
    }
}
