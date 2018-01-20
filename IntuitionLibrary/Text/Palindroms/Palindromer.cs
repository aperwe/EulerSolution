using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace QBits.Intuition.Text.Palindroms
{
    /// <summary>
    /// Main class that provides access to palindrom features.
    /// <para/>This includes building a glossary, finding a palindrom out of input string, and more.
    /// </summary>
    public class Palindromer
    {
        private static XElement sampleDictionary;
        /// <summary>
        /// Reference to a sample dictionary that contains a few words and can process a few sample palindroms.
        /// </summary>
        public static XElement SampleDictonary
        {
            get
            {
                if (sampleDictionary == null)
                {
                    sampleDictionary = CreateSampleDictionary();
                }
                return sampleDictionary;
            }
        }
        /// <summary>
        /// Initializes a new instance of the sample dictionary.
        /// </summary>
        private static XElement CreateSampleDictionary()
        {
            var newDict = new XElement("dict");
            AddWord(newDict, "a");
            AddWord(newDict, "artur");
            AddWord(newDict, "artura");
            AddWord(newDict, "od");
            AddWord(newDict, "do");
            AddWord(newDict, "dla");
            AddWord(newDict, "dal");
            AddWord(newDict, "dali");
            AddWord(newDict, "i");
            AddWord(newDict, "da");
            AddWord(newDict, "lad");
            AddWord(newDict, "lada");

            #region Palindromy z Angory - do testów
            #region Ale fest - estragon. Jada mało Izydor. Świeże jada jeże i w środy zioła ma. Daj no gar tse-tse, Fela.
            AddWord(newDict, "ale");
            AddWord(newDict, "fest");
            AddWord(newDict, "estragon");
            AddWord(newDict, "jada");
            AddWord(newDict, "mało");
            AddWord(newDict, "izydor");
            AddWord(newDict, "świeże");
            AddWord(newDict, "jada");
            AddWord(newDict, "jeże");
            AddWord(newDict, "w");
            AddWord(newDict, "środy");
            AddWord(newDict, "zioła");
            AddWord(newDict, "ma");
            AddWord(newDict, "daj");
            AddWord(newDict, "no");
            AddWord(newDict, "gar");
            AddWord(newDict, "tse");
            AddWord(newDict, "fela");
            #endregion

            #region I bul, to kumy wór apetytu. Jadamy do woli. Kot ten netto kilo wody ma. Daj utyte parówy mu - kot lubi.
            AddWord(newDict, "bul");
            AddWord(newDict, "to");
            AddWord(newDict, "kumy");
            AddWord(newDict, "wór");
            AddWord(newDict, "apetytu");
            AddWord(newDict, "jadamy");
            AddWord(newDict, "do");
            AddWord(newDict, "woli");
            AddWord(newDict, "kot");
            AddWord(newDict, "ten");
            AddWord(newDict, "netto");
            AddWord(newDict, "kilo");
            AddWord(newDict, "wody");
            AddWord(newDict, "ma");
            AddWord(newDict, "daj");
            AddWord(newDict, "utyte");
            AddWord(newDict, "parówy");
            AddWord(newDict, "mu");
            AddWord(newDict, "lubi");
            #endregion

            #region A i car był zły, bo mrowisko. Boks i w ORMO był zły. Bracia!
            AddWord(newDict, "car");
            AddWord(newDict, "był");
            AddWord(newDict, "zły");
            AddWord(newDict, "bo");
            AddWord(newDict, "mrowisko");
            AddWord(newDict, "boks");
            AddWord(newDict, "ormo");
            AddWord(newDict, "bracia");
            #endregion

            #endregion

            return newDict;
        }
        /// <summary>
        /// Adds a new word entry to the specified dictionary. The word is lower-cased before adding.
        /// </summary>
        /// <param name="dict">Dictionary to add new word to</param>
        /// <param name="newWord">Word to be addded to the dictionary.</param>
        public static void AddWord(XElement dict, string newWord)
        {
            var lcWord = newWord.ToLowerInvariant();
            if (!dict.Elements("w").Any(e => e.Value.Equals(lcWord))) //Add only unique words
            {
                dict.Add(new XElement("w", lcWord));
            }
        }
        /// <summary>
        /// Finds a palindrome for the specified source string.
        /// </summary>
        public static string FindPalindrom(string palindromSource)
        {
            throw new NotImplementedException();
        }
    }
}
