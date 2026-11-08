using System.Numerics;
using System.Text;

namespace EulerProblems.Problems.AdditionalProblems
{
    /// <summary/>
    [ProblemSolver("2 to 10 to nth power pattern", "Powers of 2^10^n",
@"Hidden pattern in the powers of 2^10^n
Described here:
https://www.youtube.com/shorts/X4C3Sz-10eA")]
    public class PowersOf2To10ToN : AbstractEulerProblem
    {
        /// <summary>Number of last digits shown on each line.</summary>
        internal const int DigitCount = 100;

        /// <summary>
        /// The last n calculated. From n = DigitCount - 1 on, the last DigitCount digits of 2^10^n no longer change,
        /// so stopping here shows the whole pattern: the last two lines have the same digits.
        /// </summary>
        internal const int LastN = DigitCount;

        /// <summary>Lets a caller stop the solver before it reaches <see cref="LastN"/>.</summary>
        internal CancellationToken StopToken { get; init; }

        protected override void Solve(out string answer)
        {
            BigInteger modulus = BigInteger.Pow(10, DigitCount);
            BigInteger exponent = 1; // 10^n, starting with 10^0
            BigInteger value = 2;    // 2^10^n: the exact number while it is smaller than modulus, then only its last DigitCount digits
            bool isTruncated = false;

            var text = new StringBuilder();
            text.AppendLine($"Last {DigitCount} digits of 2^10^n:");
            for (int n = 0; ; n++)
            {
                StopToken.ThrowIfCancellationRequested();

                // Once the number is truncated, pad it so that leading zeros of the last digits are shown.
                string digits = isTruncated ? value.ToString($"D{DigitCount}") : value.ToString();
                text.AppendLine($"2^10^{n} = 2^{exponent} = {digits}.");
                UpdateProgress(text.ToString());
                if (n == LastN) break;

                // 2^10^(n+1) = (2^10^n)^10, so each line comes from the previous one in a few multiplications.
                exponent *= 10;
                if (isTruncated)
                {
                    // Modular exponentiation: value^10, keeping only the remainder after division by modulus.
                    value = BigInteger.ModPow(value, 10, modulus);
                }
                else
                {
                    value = BigInteger.Pow(value, 10);
                    if (value >= modulus)
                    {
                        value %= modulus;
                        isTruncated = true;
                    }
                }
            }
            answer = text.ToString();
        }
    }

}
