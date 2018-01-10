using QBits.Intuition.Logger;
using QBits.Intuition.Xml;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QBits.Intuition.Crosswords.Elements
{
    class Letter : BaseCrosswordElement
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
            return new Letter(crossword, column, row);
        }
        #endregion
        protected Letter(Crossword crossword, int column, int row)
            : base(crossword, column, row)
        {
            DefiniteChanged += new EventHandler(DefiniteChangedEH);
        }
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
                if (fDefinite)
                {
                    me.Font = new Font(me.Font, FontStyle.Bold);
                }
                else
                {
                    me.Font = new Font(me.Font, FontStyle.Regular);
                }
                me.Visible = true;
                me.Click += new EventHandler(ElementClickedEH);
                _wControl = me;
                _wControl.Text = letter; //May come from serialization
            }
        }
        string _letter = "";
        public string letter
        {
            get { return _letter; }
            set { _letter = value; if (_wControl != null) { _wControl.Text = _letter; } }
        }

        private bool _definite = false;
        public bool fDefinite
        {
            get { return _definite; }
            set
            {
                _definite = value;
                DefiniteChanged(this, null);
            }
        }

        void DefiniteChangedEH(object sender, EventArgs e)
        {
            if (_wControl != null) //React to fDefinite change only if we have GUI control
            {
                if (fDefinite)
                {
                    _wControl.Font = new Font(_wControl.Font, FontStyle.Bold);
                }
                else
                {
                    _wControl.Font = new Font(_wControl.Font, FontStyle.Regular);
                }
            }
        }
        event EventHandler DefiniteChanged;
        void ElementClickedEH(object sender, EventArgs e)
        {
            ContextMenuStrip popup = new ContextMenuStrip();
            popup.AutoSize = true;
            ToolStripDropDownButton changeTo = new ToolStripDropDownButton("Change to...");

            AdjustDropDownSize(popup, changeTo);
            changeTo.DropDown = new ToolStripDropDown();
            changeTo.DropDown.Items.Add("Empty", null, new EventHandler(ChangeToEmptyEH));
            changeTo.DropDown.Items.Add("Definition", null, new EventHandler(ChangeToDefinitionEH));

            popup.Items.Add(changeTo);
            popup.Items.Add("Assign letter", null, new EventHandler(AssignLetterEH));
            if (letter.Length > 0) //If a letter has been assigned, show this menu.
            {
                ToolStripButton definiteButton = new ToolStripButton("Definite", null, new EventHandler(DefiniteClickedEH));
                definiteButton.Checked = fDefinite;
                popup.Items.Add(definiteButton);
            }
            popup.Opacity = 0.5;
            popup.PerformLayout();
            popup.Show(_wControl, new Point(_wControl.Width, _wControl.Height));
            popup.Update();
        }

        void DefiniteClickedEH(object sender, EventArgs e)
        {
            fDefinite = !fDefinite;
            LoggerSAP.Log("Letter {0} is now {1}.", letter, fDefinite ? "Definite" : "Not definite");
        }
        void ChangeToEmptyEH(object sender, EventArgs e)
        {
            LoggerSAP.Log("Changing element ({0},{1}) to empty.", _row, _column);
            BaseCrosswordElement bce = ElementsFactory.SAP.CreateObject(EmptyElement.ObjectType, _crossword, _column, _row);
            _crossword.SubstituteCrosswordElement(bce, _column, _row);
        }
        void AssignLetterEH(object sender, EventArgs e)
        {
            LoggerSAP.Log("Assigning letter.");
            _tb = new TextBox();
            _tb.Parent = _wControl;
            _tb.Leave += new EventHandler(LetterSetEH);
            _tb.MaxLength = 1;
            _tb.Focus();
            _tb.Text = letter;
        }

        void LetterSetEH(object sender, EventArgs e)
        {
            LoggerSAP.Log("Finished entering the letter.");
            letter = _tb.Text.ToUpperInvariant();
            _tb.Dispose();
        }
        TextBox _tb;

        void ChangeToDefinitionEH(object sender, EventArgs e)
        {
            LoggerSAP.Log("Changing element ({0},{1}) to definition.", _row, _column);
            BaseCrosswordElement bce = ElementsFactory.SAP.CreateObject(Definition.ObjectType, _crossword, _column, _row);
            _crossword.SubstituteCrosswordElement(bce, _column, _row);
        }

        #region XmlSerializable Members
        public override void SerializeToNode(System.Xml.XmlNode myNode, SimpleXmlDocument nodeOwner)
        {
            base.SerializeToNode(myNode, nodeOwner);
            nodeOwner.AddAttribute("letter", letter, myNode);
            nodeOwner.AddAttribute("definite", fDefinite.ToString(), myNode);
        }
        public override void DeserializeFromNode(System.Xml.XmlNode myNode)
        {
            base.DeserializeFromNode(myNode);
            letter = myNode.Attributes["letter"].Value;
            fDefinite = bool.Parse(myNode.Attributes["definite"].Value);
        }
        #endregion

        /// <summary>
        /// Static type provided for deserialization.
        /// </summary>
        public new static string ObjectType
        {
            get
            {
                return "Letter";
            }
        }
        /// <summary>
        /// Dynamic type provided for serialization.
        /// </summary>
        public override string GetObjectType()
        {
            return ObjectType;
        }
        public override string ToString()
        {
            return letter;
        }
    }
}
