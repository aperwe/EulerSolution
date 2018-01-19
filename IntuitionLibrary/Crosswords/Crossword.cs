using QBits.Intuition.Crosswords.Elements;
using QBits.Intuition.DesignPatterns.Factory;
using QBits.Intuition.Logger;
using QBits.Intuition.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

namespace QBits.Intuition.Crosswords
{
    /// <summary>
    /// Base abstract class for creating custom crossword types.
    /// </summary>
    public abstract class Crossword : IFactorableByString, IContainer, XmlSerializable
    {
        #region Factory model part
        /// <summary>
        /// For factory model, we require default constructor.
        /// </summary>
        protected Crossword()
        {
        }
        internal static UniversalFactory<string, Crossword>.ctor ctor
        {
            get
            {
                return null;
            }
        }
        #region Implementors should contain this section
        //internal static new CheckFactory.ctor ctor
        //{
        //    get
        //    {
        //        return new CheckFactory.ctor(createFunc);
        //    }
        //}
        //static IChecker createFunc()
        //{
        //    return new <mycheckerName>();
        //}
        #endregion
        #endregion
        /// <summary>
        /// Numer of columns.
        /// </summary>
        protected int _columns;
        /// <summary>
        /// Number of rows.
        /// </summary>
        protected int _rows;
        /// <summary>
        /// The parent control, where we can draw.
        /// </summary>
        protected Control _parent = null;
        /// <summary>
        /// Sets dimensions for the crossword.
        /// </summary>
        /// <param name="columns">Number of crossword columns.</param>
        /// <param name="rows">Numer of crossword rows.</param>
        public void SetDimensions(int columns, int rows)
        {
            _columns = columns;
            _rows = rows;
            if (_letters != null)
            {
                LoggerSAP.Log("Array of letter not empty. Deleting it.");
            }
            LoggerSAP.Log("Allocating new array of base crossword elements.");
            _letters = new BaseCrosswordElement[_columns, _rows];
        }
        /// <summary>
        /// Prepares a default crossword - after number of rows and columns has been specified.
        /// The default crossword consists of letters only (unassigned, of course).
        /// </summary>
        public void MakeEmptyCrossword()
        {
            for (int column = 0; column < _columns; column++)
            {
                for (int row = 0; row < _rows; row++)
                {
                    _letters[column, row] = ElementsFactory.SAP.CreateObject(Letter.ObjectType, this, column, row);
                }
            }
            CreateLetters();
        }
        /// <summary>
        /// Creates visual controls for the letters in the crossword, thus making the constructed crossword visible to the user.
        /// The controls are made children of Parent control (usually a group box).
        /// </summary>
        public void CreateLetters()
        {
            for (int column = 0; column < _columns; column++)
            {
                for (int row = 0; row < _rows; row++)
                {
                    _letters[column, row].Draw(_parent.DisplayRectangle.Left + column * letterWidth, _parent.DisplayRectangle.Top + row * letterHeight, _parent);
                }
            }
        }
        /// <summary>
        /// Replaces and redraws the element at the specified location.
        /// </summary>
        /// <param name="bce">Replacing element (new).</param>
        /// <param name="column">Location column.</param>
        /// <param name="row">Location row.</param>
        public void SubstituteCrosswordElement(BaseCrosswordElement bce, int column, int row)
        {
            if (_letters[column, row] != null)
            {
                _letters[column, row].RemoveWindowsControl();
            }
            _letters[column, row] = bce;
            if (_parent != null)
            {
                bce.Draw(_parent.DisplayRectangle.Left + column * letterWidth, _parent.DisplayRectangle.Top + row * letterHeight, _parent);
            }
        }
        /// <summary>
        /// Removes all GUI controls representing the crossword.
        /// </summary>
        public void RemoveWindowsControls()
        {
            for (int column = 0; column < _columns; column++)
            {
                for (int row = 0; row < _rows; row++)
                {
                    _letters[column, row].RemoveWindowsControl();
                }
            }
        }
        /// <summary>
        /// Array container for crossword elements.
        /// </summary>
        protected BaseCrosswordElement[,] _letters;
        /// <summary>
        /// GUI control that is a parent for this crossword.
        /// </summary>
        public Control Parent
        {
            set { _parent = value; }
        }
        /// <summary>
        /// Since the elements of the crossword we are drawing are windows controls,
        /// we don't have to worry about them, so the derived implementation doesn't have to do anything to have them drawn.
        /// </summary>
        public abstract void DrawCrossword();
        public int letterWidth
        {
            get { return 55; }
        }
        public int letterHeight
        {
            get { return 44; }
        }

        #region IContainer Members

        List<IComponent> _components = new List<IComponent>();
        void IContainer.Add(IComponent component, string name)
        {
            throw new Exception("Not supported.");
        }

        void IContainer.Add(IComponent component)
        {
            if (_components.Contains(component))
            {
                throw new Exception("This component is already contained.");
            }
            _components.Add(component);
        }

        ComponentCollection IContainer.Components
        {
            get
            {
                return new ComponentCollection(_components.ToArray());
            }
        }

        void IContainer.Remove(IComponent component)
        {
            if (_components.Contains(component))
            {
                _components.Remove(component);
            }
            else
            {
                throw new Exception("This component is not in our list of components.");
            }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            foreach (IComponent c in _components)
            {
                c.Dispose();
            }
        }

        #endregion

        #region XmlSerializable Members

        virtual public void SerializeToNode(System.Xml.XmlNode myNode, SimpleXmlDocument nodeOwner)
        {
            nodeOwner.AddAttribute("columns", _columns.ToString(), myNode);
            nodeOwner.AddAttribute("rows", _rows.ToString(), myNode);
            nodeOwner.AddAttribute("type", GetObjectType(), myNode);
            XmlNode elements = nodeOwner.CreateChildNode(myNode, "elements");
            for (int column = 0; column < _columns; column++)
            {
                for (int row = 0; row < _rows; row++)
                {
                    XmlNode subElement = nodeOwner.CreateChildNode(elements, "crosswordItem");
                    _letters[column, row].SerializeToNode(subElement, nodeOwner);
                }
            }
        }

        /// <summary>
        /// Default implementation reads crossword dimensions (type is already known by CrosswordLoader, who used factory method
        /// to create appropriate crossword type). Then it deserializes all elements, by calling their appropriate deserializers
        /// (using factory method from ElementsFactory).
        /// Override it, if your derived crossword type requires to read specific information that was serialized.
        /// </summary>
        /// <param name="myNode">Node from disk file, containing serialized information about this crossword.</param>
        virtual public void DeserializeFromNode(System.Xml.XmlNode myNode)
        {
            int columns = int.Parse(myNode.Attributes["columns"].Value);
            int rows = int.Parse(myNode.Attributes["rows"].Value);
            SetDimensions(columns, rows);
            foreach (XmlNode element in myNode.SelectNodes("elements/crosswordItem"))
            {
                BaseCrosswordElement bce = RecreateElementFromXmlNode(element, this);
            }
        }
        private static BaseCrosswordElement RecreateElementFromXmlNode(XmlNode element, Crossword loadedObject)
        {
            string type = element.Attributes["type"].Value;
            int column = int.Parse(element.Attributes["column"].Value);
            int row = int.Parse(element.Attributes["row"].Value);
            BaseCrosswordElement bce = ElementsFactory.SAP.CreateObject(type, loadedObject, column, row);
            bce.DeserializeFromNode(element);
            loadedObject.SubstituteCrosswordElement(bce, column, row);
            return bce;
        }
        #endregion

        #region FactorableByString Members

        virtual public string GetObjectType()
        {
            return null;
        }

        #endregion

        /// <summary>
        /// Contains definitions of this crossword.
        /// </summary>
        internal Container _defContainer = new Container();
    }
}
