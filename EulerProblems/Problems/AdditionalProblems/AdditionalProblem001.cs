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
    [ProblemSolver("Persistence problem", "Additional 001",
@"Multiplication number persistence
Based on Numberfile article on number persistence: https://www.youtube.com/watch?v=Wim9WJeDTHQ&ab_channel=Numberphile
for number n calculate how many steps before it reaches 0 or single digit following this 
let n = 3456
Step1: n = pers(n) => 3 * 4 * 5 * 6 => 360
Step2: n = pers(n) => 3 * 6 * 0 => 0
END
return number of steps(2).
")]
    public class AdditionalProblem001 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            UpdateProgress($"Solution not created yet...");


            StringBuilder stringBuilder = new StringBuilder();
            //BigInteger bigInteger = 277777788888899;
            //UInt64 bigInteger =     277777788888899; checked already 11 is max
            //UInt64 bigInteger =     277778163000000; checked already 11 is max
            //UInt64 bigInteger =     277783080000000; checked already 11 is max
            //UInt64 bigInteger =     277816000000000; checked already 11 is max
            UInt64 bigInteger   =     277820000000000; //<current max (program running)
            //                        277969000000000
            int persistence = 0;
            int maxPersistence = 0;

            while (true)
            {
                persistence = Persistence(bigInteger);
                if (persistence > maxPersistence)
                {
                    stringBuilder.Insert(0, $"Multiplication number persistence of ({bigInteger}) = {persistence}. Elapsed time: {ElapsedTime}\n");
                    UpdateProgress(stringBuilder.ToString());
                    maxPersistence = persistence;
                }
                if (bigInteger % 1000000000 == 0)
                {
                    stringBuilder.Insert(0, $"Current ({bigInteger}) = {persistence}. Elapsed time: {ElapsedTime}\n");
                    UpdateProgress(stringBuilder.ToString());
                }
                bigInteger++;
            }

            answer += $"Multiplication number persistence of ({bigInteger}) = {persistence}.";
        }

        //private int Persistence(BigInteger bigInteger)
        private int Persistence(UInt64 bigInteger)
        {
            if (bigInteger < 10) return 0;

            int steps = 0;
            //BigInteger current = bigInteger;
            UInt64 current = bigInteger;
            while (current > 9) //repeat while current multiplication has 2 or more digits
            {
                current = Multiply(current);
                steps++;
            }

            return steps;
        }
        /// <summary>Mutiplies digits in <paramref name="current"/> and returns the multiplication result.</summary>
        /// <param name="current"></param>
        /// <returns>Digit multiplication result</returns>
        //private BigInteger Multiply(BigInteger current)
        private UInt64 Multiply(UInt64 current)
        {
            var bytes = current.ToString().ToArray().Select((b) => b - '0');

            #region Optimization
            if (bytes.Any(b => b == 0)) return 0;
            if (bytes.Any(b => b == 5))
            {
                if (bytes.Any(b => b == 2)) return 10;
                if (bytes.Any(b => b == 4)) return 10;
                if (bytes.Any(b => b == 8)) return 10;
            }
            #endregion

            UInt64 retVal = 1;
            foreach (var b in bytes) retVal *= (UInt64)b;
            return retVal;
        }
    }
}
