using QBits.Intuition.Crosswords.Definitions;
using QBits.Intuition.Logger;
using QBits.Intuition.Xml;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace QBits.Intuition.Crosswords.Elements
{
    /// <summary>
    /// Definition field inside the crossword - cannot contain letters, but is blocking space.
    /// </summary>
    class Definition : BaseCrosswordElement
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
            return new Definition(crossword, column, row);
        }
        #endregion
        protected Definition(Crossword crossword, int column, int row)
            : base(crossword, column, row)
        {
            _def = BaseDefinition.CreateDefinition(Kierunek.Poziomo);
            crossword._defContainer.Add(_def);
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
                me.FlatAppearance.BorderSize = 0;
                me.FlatStyle = FlatStyle.Flat;
                me.BackColor = Color.White;
                me.Visible = true;
                me.Font = new Font(
                    me.Font.FontFamily,
                    me.Font.Size * (float)0.70,
                    me.Font.Style,
                    me.Font.Unit,
                    me.Font.GdiCharSet);
                me.Click += new EventHandler(ElementClickedEH);
                _wControl = me;
                _wControl.Text = _def.definicja; //May come from serialization
            }
        }
        void ElementClickedEH(object sender, EventArgs e)
        {
            ContextMenuStrip popup = new ContextMenuStrip();
            popup.AutoSize = true;
            ToolStripDropDownButton changeTo = new ToolStripDropDownButton("Change to...");

            AdjustDropDownSize(popup, changeTo);
            changeTo.DropDown = new ToolStripDropDown();
            changeTo.DropDown.Items.Add("Empty element", null, new EventHandler(ChangeToEmptyEH));
            changeTo.DropDown.Items.Add("Letter", null, new EventHandler(ChangeToLetterEH));

            popup.Items.Add(changeTo);
            popup.Items.Add("Assign definition", null, new EventHandler(AssignDefinitionEH));
            AddPossibleWords(popup);
            popup.Opacity = 0.5;
            popup.PerformLayout();
            popup.Show(_wControl, new Point(_wControl.Width, _wControl.Height));
            popup.Update();
        }

        private void AddPossibleWords(ContextMenuStrip popup)
        {
            popup.Items.Add(new ToolStripSeparator());
            popup.Items.Add("Add possible word", null, new EventHandler(AddPossibleWordClickedEH));
            if (_def.propozycje.Count > 0)
            {
                popup.Items.Add(new ToolStripSeparator());
                foreach (string w in _def.propozycje)
                {
                    popup.Items.Add(FormatWordForMenu(w));
                }
            }
        }

        private static string FormatWordForMenu(string w)
        {
            string p = string.Format("{0} ({1})", w, w.Length);
            return p;
        }
        void ChangeToEmptyEH(object sender, EventArgs e)
        {
            LoggerSAP.Log("Changing element ({0},{1}) to empty.", _row, _column);
            BaseCrosswordElement bce = ElementsFactory.SAP.CreateObject(EmptyElement.ObjectType, _crossword, _column, _row);
            _crossword.SubstituteCrosswordElement(bce, _column, _row);
        }
        void ChangeToLetterEH(object sender, EventArgs e)
        {
            LoggerSAP.Log("Changing element ({0},{1}) to letter.", _row, _column);
            BaseCrosswordElement bce = ElementsFactory.SAP.CreateObject(Letter.ObjectType, _crossword, _column, _row);
            _crossword.SubstituteCrosswordElement(bce, _column, _row);
        }
        void AssignDefinitionEH(object sender, EventArgs e)
        {
            LoggerSAP.Log("Assigning definition.");
            _tb = new TextBox();
            _tb.Location = _wControl.Location;
            _tb.Parent = _wControl.Parent;
            _tb.Leave += new EventHandler(DefinitionSetEH);
            _tb.Focus();
            _tb.Text = _def.definicja;
            _tb.BringToFront();
        }
        TextBox _tb;
        void DefinitionSetEH(object sender, EventArgs e)
        {
            LoggerSAP.Log("Finished entering the definition.");
            _def.DefinitionChangedEvt += new EventHandler(DefinitionSetEH2);
            _def.definicja = _tb.Text;
            _tb.Dispose();
        }

        /// <summary>
        /// Handles event from the underlying class BaseDefinition cointained within _def.
        /// </summary>
        void DefinitionSetEH2(object sender, EventArgs e)
        {
            BaseDefinition bd = (BaseDefinition)sender;
            _wControl.Text = bd.definicja;
        }
        void AddPossibleWordClickedEH(object sender, EventArgs e)
        {
            LoggerSAP.Log("Adding possible word to ({0},{1}) definition.", _row, _column);
            _tb = new TextBox();
            _tb.Location = _wControl.Location;
            _tb.Parent = _wControl.Parent;
            _tb.Leave += new EventHandler(PossibleWordAddedEH);
            _tb.Focus();
            _tb.BringToFront();
        }
        void PossibleWordAddedEH(object sender, EventArgs e)
        {
            LoggerSAP.Log("Finished entering a possible word.");
            string newWord = _tb.Text;
            _tb.Dispose();
            AddPossibleWord(newWord);
        }

        private void AddPossibleWord(string newWord)
        {
            if (newWord.Length > 0)
            {
                if (_def.propozycje.Contains(newWord.ToLowerInvariant()))
                {
                    LoggerSAP.Log("Word {0} is already contained.", newWord);
                }
                else
                {
                    _def.propozycje.Add(newWord.ToLowerInvariant());
                }
            }
        }
        #region XmlSerializable Members
        public override void SerializeToNode(System.Xml.XmlNode myNode, SimpleXmlDocument nodeOwner)
        {
            base.SerializeToNode(myNode, nodeOwner);
            nodeOwner.AddAttribute("definition", _def.definicja, myNode);
            nodeOwner.AddAttribute("direction", _def.kierunek.ToString(), myNode);
            XmlNode possibilities = nodeOwner.CreateChildNode(myNode, "possibilities");
            foreach (string word in _def.propozycje)
            {
                XmlNode possibility = nodeOwner.CreateChildNode(possibilities, "possibility");
                nodeOwner.AddAttribute("word", word, possibility);
            }
        }
        public override void DeserializeFromNode(System.Xml.XmlNode myNode)
        {
            base.DeserializeFromNode(myNode);
            Definitions.Kierunek k = (Definitions.Kierunek)Enum.Parse(typeof(Definitions.Kierunek), myNode.Attributes["direction"].Value);
            switch (k)
            {
                case Crosswords.Definitions.Kierunek.Nieokreślony: throw new Exception("Invalid file");
                case Crosswords.Definitions.Kierunek.Pionowo: _def = BaseDefinition.CreateDefinition(k); break;
                case Crosswords.Definitions.Kierunek.Poziomo: _def = BaseDefinition.CreateDefinition(k); break;
            }
            _def.definicja = myNode.Attributes["definition"].Value;
            foreach (XmlNode element in myNode.SelectNodes("possibilities/possibility"))
            {
                _def.propozycje.Add(element.Attributes["word"].Value);
            }
        }
        #endregion

        /// <summary>
        /// Static type provided for deserialization.
        /// </summary>
        public new static string ObjectType
        {
            get
            {
                return "Definition";
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
            return _def.definicja;
        }
        Definitions.BaseDefinition _def;
    }
}
