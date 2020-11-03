using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary/>
    [ProblemSolver("Lexicographic permutations", displayName = "Problem 24", problemDefinition =
@"A permutation is an ordered arrangement of objects. For example, 3124 is one possible permutation of the digits 1, 2, 3 and 4. If all of the permutations are listed numerically or alphabetically, we call it lexicographic order. The lexicographic permutations of 0, 1 and 2 are:

                  012   021   102   120   201   210

What is the millionth lexicographic permutation of the digits 0, 1, 2, 3, 4, 5, 6, 7, 8 and 9?"
        )]
    public class EulerProblem24 : AbstractEulerProblem
    {
        const char c0 = '0';
        const char c9 = '9';

        protected override void Solve(out string answer)
        {
            //Perm[0000001]: 0 1 2 3 4 5 6 7 8 9
            //Perm[0000002]: 0 1 2 3 4 5 6 7 9 8
            //Perm[0000003]: 0 1 2 3 4 5 6 8 7 9
            //Perm[0000004]: 0 1 2 3 4 5 6 8 9 7
            //Perm[0000005]: 0 1 2 3 4 5 7 6 8 9
            //Perm[0000006]: 0 1 2 3 4 5 7 6 9 8
            //Perm[0000007]: 0 1 2 3 4 5 7 8 6 9
            //Perm[0000008]: 0 1 2 3 4 5 7 8 9 6

            List<string> permutations = new List<string>();

            ParallelEnumerable.Range(c0, 9).ForAll(inti0 =>
            //for (char i0 = c0; i0 <= c9; i0++)
            {
                char i0 = (char)inti0;

                for (char i1 = c0; i1 <= c9; i1++)
                {
                    if (i1 == i0) continue;//In permutations numbers cannot repeat
                    for (char i2 = c0; i2 <= c9; i2++)
                    {
                        if (i2 == i1) continue;
                        if (i2 == i0) continue;
                        for (char i3 = c0; i3 <= c9; i3++)
                        {
                            if (i3 == i2) continue;
                            if (i3 == i1) continue;
                            if (i3 == i0) continue;
                            for (char i4 = c0; i4 <= c9; i4++)
                            {
                                if (i4 == i3) continue;
                                if (i4 == i2) continue;
                                if (i4 == i1) continue;
                                if (i4 == i0) continue;
                                for (char i5 = c0; i5 <= c9; i5++)
                                {
                                    if (i5 == i4) continue;
                                    if (i5 == i3) continue;
                                    if (i5 == i2) continue;
                                    if (i5 == i1) continue;
                                    if (i5 == i0) continue;
                                    for (char i6 = c0; i6 <= c9; i6++)
                                    {
                                        if (i6 == i5) continue;
                                        if (i6 == i4) continue;
                                        if (i6 == i3) continue;
                                        if (i6 == i2) continue;
                                        if (i6 == i1) continue;
                                        if (i6 == i0) continue;
                                        for (char i7 = c0; i7 <= c9; i7++)
                                        {
                                            if (i7 == i6) continue;
                                            if (i7 == i5) continue;
                                            if (i7 == i4) continue;
                                            if (i7 == i3) continue;
                                            if (i7 == i2) continue;
                                            if (i7 == i1) continue;
                                            if (i7 == i0) continue;
                                            for (char i8 = c0; i8 <= c9; i8++)
                                            {
                                                if (i8 == i7) continue;
                                                if (i8 == i6) continue;
                                                if (i8 == i5) continue;
                                                if (i8 == i4) continue;
                                                if (i8 == i3) continue;
                                                if (i8 == i2) continue;
                                                if (i8 == i1) continue;
                                                if (i8 == i0) continue;
                                                for (char i9 = c0; i9 <= c9; i9++)
                                                {
                                                    if (i9 == i8) continue;
                                                    if (i9 == i7) continue;
                                                    if (i9 == i6) continue;
                                                    if (i9 == i5) continue;
                                                    if (i9 == i4) continue;
                                                    if (i9 == i3) continue;
                                                    if (i9 == i2) continue;
                                                    if (i9 == i1) continue;
                                                    if (i9 == i0) continue;

                                                    List<char> list = new List<char>() { i0, i1, i2, i3, i4, i5, i6, i7, i8, i9 };
                                                    string permutationString = new string(list.ToArray());
                                                    lock (permutations)
                                                    {
                                                        permutations.Add(permutationString);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            );

            //Now sort the permutations alphabetically and get the millionth
            permutations.Sort();
            var millionthPermutation = permutations.Skip(999999).First();


            answer = string.Format("All permuations {0}. Millionth permutation is {1}.", permutations.Count, millionthPermutation);
        }

    }
}
