using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// If the numbers 1 to 5 are written out in words: one, two, three, four, five, then there are 3 + 3 + 5 + 4 + 4 = 19 letters used in total.

    /// If all the numbers from 1 to 1000 (one thousand) inclusive were written out in words, how many letters would be used?

    /// NOTE: Do not count spaces or hyphens.
    /// For example, 342 (three hundred and forty-two) contains 23 letters and 115 (one hundred and fifteen) contains 20 letters.
    /// The use of "and" when writing out numbers is in compliance with British usage.
    /// </summary>
    public class EulerProblem17 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            int countOfLetters = Enumerable.Range(0, 1001).Aggregate((p, q) => p + CountLetters(q));
            answer = string.Format("If all the numbers from 1 to 1000 (one thousand) inclusive were written out in words, the number of letters that would be used is {0}.", countOfLetters);
        }

        /// <summary>
        /// Counts letters in <paramref name="number"/>
        /// </summary>
        /// <returns>Number of letters</returns>
        private int CountLetters(int number)
        {
            var numberTranslator = new NumberTranslator(number);
            int count = 0;

            count += numberTranslator.CountThousands();
            count += numberTranslator.CountHundreds();
            count += numberTranslator.CountAndsBetweenHundredsAndTens();
            count += numberTranslator.CountTensAbove19();
            count += numberTranslator.CountTeensAndSingles();

            return count;
        }


        private class NumberTranslator
        {
            #region Interface methods computing number of letters
            public override string ToString()
            {
                return number.ToString();
            }
            internal int CountThousands()
            {
                int thousandCountString = CountDigitsUnder10(thousands);
                if (thousandCountString > 0)
                {
                    thousandCountString += "thousand".Count();
                }
                return thousandCountString;
            }

            /// <summary>
            /// Returns the length of string for hundreds. Eg. for 200 it returns the length of string "two" and "hundred".
            /// </summary>
            internal int CountHundreds()
            {
                int hundredCountString = CountDigitsUnder10(hundreds);
                if (hundredCountString>0)
                {
                    hundredCountString += "hundred".Count();

                }
                return hundredCountString;
            }

            internal int CountTensAbove19()
            {
                return CountTens(tensAbove19);
            }

            internal int CountTeensAndSingles()
            {
                return CountDigits0Through19(teensAndSingles);
            }

            /// <summary>
            /// Counts "and" words between hudreds and tens or sigle digits.
            /// If there is a hundred or more followed by non-zero tens or single digits, then an "and" should be inserted.
            /// </summary>
            /// <returns></returns>
            internal int CountAndsBetweenHundredsAndTens()
            {
                if (hundreds > 0)
                {
                    if (tensAbove19 > 0 || teensAndSingles > 0)
                    {
                        return "and".Count();
                    }
                }
                return 0;
            }

            /// <summary>
            /// </summary>
            /// <param name="digit">Digit in the range of 0 to 19</param>
            /// <returns></returns>
            private int CountDigits0Through19(int digit)
            {
                if (digit >= 0 && digit < 10) return CountDigitsUnder10(digit);
                switch (digit)
                {
                    case 10: return "ten".Count(); ;
                    case 11: return "eleven".Count();
                    case 12: return "twelve".Count();
                    case 13: return "thirteen".Count();
                    case 14: return "fourteen".Count();
                    case 15: return "fifteen".Count();
                    case 16: return "sixteen".Count();
                    case 17: return "seventeen".Count();
                    case 18: return "eighteen".Count();
                    case 19: return "nineteen".Count();
                    default: throw new ArgumentOutOfRangeException("digit", digit.ToString());
                }
            }

            private int CountTens(int digit)
            {
                switch (digit)
                {
                    case 0: return 0;
                    case 1: throw new InvalidOperationException("Unexpected value of digit found.");
                    case 2: return "twenty".Count();
                    case 3: return "thirty".Count();
                    case 4: return "forty".Count();
                    case 5: return "fifty".Count();
                    case 6: return "sixty".Count();
                    case 7: return "seventy".Count();
                    case 8: return "eighty".Count();
                    case 9: return "ninety".Count();
                    default: throw new ArgumentOutOfRangeException("digit", digit.ToString());
                }
            }

            /// <summary>
            /// Return a number of letters in a number.
            /// </summary>
            /// <param name="digit">Digit in the range of 0-9</param>
            internal int CountDigitsUnder10(int digit)
            {
                switch (digit)
                {
                    case 0: return 0;
                    case 1: return "one".Count();
                    case 2: return "two".Count();
                    case 3: return "three".Count();
                    case 4: return "four".Count();
                    case 5: return "five".Count();
                    case 6: return "six".Count();
                    case 7: return "seven".Count();
                    case 8: return "eight".Count();
                    case 9: return "nine".Count();
                    default: throw new ArgumentOutOfRangeException("digit", digit.ToString());
                }
            }

            #endregion

            private int number;

            public NumberTranslator(int number)
            {
                this.number = number;
                Convert();
            }

            public int hundreds { get; private set; }
            public int thousands { get; private set; }
            public int tensAbove19 { get; private set; }
            public int teensAndSingles { get; private set; }

            private void Convert()
            {
                int reminder = number;
                reminder = FindThousands(reminder);
                reminder = FindHundreds(reminder);
                reminder = FindTensAbove19(reminder);
                reminder = FindTeensAndSingles(reminder);
            }

            private int FindTeensAndSingles(int reminder)
            {
                int magnitude = 1;
                int temp = reminder / magnitude;
                #region Check parameter validity
                if (temp > 19) throw new ArgumentOutOfRangeException("reminder", reminder.ToString());
                if (temp < 0) throw new ArgumentOutOfRangeException("reminder", reminder.ToString());
                #endregion
                teensAndSingles = temp;

                return reminder - (temp * magnitude);
            }

            /// <summary>
            /// Isolates numbers in range 20-99. Teens and single digit numbers will be in one function as last reminder.
            /// </summary>
            /// <returns>Reminder after deducting the tens. If <param name="reminder"/> is in the range 0-19, nothing is modified and the same value of <param name="reminder"/> is returned.</returns>
            private int FindTensAbove19(int reminder)
            {
                int magnitude = 10;
                int temp = reminder / magnitude;
                #region Check parameter validity
                if (temp > 9) throw new ArgumentOutOfRangeException("reminder", reminder.ToString());
                if (temp < 2)
                {
                    tensAbove19 = 0;
                    return reminder;
                }
                #endregion
                tensAbove19 = temp;

                return reminder - (temp * magnitude);
            }

            private int FindHundreds(int reminder)
            {
                int magnitude = 100;
                int temp = reminder / magnitude;
                #region Check parameter validity
                if (temp > 9) throw new ArgumentOutOfRangeException("reminder", reminder.ToString());
                #endregion
                hundreds = temp;

                return reminder - (temp * magnitude);
            }

            private int FindThousands(int reminder)
            {
                int magnitude = 1000;
                int temp = reminder / magnitude;
                #region Check parameter validity
                if (temp > 99) throw new ArgumentOutOfRangeException("reminder", reminder.ToString());
                #endregion
                thousands = temp;

                return reminder - (temp * magnitude);
            }

        }
    }
}
