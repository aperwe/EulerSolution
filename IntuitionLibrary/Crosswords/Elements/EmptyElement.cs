using QBits.Intuition.Logger;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QBits.Intuition.Crosswords.Elements
{
    class EmptyElement : BaseCrosswordElement
    {
        #region Factory model part
        internal static new ElementsFactory.ctor ctor
        {
            get
            {
                return new ElementsFactory.ctor(createFunc);
            }
        }
        static BaseCrosswordElement createFunc(Crossword crossword, int column, int row)
        {
            return new EmptyElement(crossword, column, row);
        }
        #endregion
        protected EmptyElement(Crossword crossword, int column, int row) : base(crossword, column, row) { }
        public override void Draw(int x, int y, Control parent)
        {
            base.Draw(x, y, parent);
            if (_wControl == null)
            {
                Button me = new Button();
                me.Parent = parent;
                me.Width = _crossword.letterWidth;
                me.Height = _crossword.letterHeight;
                me.Left = x;
                me.Top = y;
                me.FlatStyle = FlatStyle.Flat;
                me.FlatAppearance.BorderSize = 0;
                me.BackColor = Color.Green;
                me.Visible = true;
                me.Click += new EventHandler(ElementClickedEH);
                _wControl = me;
            }
        }

        void ElementClickedEH(object sender, EventArgs e)
        {
            ContextMenuStrip popup = new ContextMenuStrip();
            popup.AutoSize = true;
            ToolStripDropDownButton changeTo = new ToolStripDropDownButton("Change to...");

            AdjustDropDownSize(popup, changeTo);
            changeTo.DropDown = new ToolStripDropDown();
            changeTo.DropDown.Items.Add("Letter", null, new EventHandler(ChangeToLetterEH));
            changeTo.DropDown.Items.Add("Definition", null, new EventHandler(ChangeToDefinitionEH));

            popup.Items.Add(changeTo);
            popup.Opacity = 0.5;
            popup.PerformLayout();
            popup.Show(_wControl, new Point(_wControl.Width, _wControl.Height));
            popup.Update();
        }

        void ChangeToLetterEH(object sender, EventArgs e)
        {
            LoggerSAP.Log("Changing element ({0},{1}) to letter.", _row, _column);
            BaseCrosswordElement bce = ElementsFactory.SAP.CreateObject(Letter.ObjectType, _crossword, _column, _row);
            _crossword.SubstituteCrosswordElement(bce, _column, _row);
        }
        void ChangeToDefinitionEH(object sender, EventArgs e)
        {
            LoggerSAP.Log("Changing element ({0},{1}) to definition.", _row, _column);
            BaseCrosswordElement bce = ElementsFactory.SAP.CreateObject(Definition.ObjectType, _crossword, _column, _row);
            _crossword.SubstituteCrosswordElement(bce, _column, _row);
        }

        /// <summary>
        /// Static type provided for deserialization.
        /// </summary>
        public new static string ObjectType
        {
            get
            {
                return "EmptyElement";
            }
        }
        /// <summary>
        /// Dynamic type provided for serialization.
        /// </summary>
        public override string GetObjectType()
        {
            return ObjectType;
        }
    }
}
