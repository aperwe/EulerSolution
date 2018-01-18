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
        /// <summary>
        /// Registers known crossword implementors with the factory.
        /// </summary>
        public static void RegisterKnownCrosswordTypes()
        {
            LoggerSAP.Log("Registering known crossword types.");
            UniversalFactory<string, Crossword>.SAP.RegisterConstructor("Jolka", Jolka.ctor);
        }
        /// <summary>
        /// Registers known crossword element types with <see cref="ElementsFactory"/>.
        /// </summary>
        public static void RegisterKnownElementTypes()
        {
            LoggerSAP.Log("Registering known crossword element types.");
            ElementsFactory.SAP.RegisterConstructor(Letter.ObjectType, Letter.ctor);
            ElementsFactory.SAP.RegisterConstructor(EmptyElement.ObjectType, EmptyElement.ctor);
            ElementsFactory.SAP.RegisterConstructor(Definition.ObjectType, Definition.ctor);
        }
    }
}
