using QBits.Intuition.DesignPatterns.Factory;
using QBits.Intuition.Logger;
using System;

namespace QBits.Intuition.Crosswords
{
    class Jolka : Crossword
    {
        #region Implementors should contain this section
        internal static new UniversalFactory<string, Crossword>.ctor ctor
        {
            get
            {
                return new UniversalFactory<string, Crossword>.ctor(createFunc);
            }
        }
        static Crossword createFunc()
        {
            return new Jolka();
        }
        #endregion

        public override void DrawCrossword()
        {
            LoggerSAP.Log("Drawing crossword.");
            if (_parent != null)
            {
                LoggerSAP.Log("Parent control set. Should draw.");
                LoggerSAP.Log("Parent: {0}, dimensions: [{1}, {2}].", _parent, _columns, _rows);
            }
            else
            {
                LoggerSAP.Log("Parent control not set. Nothing to draw.");
            }
        }
        public override string GetObjectType()
        {
            return "Jolka";
        }
    }
}
