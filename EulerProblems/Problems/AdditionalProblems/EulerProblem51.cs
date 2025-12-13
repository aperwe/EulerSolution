using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using QBits.Intuition.Text;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EulerProblems.Problems.AdditionalProblems
{
    /// <summary/>
    [ProblemSolver("<Under construction>", "Powers of 2^10^n [TODO]",
@"Hidden pattern in the powers of 2^10^n
Described here:
https://www.youtube.com/shorts/X4C3Sz-10eA")]
    public class PowersOf2To10ToN : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            //Test Int128 operations for speed
            //SolveInt128(out answer);
            //Test Int128 operations for speed
            SolveInt64(out answer);
        }
        protected void SolveInt64(out string answer)
        {
            Int64 n = 0; //Starting index of the exponent of 2^10^n => so 2^10^0 = 2^1 = 2
            Int64 value = 2; //Starting value = 2^1 = 2
            //Calculate starting value of 10^n = 10^0 = 1
            Int64 tenToN = (Int64)Math.Pow(10, (double)n);

            answer = $"Starting Int64"; UpdateProgress(answer);

            //Limits
            var truncateLimit = Int64.MaxValue / 2;
            //We need to establish a 10-base value, whose most significant digit is the same as truncateLimit, and the rest are zeros
            var truncateLimitStr = truncateLimit.ToString();
            truncateLimitStr = truncateLimitStr[0] + new string('0', truncateLimitStr.Length - 1);
            truncateLimit = Int64.Parse(truncateLimitStr);
            var lengthToKeep = truncateLimitStr.Length - 1;
            var multiplicator = 2;
            // Create an Enumerable lambda expression that assumes multiplicator is a power of 2 and returns backwards what the power of 2 it was. For example, for multiplicator = 1024 it returns 10, for multiplicator = 2048 it returns 11, etc.
            var powerOfTwoFromMultiplicator = new Func<Int64, Int64>((mul) =>
            {
                Int64 power = 0;
                Int64 val = 1;
                while (val < mul)
                {
                    val *= 2;
                    power++;
                }
                return power;
            });
            //Here the powerOfTwoFromMultiplicator(multiplicator) should return 10
            Int64 xxxx = powerOfTwoFromMultiplicator(multiplicator);

            for (Int64 exponent = 1; ; exponent++)
            {


                //Check if exponent is a power of 10
                if (exponent == tenToN)
                {

                    answer += $"2^10^{n} = 2^{exponent} = {value.ToString()}.{Environment.NewLine}";
                    UpdateProgress(answer);
                    n++;

                    //Update 10^n for the next n'th
                    tenToN = (Int64)Math.Pow(10, (double)n);
                }

                //After fist iteration, go to the next power of 2
                value *= multiplicator; //Calculate 2^exponent

                //Truncate the most significant digit of value to avoid overflow, while retaining all trailing digits
                if (value > truncateLimit)
                {
                    var prevValue = value;
                    value -= truncateLimit;
                }

            }

        }
        protected void SolveInt128(out string answer)
        {
            int n = 0; //Starting index of the exponent of 2^10^n => so 2^10^0 = 2^1 = 2
            Int128 value = 2; //Starting value = 2^1 = 2
            //Calculate starting value of 10^n = 10^0 = 1
            Int64 tenToN = (Int64)Math.Pow(10, (double)n);

            answer = $"Starting Int128"; UpdateProgress(answer);

            //Limits
            var truncateLimit = Int128.MaxValue / 2;
            //We need to establish a 10-base value, whose most significant digit is the same as truncateLimit, and the rest are zeros
            var truncateLimitStr = truncateLimit.ToString();
            truncateLimitStr = truncateLimitStr[0] + new string('0', truncateLimitStr.Length - 1);
            truncateLimit = Int128.Parse(truncateLimitStr);
            var lengthToKeep = truncateLimitStr.Length - 1;
            var multiplicator = 2;
            // Create an Enumerable lambda expression that assumes multiplicator is a power of 2 and returns backwards what the power of 2 it was. For example, for multiplicator = 1024 it returns 10, for multiplicator = 2048 it returns 11, etc.
            var powerOfTwoFromMultiplicator = new Func<Int128, int>((mul) =>
            {
                int power = 0;
                Int128 val = 1;
                while (val < mul)
                {
                    val *= 2;
                    power++;
                }
                return power;
            });
            //Here the powerOfTwoFromMultiplicator(multiplicator) should return 10
            int xxxx = powerOfTwoFromMultiplicator(multiplicator);

            for (int exponent = 1; ; exponent++)
            {


                //Check if exponent is a power of 10
                if (exponent == tenToN)
                {

                    answer += $"2^10^{n} = 2^{exponent} = {value.ToString()}.{Environment.NewLine}";
                    UpdateProgress(answer);
                    n++;

                    //Update 10^n for the next n'th
                    tenToN = (Int64)Math.Pow(10, (double)n);
                }

                //After fist iteration, go to the next power of 2
                value *= multiplicator; //Calculate 2^exponent

                //Truncate the most significant digit of value to avoid overflow, while retaining all trailing digits
                if (value > truncateLimit)
                {
                    var prevValue = value;
                    value -= truncateLimit;
                }

            }

        }
    }

}