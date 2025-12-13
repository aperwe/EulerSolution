using System.Numerics;

namespace EulerProblems.Problems.AdditionalProblems
{
    /// <summary/>
    [ProblemSolver("2 to 10 to nth power pattern", "Powers of 2^10^n",
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
            //Test BigInteger operations for speed
            //SolveBigInteger(out answer);
        }
        protected void SolveInt64(out string answer)
        {
            Int64 n = 0; //Starting index of the exponent of 2^10^n => so 2^10^0 = 2^1 = 2
            Int64 value = 1; //Starting value = 2^1 = 2
            //Calculate starting value of 10^n = 10^0 = 1
            Int64 tenToN = (Int64)Math.Pow(10, (double)n);

            //Number of least-significant digits to keep
            int digitCount = 13;
            Int64 modulus = (Int64)Math.Pow(10.0, (double)digitCount);

            answer = $"Starting Int64{Environment.NewLine}"; UpdateProgress(answer);

            //Limits
            var truncateLimit = Int64.MaxValue / 2;
            //We need to establish a 10-base value, whose most significant digit is the same as truncateLimit, and the rest are zeros
            var truncateLimitStr = truncateLimit.ToString();
            truncateLimitStr = truncateLimitStr[0] + new string('0', truncateLimitStr.Length - 1);
            truncateLimit = Int64.Parse(truncateLimitStr);
            var lengthToKeep = truncateLimitStr.Length - 1;
            Int64 multiplicator = 2;
            Int32 speedFactor = 10;
            Int64 effectiveMultiplicator = multiplicator;
            //After fist iteration, go to the next power of 2
            foreach (Int64 x in Enumerable.Range(1, speedFactor - 1))
            {
                effectiveMultiplicator *= multiplicator; //Calculate 2^exponent * speed multiplier
            }

            for (Int64 exponent = 0; ; exponent += 1 * speedFactor) //Twice as fast
            {
                //Check if exponent is a power of 10
                if (exponent >= tenToN)
                {

                    answer += $"2^10^{n} = 2^{exponent} = {value.ToString()}.{Environment.NewLine}";
                    UpdateProgress(answer);
                    n++;

                    //Update 10^n for the next n'th
                    tenToN = (Int64)Math.Pow(10, (double)n);
                }

                value *= effectiveMultiplicator;

                //Truncate the most significant digit of value to avoid overflow, while retaining all trailing digits
                value %= modulus;

                //Alternative implementation of digit truncation
                //if (value > truncateLimit)
                //{
                //    var prevValue = value;
                //    value -= truncateLimit;
                //}

            }

        }
        protected void SolveInt128(out string answer)
        {
            int n = 0; //Starting index of the exponent of 2^10^n => so 2^10^0 = 2^1 = 2
            Int128 value = 2; //Starting value = 2^1 = 2
            //Calculate starting value of 10^n = 10^0 = 1
            Int64 tenToN = (Int64)Math.Pow(10, (double)n);

            answer = $"Starting Int128{Environment.NewLine}"; UpdateProgress(answer);

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
        protected void SolveBigInteger(out string answer)
        {
            int initialN = 0;
            BigInteger currentValue = BigInteger.One; //Starting value = 2^0 = 1
            //Number of least-significant digits to keep
            int digitCount = 40;
            BigInteger modulus = BigInteger.Pow(10, digitCount);

            answer = $"Starting BigInteger {Environment.NewLine}"; UpdateProgress(answer);

            //Limits: With BigInteger we can go as high as we want, there are no limits

            for (int theN = initialN; ; theN++)
            {
                //Calculate next value of 10^n = 10^0 = 1
                int tenToN = ((int)BigInteger.Pow(10, theN));

                currentValue = BigInteger.One << tenToN; //Calculate 2^10^n 
                var lastDigitsWord = currentValue % modulus;

                var lastWordAsArray = lastDigitsWord.ToString().TakeLast(40).ToArray();
                //Convert dupa to string
                var lastWordAsString = new string(lastWordAsArray);
                answer += $"2^10^{theN} = 2^{tenToN} = {lastWordAsString}.{Environment.NewLine}";
                UpdateProgress(answer);
            }

        }
    }

}