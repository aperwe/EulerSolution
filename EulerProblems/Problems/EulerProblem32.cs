using QBits.Intuition.Mathematics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// We shall say that an n-digit number is pandigital if it makes use of all the digits 1 to n exactly once; for example, the 5-digit number, 15234, is 1 through 5 pandigital.
    /// The product 7254 is unusual, as the identity, 39 × 186 = 7254, containing multiplicand, multiplier, and product is 1 through 9 pandigital.
    /// Find the sum of all products whose multiplicand/multiplier/product identity can be written as a 1 through 9 pandigital.
    /// HINT: Some products can be obtained in more than one way so be sure to only include it once in your sum.
    /// </summary>
    [ProblemSolverClass("Problem 32", DisplayName = "Problem 32")]
    public class EulerProblem32 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Permutations<int> permutations = new Permutations<int>();
            var allSequencesToCheck = permutations.GeneratePermutations(new List<int>(numbers));
            long iterationCounter = 0;
            List<PandigitCandidate> allCombinations = new List<PandigitCandidate>();

            //All sequences of length 9 are ready.
            //foreach (var sequence in allSequencesToCheck)
            allSequencesToCheck.AsParallel().ForAll(sequence =>
            {
                // Now break each sequence into lengths of 2 through 9
                foreach (int i in Enumerable.Range(1, 9))
                {
                    foreach (int j in Enumerable.Range(1, 9))
                    {
                        int totalLength = i + j;
                        if (totalLength > 8) continue; //If 9 or longer, there is no space for product digits, so no point in continuing. This is definitely not a candidate.
                        lock (this) iterationCounter++;
                        var fistSequence = sequence.Take(i);
                        var secondSequence = sequence.Skip(i).Take(j);
                        var tester = new PandigitCandidate(fistSequence, secondSequence);
                        if (tester.IsATruePandigit()) //Test early in parallel, to maximize effort spent in parallel
                            lock (allCombinations) allCombinations.Add(tester);
                    }
                }
            }
            );

            //All combinations to check for pandigitability are in allCombinations collection.
            //Select only those that are pandigital.
            var newCollection = from candidate in allCombinations
                                where candidate.IsATruePandigit()
                                select candidate;
            var finalList = newCollection.ToList();
            //Since there are multiple results in the list, group them by product to make sure we include only one distinct product.
            var uniqueList = finalList.GroupBy(candidate => candidate.Product);
            StringBuilder answerPart = new StringBuilder().AppendLine();
            long sumProducts = 0;
            foreach (var group in uniqueList)
            {
                answerPart.AppendLine(group.First().ToString());
                sumProducts += group.Key;
            }

            answer = string.Format("All sequences = {0}. Loop iterations: {1}. Results: {2}. Sum of unique products: {3}. List of results: {4}", allSequencesToCheck.Count, iterationCounter, finalList.Count, sumProducts, answerPart);
        }
    }
    /// <summary>
    /// Single instance describing a combination of multiplicand*multiplier=product.
    /// </summary>
    internal class PandigitCandidate
    {
        static char[] digits = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        internal long Multiplicand { get; private set; }
        internal long Multiplier { get; private set; }
        internal long Product => Multiplicand * Multiplier;
        /// <summary>Creates an instance of the candidate with the specified multiplicand and multiplier.</summary>
        /// <param name="multiplicand">Value of the multiplicand.</param>
        /// <param name="multiplier">Value of the multiplier.</param>
        public PandigitCandidate(IEnumerable<int> multiplicand, IEnumerable<int> multiplier)
        {
            Multiplicand = MoreMath.IntFromDigits(multiplicand);
            Multiplier = MoreMath.IntFromDigits(multiplier);
        }
        /// <summary>Displays info aout this <seealso cref="PandigitCandidate"/>.</summary>
        public override string ToString()
        {
            return string.Format("{0} * {1} = {2}", Multiplicand, Multiplier, Product);
        }

        /// <summary>Checks if the specified combination of a,b, and a*b is a Pandigit combination.</summary>
        /// <returns>True if it is, false if it is not.</returns>
        internal bool IsATruePandigit()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Multiplicand);
            sb.Append(Multiplier);
            sb.Append(Product);
            if (sb.Length != 9) return false; //Required but not sufficient
            var array = sb.ToString().ToCharArray();
            var result = digits.All(digit => array.Contains(digit)); //Check that all digits 1-9 are contained within the array.
            return result;
        }
        /// <summary>
        /// Returns all digits as a sorted string.
        /// </summary>
        /// <returns></returns>
        internal string AllDigitsSorted()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Multiplicand);
            sb.Append(Multiplier);
            sb.Append(Product);
            return sb.ToString().OrderBy(c => c).ToString();
        }

    }
}
