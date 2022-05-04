using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EulerProblems.Problems.AdditionalProblems
{
    /// <summary/>
    [ProblemSolver("Shows pairs of primes, separated by no more than 1 number", "Additional problem 002",
@"Find and type all pairs of primes, separated by no more than 2 numbers (e.g. 3 and 5 is a pair, 41 and 43 is a pair, but 13 and 17 is not a pair).")]
    public class AdditionalProblem002 : AbstractEulerProblem
    {
        PrimeSolver primeSolver = new PrimeSolver();
        object locker = new object();
        protected override void Solve(out string answer)
        { 
            answer = $"Solution not created yet...";
            long previousPrime = 0, nextPrime = 0, lastPairLow = 0;
            long pairCount = 0;
            long sumDistance = 0;
            long screenUpdateInterval = 1000; //Update screen every 1000 pairs found

            ///Main petla
            for (long testedPrime = 3; ; testedPrime += 2)
            {
                if (primeSolver.IsPrime(testedPrime))
                {
                    previousPrime = nextPrime;
                    nextPrime = testedPrime;
                    long diff = nextPrime - previousPrime;
                    if (diff == 2) //it is a pair
                    {
                        pairCount++;
                        var distanceFromPrevious = previousPrime - lastPairLow;
                        sumDistance += distanceFromPrevious;
                        if (pairCount % screenUpdateInterval == 0)
                        {
                            this.UpdateProgress($"Pair: ({previousPrime:n0}, {nextPrime:n0}).\n Distance: {distanceFromPrevious}.\n Average distance: {sumDistance / pairCount}\n Pairs found: {pairCount}.");
                        }

                        lastPairLow = previousPrime; //Remember this pair for next iteration
                    }
                }
            }


            answer += $"Finished.";
        }

    }
}