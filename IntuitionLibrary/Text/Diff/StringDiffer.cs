using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Text.Diff
{
    /// <summary>
    /// Analyzes two strings and presents an information about how different they are.
    /// <para/>Algorighm implemented after http://en.wikipedia.org/wiki/Diff
    /// and http://en.wikipedia.org/wiki/Longest_common_subsequence_problem.
    /// </summary>
    public class StringDiffer
    {
        /// <summary>
        /// Gets or sets original string.
        /// </summary>
        public string OriginalString { get; set; }

        /// <summary>
        /// Builds a differ that will be able to compare the original string with another string.
        /// </summary>
        /// <param name="originalString">String that will be treated as 'original' when comparing with other strings.</param>
        public StringDiffer(string originalString)
        {
            OriginalString = originalString;
        }

        /// <summary>
        /// Compares the original string with this string.
        /// </summary>
        /// <param name="otherString">String that should be diffed.</param>
        /// <returns>Information about differences between strings.</returns>
        public Difference[] Diff(string otherString)
        {
            List<Difference> differences = new List<Difference>();

            #region Special case: Original is empty
            if (string.IsNullOrEmpty(OriginalString))
            {
                if ((string.IsNullOrEmpty(otherString)))
                {
                    differences.Add(new Same(string.Empty));
                }
                else
                {
                    differences.Add(new Addition(otherString));
                }
                return differences.ToArray();
            }
            #endregion

            #region Special case: Other string is empty (note: we already know the original string is not empty).
            if ((string.IsNullOrEmpty(otherString)))
            {
                differences.Add(new Deletion(OriginalString));
                return differences.ToArray();
            }
            #endregion

            #region Special case: Both strings are equal
            if (OriginalString.Equals(otherString))
            {
                differences.Add(new Same(OriginalString));
                return differences.ToArray();
            }
            #endregion

            #region General case: Find the longest common subsequence
            IEnumerable<char> longestCommonSubsequence = FindLongestCommonSubsequence(OriginalString.ToCharArray(), otherString.ToCharArray());
            #endregion

            return differences.ToArray();
        }

        /// <summary>
        /// Finds the longest common subsequence as described in http://en.wikipedia.org/wiki/Longest_common_subsequence_problem
        /// </summary>
        /// <param name="left">Characters from the left (original) string.</param>
        /// <param name="right">Characters from the right (updated or new) string.</param>
        /// <returns>Longest common sequence found.</returns>
        private IEnumerable<char> FindLongestCommonSubsequence(char[] left, char[] right)
        {
            List<char> sequence = new List<char>();
            var m = GetLCS(left, right);
            var x = left.Length; var y = right.Length;

            var bt = BackTrace(m, left, right, x - 1, y - 1);
            if (bt.Length > 0)
            {
                sequence.AddRange(bt.ToCharArray());
            }

            return sequence;
        }



        private string BackTrace(Int32[,] lcs, char[] left, char[] right, int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return string.Empty;
            }
            if (left[x] == right[y])
            {
                return string.Format("{0}{1}", BackTrace(lcs, left, right, x - 1, y - 1), left[x]);
            }
            if (lcs.PreviousInX(x, y) > lcs.PreviousInY(x, y))
            {
                return BackTrace(lcs, left, right, x - 1, y);
            }
            else
            {
                return BackTrace(lcs, left, right, x, y - 1);
            }
        }

        /// <summary>
        /// Builds the longest common subsequence array for the provided strings.
        /// </summary>
        /// <param name="left">Original string</param>
        /// <param name="right">Updated version of the original string.</param>
        private static int[,] GetLCS(char[] left, char[] right)
        {
            var x = left.Length; var y = right.Length;
            var LCS = new int[x, y];

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (left[i] == right[j])
                    {
                        LCS[i, j] = LCS.PreviousDiagonal(i, j) + 1;
                    }
                    else
                    {
                        LCS[i, j] = Math.Max(LCS.PreviousInX(i, j), LCS.PreviousInY(i, j));
                    }
                }
            }
            return LCS;
        }
    }
    /// <summary>Extension methods for manipulating arrays.</summary>
    public static class Extensions
    {
        /// <summary>Returns the previous diagonal value.</summary>
        public static int PreviousDiagonal(this int[,] me, int x, int y)
        {
            int previousX = x - 1;
            if (previousX < 0) return 0;
            int previousY = y - 1;
            if (previousY < 0) return 0;
            return me[previousX, previousY];
        }
        /// <summary>Returns the previous x-1 value.</summary>
        public static int PreviousInX(this int[,] me, int x, int y)
        {
            int previousX = x - 1;
            if (previousX < 0) return 0;
            return me[previousX, y];
        }
        /// <summary>Returns the previous y-1 value.</summary>
        public static int PreviousInY(this int[,] me, int x, int y)
        {
            int previousY = y - 1;
            if (previousY < 0) return 0;
            return me[x, previousY];
        }
    }

    /// <summary>
    /// A difference type that denotes 'sameness' (namely, this difference is actually no difference at all).
    /// </summary>
    public class Same : Difference
    {
        public string SameText { get; set; }

        /// <summary>
        /// Constructs an indicator that a string item is 'same'.
        /// </summary>
        /// <param name="text">Substring that is deemed as 'same'.</param>
        public Same(string text)
        {
            SameText = text;
        }

        /// <summary>
        /// User-friendly display of the contents of this difference.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Same: {0}", SameText);
        }
    }

    /// <summary>
    /// A difference type that is a text addition.
    /// </summary>
    public class Addition : Difference
    {
        public string AddedText { get; set; }

        /// <summary>
        /// Constructs an indicator that a string item has been added to the original string.
        /// </summary>
        /// <param name="text">Substring that is deemed as 'addition'.</param>
        public Addition(string text)
        {
            AddedText = text;
        }

        /// <summary>
        /// User-friendly display of the contents of this difference.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Added: {0}", AddedText);
        }
    }

    /// <summary>
    /// A difference type that is a text removal.
    /// </summary>
    public class Deletion : Difference
    {
        public string DeletedText { get; set; }

        /// <summary>
        /// Constructs an indicator that a string item has been added to the original string.
        /// </summary>
        /// <param name="text">Substring that is deemed as 'addition'.</param>
        public Deletion(string text)
        {
            DeletedText = text;
        }

        /// <summary>
        /// User-friendly display of the contents of this difference.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Deleted: {0}", DeletedText);
        }
    }

    /// <summary>
    /// Base class for all differences
    /// </summary>
    public abstract class Difference
    {
        /// <summary>
        /// User-friendly display of the contents of this difference.
        /// </summary>
        public override string ToString()
        {
            return "Difference";
        }
    }
}
