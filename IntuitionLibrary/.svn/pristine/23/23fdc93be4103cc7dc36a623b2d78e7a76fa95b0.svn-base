using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Mathematics
{
    /// <summary>
    /// Calculates a http://en.wikipedia.org/wiki/Continued_fraction Continued Fraction
    /// that represents a square root of the specified number.
    /// </summary>
    public class ContinuedFractionSquare
    {
        /// <summary>
        /// Constructs an instance that is able to calculate the Continued Fraction that represents 
        /// a square root of the specified <paramref name="liczba"/> number.
        /// </summary>
        /// <param name="liczba">Number, whose square root is to be represented as continued fraction</param>
        public ContinuedFractionSquare(double liczba)
        {
            Liczba = liczba;
            SekwencjaUłamkaCiągłego = new List<int>();
        }
        double Liczba;
        double Pierwiastek { get { return Math.Sqrt(Liczba); } }
        /// <summary>
        /// Finds the continued fraction for the square of the specified number.
        /// </summary>
        public bool Znajdź()
        {
            A = (int)Math.Floor(Pierwiastek);
            ZnalezionaNastępnaLiczba(A);
            int maksymalnaLiczbaIteracji = 200;
            int bieżącaIteracja = 1;
            while (bieżącaIteracja++ < maksymalnaLiczbaIteracji)
            {
                if (BieżącyUłamekCiągły == Pierwiastek) return true;

                #region Find palindrome
                bool TestWejściowy = BieżącyUłamekCiągły.JestMniejszeNiż(Pierwiastek);
                //Find such new candidate until CurrentContinuedFraction exceeds sq
                int nowyKandydat = 1;
                bool TestBieżący = NowyUłamekTestowy(nowyKandydat).JestMniejszeNiż(Pierwiastek); //This has to be opposite result from the entry result
                if (TestBieżący == TestWejściowy) throw new Exception(); //This is not expected to happen.
                //Check if this is the last candidate before changing the sign again
                while (NowyUłamekTestowy(nowyKandydat).JestMniejszeNiż(Pierwiastek) == TestBieżący) nowyKandydat++;
                nowyKandydat--; //Return to the previous number, just before changing the sign.
                DodajNowyElementPalindromu(nowyKandydat);
                #endregion
                //Verify if this is a palindrome indeed followed by double A
                if (CzyToJestPalindromPoKtórymWystępuje2A) return true;
            }
            return false;
        }
        private double BieżącyUłamekCiągły
        {
            get
            {
                return A + WartośćUłamka();
            }
        }
        private double NowyUłamekTestowy(int newCandidate)
        {
            return A + WartośćUłamka(newCandidate);
        }
        /// <summary>
        /// Calculates the remaining portion of the number after A
        /// 1/(B+(1/(C+(1/(...)))))
        /// </summary>
        private double WartośćUłamka()
        {
            if (SekwencjaUłamkaCiągłego.Count == 0) return 0;
            double current = 0;
            foreach (var Bx in SekwencjaUłamkaCiągłego.ToArray().Reverse())
            {
                current = 1 / (Bx + current);
            }
            return current;
        }
        /// <summary>
        /// Calculates the remaining portion of the number after A
        /// 1/(B+(1/(C+(1/(.../newCandidate)))))
        /// </summary>
        private double WartośćUłamka(int newCandidate)
        {
            double wartośćBieżąca = 1 / (double)newCandidate;
            foreach (var Bx in SekwencjaUłamkaCiągłego.ToArray().Reverse())
            {
                wartośćBieżąca = 1 / (Bx + wartośćBieżąca);
            }
            return wartośćBieżąca;
        }
        private void DodajNowyElementPalindromu(int newItem)
        {
            SekwencjaUłamkaCiągłego.Add(newItem);
            ZnalezionaNastępnaLiczba(newItem);
        }
        List<int> SekwencjaUłamkaCiągłego;
        /// <summary>
        /// Gets a copy of the current sequence that has been calculated so far. It may not be complete.
        /// It may contain 0 elements.
        /// </summary>
        public IEnumerable<int> SekwencjaNieobrobiona
        {
            get
            {
                return SekwencjaUłamkaCiągłego.ToArray();
            }
        }
        /// <summary>
        /// Gets the final computed palindrome sequence (except for the first element A, which is stored as <see cref="A"/> member).
        /// The terminating number (equal to 2A) is not included. This is the palindrome sequence alone.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when an attempt is made to get the palindrome, when there is no palindrome.</exception>
        public IEnumerable<int> Palindrom
        {
            get
            {
                if (!CzyToJestPalindromPoKtórymWystępuje2A) throw new InvalidOperationException("This is not a palindromic sequence, cannot obtain a palindrom");
                return SekwencjaUłamkaCiągłego.Take(SekwencjaUłamkaCiągłego.Count - 1);
            }
        }
        /// <summary>
        /// Checks if the sequence calculated so far already makes up a palindrome.
        /// </summary>
        public bool CzyToJestPalindromPoKtórymWystępuje2A
        {
            get
            {
                if (SekwencjaUłamkaCiągłego.Count == 0)
                {
                    if (BieżącyUłamekCiągły == Pierwiastek) return true;
                    return false;
                }
                if (SekwencjaUłamkaCiągłego.Last() != 2 * A) return false;
                if (CzyToJestPalindrom(SekwencjaUłamkaCiągłego.Take(SekwencjaUłamkaCiągłego.Count - 1)))
                {
                    //if (ContinuedSequence.Count < 4) //For short sequences do double checking
                    //{
                    //    //Double check if we are not terminating early.
                    //    //Make sure that the square root computed by the CPU is not too far (due to round-off errors on doubles)
                    //    //from what we have calculated so far. If the discrepancy is greater than very minute,
                    //    //probably the palindrome-looking sequence is not really the final one.
                    //    double MaxError = double.Epsilon * 1024.0 * 1024.0 * 1024.0 * 1024.0;
                    //    if (Math.Abs(CurrentContinuedFraction - Pierwiastek) > MaxError) //Treat discrepancies up to specific accuracy as 0.
                    //    {
                    //        return false;
                    //    }
                    //}
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Checks whether the numbers in the sequence form a palindromic structure.
        /// </summary>
        /// <returns>True if it is a palindrome</returns>
        private bool CzyToJestPalindrom(IEnumerable<int> sekwencja)
        {
            var długośćPalindromu = sekwencja.Count();
            if (długośćPalindromu == 0) return false; //Empty sequence is not a palindrome
            if (długośćPalindromu == 1) return true; //One-element sequence is a palindrome by definition.
            foreach (int indeks in Enumerable.Range(0, długośćPalindromu))
            {
                if (sekwencja.ElementAt(indeks) != sekwencja.ElementAt(długośćPalindromu - 1 - indeks)) return false;
                if (indeks * 2 > długośćPalindromu) return true; //No point in checking the rest of the sequence, because it was already checked.
            }
            return true;
        }
        /// <summary>
        /// Action that is executed every time a new sequence entry has been found.
        /// </summary>
        public Action<int> ZnalezionaNastępnaLiczba;
        /// <summary>
        /// First element in the continued fraction expression.
        /// </summary>
        public int A { get; private set; }
    }
    public static class Rozszerzenia
    {
        public static bool JestWiększeNiż(this double ja, double co)
        {
            return ja > co;
        }
        public static bool JestMniejszeNiż(this double ja, double co)
        {
            return ja < co;
        }
    }
}
