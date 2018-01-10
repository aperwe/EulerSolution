using QBits.Intuition.DesignPatterns.Factory;
using QBits.Intuition.Xml;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QBits.Intuition.Crosswords.Elements
{
    public abstract class BaseCrosswordElement : IFactorableByString, XmlSerializable
    {
        #region Factory model part
        internal static ElementsFactory.ctor ctor
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
        protected BaseCrosswordElement(Crossword crossword, int column, int row)
        {
            _crossword = crossword;
            _column = column;
            _row = row;
        }
        /// <summary>
        /// Overriders decide what to do.
        /// </summary>
        /// <param name="x">Horizontal position (pixels) within the parent control.</param>
        /// <param name="y">Vertical position (pixels) within the parent control.</param>
        /// <param name="parent">The parent control.</param>
        public virtual void Draw(int x, int y, Control parent) { }
        /// <summary>
        /// Parent crossword of this element.
        /// </summary>
        protected Crossword _crossword;
        protected int _column;
        protected int _row;
        /// <summary>
        /// Windows control associated with this element. Null if nothing.
        /// </summary>
        protected Control _wControl;
        protected static void AdjustDropDownSize(ContextMenuStrip graphicsProvider, ToolStripDropDownButton popupDropdown)
        {
            Graphics g = graphicsProvider.CreateGraphics();
            Size textSize = g.MeasureString(popupDropdown.Text, popupDropdown.Font).ToSize();
            popupDropdown.Width = textSize.Width;
            popupDropdown.Height = textSize.Height;
        }
        public void RemoveWindowsControl()
        {
            if (_wControl != null)
            {
                _wControl.Dispose();
                _wControl = null;
            }
        }
        public static string ObjectType
        {
            get
            {
                return null;
            }
        }

        #region XmlSerializable Members

        virtual public void SerializeToNode(System.Xml.XmlNode myNode, SimpleXmlDocument nodeOwner)
        {
            nodeOwner.AddAttribute("column", _column.ToString(), myNode);
            nodeOwner.AddAttribute("row", _row.ToString(), myNode);
            nodeOwner.AddAttribute("type", GetObjectType(), myNode);
        }

        /// <summary>
        /// Default implementation doesn't require anything from Xml.
        /// Override it, if your element type requires to read specific information that was serialized.
        /// </summary>
        /// <param name="myNode">Node from disk file, containing serialized information about this crossword element.</param>
        virtual public void DeserializeFromNode(System.Xml.XmlNode myNode) { }

        #endregion

        #region FactorableByString Members

        virtual public string GetObjectType()
        {
            return null;
        }

        #endregion
    }
}
