using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Mathematics
{
    /// <summary>
    /// Class used to generate permutations
    /// </summary>
    /// <typeparam name="T">Type of items in the permutations.</typeparam>
    public class Permutations<T>
    {
        /// <summary>Generate permutations of all the items in the <paramref name="items"/> collection.</summary>
        /// <param name="items"></param>
        /// <returns>List of permutations.</returns>
        public List<List<T>> GeneratePermutations(List<T> items)
        {
            // Make an array to hold the
            // permutation we are building.
            T[] current_permutation = new T[items.Count];

            // Make an array to tell whether
            // an item is in the current selection.
            bool[] in_selection = new bool[items.Count];

            // Make a result list.
            List<List<T>> results = new List<List<T>>();

            // Build the combinations recursively.
            PermuteItems(items, in_selection, current_permutation, results, 0);
            return results;
        }

        /// <summary>
        /// Recursively permute the items that are not yet in the current selection.
        /// </summary>
        /// <param name="items">List of permutations</param>
        /// <param name="in_selection"></param>
        /// <param name="current_permutation"></param>
        /// <param name="results"></param>
        /// <param name="next_position"></param>
        private void PermuteItems(List<T> items, bool[] in_selection, T[] current_permutation, List<List<T>> results, int next_position)
        {
            // See if all of the positions are filled.
            if (next_position == items.Count)
            {
                // All of the positioned are filled.
                // Save this permutation.
                results.Add(current_permutation.ToList());
            }
            else
            {
                // Try options for the next position.
                for (int i = 0; i < items.Count; i++)
                {
                    if (!in_selection[i])
                    {
                        // Add this item to the current permutation.
                        in_selection[i] = true;
                        current_permutation[next_position] = items[i];

                        // Recursively fill the remaining positions.
                        PermuteItems(items, in_selection, current_permutation, results, next_position + 1);

                        // Remove the item from the current permutation.
                        in_selection[i] = false;
                    }
                }
            }
        }
    }
}