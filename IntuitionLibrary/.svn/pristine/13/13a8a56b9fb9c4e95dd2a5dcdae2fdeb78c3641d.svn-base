using QBits.Intuition.Crosswords.Elements;
using QBits.Intuition.DesignPatterns.Factory;
using QBits.Intuition.Logger;
using System;

namespace QBits.Intuition.Crosswords
{
    /// <summary>
    /// Registrar of crossword types.
    /// </summary>
    public class CrosswordTypeRegistrar
    {
        public static void RegisterKnownCrosswordTypes()
        {
            LoggerSAP.Log("Registering known crossword types.");
            UniversalFactory<string, Crossword>.SAP.RegisterConstructor("Jolka", Jolka.ctor);
        }
        public static void RegisterKnownElementTypes()
        {
            LoggerSAP.Log("Registering known crossword element types.");
            ElementsFactory.SAP.RegisterConstructor(Letter.ObjectType, Letter.ctor);
            ElementsFactory.SAP.RegisterConstructor(EmptyElement.ObjectType, EmptyElement.ctor);
            ElementsFactory.SAP.RegisterConstructor(Definition.ObjectType, Definition.ctor);
        }
    }
}
