using QBits.Intuition.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// The fraction 49/98 is a curious fraction, as an inexperienced mathematician in attempting to simplify it may incorrectly believe that 49/98 = 4/8,
    /// which is correct, is obtained by cancelling the 9s.
    /// 
    /// We shall consider fractions like, 30/50 = 3/5, to be trivial examples.
    /// There are exactly four non-trivial examples of this type of fraction, less than one in value, and containing two digits in the numerator and denominator.
    /// If the product of these four fractions is given in its lowest common terms, find the value of the denominator.
    /// </summary>
    [ProblemSolverClass("Problem 33", DisplayName = "Problem 33")]
    public class EulerProblem33 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            int counter = 0;
            List<FractionRepresentation> list = new List<FractionRepresentation>();
            //Loop over 2-digit nominators
            ParallelEnumerable.Range(10, 90).ForAll(nominator =>
            //foreach (int nominator in Enumerable.Range(10, 90))
            {
                foreach (int denominator in Enumerable.Range(10, 90))
                {
                    FractionRepresentation representation = new FractionRepresentation(nominator, denominator);
                    if (representation.IsLessThanOne) //Skip fractions bigger than 1
                    {
                        if (representation.HasAtLeastOneSameDigitInNominatorAndDenominator)
                        {
                            if (representation.RemovingDigitDoesNotChangeValue)
                            {
                                if (representation.IsCuriousFraction)
                                {
                                    lock (this)
                                    {
                                        counter++;
                                        list.Add(representation);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            );
            //Translate original fractions to their shorter versions
            List<FractionRepresentation> simpleFractions = (from item in list select item.ShorterValue).ToList();

            //Finally, from simple versions of the final set of fractions create a single fraction
            int finalN = 1, finalD = 1;

            simpleFractions.ForEach(fraction => { finalN *= fraction.Nominator; finalD *= fraction.Denominator; });
            int nwd = MoreMath.NWD(finalN, finalD);
            FractionRepresentation final = new FractionRepresentation(finalN / nwd, finalD / nwd);


            StringBuilder DEBUGString = new StringBuilder().AppendLine();
            list.ForEach(item => DEBUGString.AppendLine($"{item.Nominator}/{item.Denominator}"));
            DEBUGString.AppendLine("=== Shorter ===");
            simpleFractions.ForEach(item => DEBUGString.AppendLine($"{item.Nominator}/{item.Denominator}"));
            DEBUGString.AppendLine("=== Final ===");
            DEBUGString.AppendLine($"{final.ToString()}");
            answer = $"Computing... {counter}/{list.Count}. {DEBUGString} Final denominator is {final.Denominator}.";
        }

        /// <summary>
        /// Helper class to represent a fraction.
        /// </summary>
        internal class FractionRepresentation
        {
            public FractionRepresentation(int nominator, int denominator)
            {
                Nominator = nominator;
                Denominator = denominator;
            }

            public FractionRepresentation(string newNominator, string newDenominator)
            {
                try
                {
                    Nominator = int.Parse(newNominator);
                }
                catch { Nominator = 0; }
                try
                {
                    Denominator = int.Parse(newDenominator);
                }
                catch { Denominator = 0; }
            }

            public override string ToString()
            {
                return $"{Nominator}/{Denominator}";
            }
            /// <summary>
            /// Nominator
            /// </summary>
            internal int Nominator { get; private set; }
            internal int Denominator { get; private set; }
            /// <summary>
            /// If Denominator == 0, -1 is returned as error value.
            /// </summary>
            double FractionValue => Denominator == 0 ? -1 : (double)Nominator / (double)Denominator;
            string NominatorString => Nominator.ToString();
            string DenominatorString => Denominator.ToString();
            /// <summary>
            /// True if the fraction is less than 1.
            /// </summary>
            internal bool IsLessThanOne => Nominator < Denominator;
            /// <summary>
            /// Finds whether there is at least 1 digit that is common in nominator and denominator
            /// </summary>
            public bool HasAtLeastOneSameDigitInNominatorAndDenominator
            {
                get
                {
                    var repeatedDigits = ListOfDigitsRepeatedInNominatorAndDenominator;
                    return repeatedDigits.Length > 0;
                }
            }

            private char[] ListOfDigitsRepeatedInNominatorAndDenominator
            {
                get
                {
                    var nomDigits = NominatorString.ToArray();
                    var denomDigits = DenominatorString.ToArray();
                    List<char> repetitiveDigits = new List<char>();
                    foreach (var nomDigit in nomDigits)
                    {
                        if (denomDigits.Contains(nomDigit))
                        {
                            repetitiveDigits.Add(nomDigit);
                        }
                    }
                    return repetitiveDigits.ToArray();
                }
            }

            /// <summary>
            /// Tests if removing repeated digits from nominator and denominator does not change the value of the fraction.
            /// Returns true if it doesn't, false if value is different.
            /// </summary>
            public bool RemovingDigitDoesNotChangeValue
            {
                get
                {
                    var newFraction = ShorterValue;

                    var newFractionValue = newFraction.FractionValue;
                    return newFractionValue == this.FractionValue;
                }
            }

            /// <summary>
            /// Assuming that fraction is already a non-changer, check if it is curious (i.e. non Trivial).
            /// Returns true if the fraction is non trivial.
            /// </summary>
            public bool IsCuriousFraction
            {
                get
                {
                    var repetitions = ListOfDigitsRepeatedInNominatorAndDenominator;
                    var repetition = repetitions.First();

                    var nom = NominatorString;
                    var denom = DenominatorString;
                    if ((nom.First() == repetition) && (denom.Last() == repetition)) return true;
                    if ((nom.Last() == repetition) && (denom.First() == repetition)) return true;
                    return false;
                }
            }

            /// <summary>
            /// For curious fractions it will return the shorter version of fraction with redundant numbers removed.
            /// </summary>
            public FractionRepresentation ShorterValue
            {
                get
                {
                    var digitsToRemove = ListOfDigitsRepeatedInNominatorAndDenominator;
                    var firstDigitToRemove = digitsToRemove.First();
                    var newNominator = new string(NominatorString.Where(c => c != firstDigitToRemove).ToArray());
                    var newDenominator = new string(DenominatorString.Where(c => c != firstDigitToRemove).ToArray());
                    var newFraction = new FractionRepresentation(newNominator, newDenominator);
                    return newFraction;
                }
            }
        }
    }
}
