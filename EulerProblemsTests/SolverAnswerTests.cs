using EulerProblems;
using EulerProblems.Problems.AdditionalProblems;
using EulerProblems.Problems.Problems00to09;
using EulerProblems.Problems.Problems10to19;
using EulerProblems.Problems.Problems20to29;
using EulerProblems.Problems.Problems30to39;
using EulerProblems.Problems.Problems40to49;
using EulerProblems.Problems.Problems50to59;
using EulerProblems.Problems.Problems60to69;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;

namespace EulerProblemsTests;

/// <summary>
/// Tests every solver, following the testing policy in AGENTS.md.
/// Each expected value includes its surrounding text, because a bare number could also match the elapsed time in the answer.
/// </summary>
[TestClass]
public class SolverAnswerTests
{
    // AGENTS.md requires at least 3 minutes. EulerProblem51 needs more than that in a Debug build.
    private const int SolverTimeLimitMs = 5 * 60 * 1000;
    private static readonly TimeSpan OpenEndedRunTime = TimeSpan.FromMinutes(3);
    private const int OpenEndedTestLimitMs = 4 * 60 * 1000;
    // Marks slow tests for reporting and future optimization. AGENTS.md does not allow leaving them out of a test run.
    private const string SlowCategory = "Slow";

    [DataTestMethod]
    [Timeout(SolverTimeLimitMs)]
    [DataRow(typeof(EulerProblem1), "is: 233168.")]
    [DataRow(typeof(EulerProblem2), "under 1 million is: 4613732.")]
    [DataRow(typeof(EulerProblem3), "600851475143 is: 6857")]
    [DataRow(typeof(EulerProblem4), "3-digit numbers is: 906609.")]
    [DataRow(typeof(EulerProblem6), "Diff = 25164150.")]
    [DataRow(typeof(EulerProblem7), "The answer is: 104743")]
    [DataRow(typeof(EulerProblem8), "= 23514624000")]
    [DataRow(typeof(EulerProblem10), "is equal to 142913828922.")]
    [DataRow(typeof(EulerProblem11), "diagonally) is 70600674.")]
    [DataRow(typeof(EulerProblem12), "Triangle number: 76576500,")]
    [DataRow(typeof(EulerProblem13), "50-digit numbers is: 5537376230")]
    [DataRow(typeof(EulerProblem14), "is: 837799. Chain length: 525.")]
    [DataRow(typeof(EulerProblem15), "exactly 137846528820 routes.")]
    [DataRow(typeof(EulerProblem16), "Sum of digits = 1366.")]
    [DataRow(typeof(EulerProblem17), "would be used is 21124.")]
    [DataRow(typeof(EulerProblem18), "triangle is 1074.")]
    [DataRow(typeof(EulerProblem19), "is 171.")]
    [DataRow(typeof(EulerProblem20), "is equal to 648.")]
    [DataRow(typeof(EulerProblem21), "under 10000 is 31626.")]
    [DataRow(typeof(EulerProblem22), "in the file is 871198282.")]
    [DataRow(typeof(EulerProblem23), "abundant numbers is 4179871.")]
    [DataRow(typeof(EulerProblem24), "Millionth permutation is 2783915460.")]
    [DataRow(typeof(EulerProblem25), "1000 digits is: 4782.")]
    [DataRow(typeof(EulerProblem26), "is: 1/983. Cycle is:")]
    [DataRow(typeof(EulerProblem27), "is -59231.")]
    [DataRow(typeof(EulerProblem28a), "Answer: 669171001,")]
    [DataRow(typeof(EulerProblem28b), "Answer: 669171001,")]
    [DataRow(typeof(EulerProblem29), "There are 9183 distinct terms.")]
    [DataRow(typeof(EulerProblem30), "of their digits is: 443839.")]
    [DataRow(typeof(EulerProblem31), "Number of pockets 73682.")]
    [DataRow(typeof(EulerProblem32), "Sum of unique products: 45228.")]
    [DataRow(typeof(EulerProblem33), "Final denominator is 100.")]
    [DataRow(typeof(EulerProblem34), "Answer is 40730.")]
    [DataRow(typeof(EulerProblem35), "Primes 55.")]
    [DataRow(typeof(EulerProblem36), "Sum = 872187. Count = 19.")]
    [DataRow(typeof(EulerProblem37), "Sum = 748317.")]
    [DataRow(typeof(EulerProblem38), "(9327*2=932718654)")]
    [DataRow(typeof(EulerProblem39), "Solution = P=840")]
    [DataRow(typeof(EulerProblem40), "Result = 210")]
    [DataRow(typeof(EulerProblem41), "7654321: 7652413")]
    [DataRow(typeof(EulerProblem42), "Number of triangle words in file: 162.")]
    [DataRow(typeof(EulerProblem43), "Sum = 16695334890")]
    [DataRow(typeof(EulerProblem45), "; 1533776805")]
    [DataRow(typeof(EulerProblem46), "twice a square is: 5777.")]
    [DataRow(typeof(EulerProblem47), "Found: (134043).")]
    [DataRow(typeof(EulerProblem48), "Solution: '9110846700'")]
    [DataRow(typeof(EulerProblem49), "- 296962999629,")]
    [DataRow(typeof(EulerProblem50), "under 1 million is: 997651. Number of elements: 543.")]
    [DataRow(typeof(EulerProblem52Gugus), "Found total of 1229 primes.")]
    [DataRow(typeof(EulerProblem67), "triangle is 7273.")]
    public void SolverProducesExpectedAnswer(Type solverType, string expectedAnswerText)
        => AssertSolverAnswer(solverType, expectedAnswerText);

    [DataTestMethod]
    [TestCategory(SlowCategory)]
    [Timeout(SolverTimeLimitMs)]
    [DataRow(typeof(EulerProblem44), "D = 5482660.")]
    // Unfinished (#90): this records today's result. Update it when the problem is solved.
    [DataRow(typeof(EulerProblem51), "Step 3 complete. Result: -1;")]
    public void SlowSolverProducesExpectedAnswer(Type solverType, string expectedAnswerText)
        => AssertSolverAnswer(solverType, expectedAnswerText);

    private static void AssertSolverAnswer(Type solverType, string expectedAnswerText)
    {
        var solver = (AbstractEulerProblem)Activator.CreateInstance(solverType)!;
        string answer = "";
        solver.AnswerAvailableEventHandler += (_, e) => answer = e.Answer;
        solver.StartSolving();
        StringAssert.Contains(answer, expectedAnswerText);
    }

    // The digits shown for n = 0 to 11 before #153, which kept the last 13 digits and reached n = 11 in 5 minutes.
    // The value for n = 7 lacked its leading zero, so the test compares them as numbers.
    private static readonly string[] PowersOf2To10ToNDigitsBefore153 =
    {
        "2", "1024", "1496703205376", "7205668069376", "4792596709376", "4389883109376",
        "3162747109376", "891387109376", "8177787109376", "1041787109376", "9681787109376", "6081787109376",
    };

    /// <summary>This solver finishes on its own after n = LastN, so the test checks its final answer.</summary>
    [TestMethod]
    [Timeout(SolverTimeLimitMs)]
    public void PowersOf2To10ToNComputesEveryLineQuickly()
    {
        var solver = new PowersOf2To10ToN();
        string answer = "";
        solver.AnswerAvailableEventHandler += (_, e) => answer = e.Answer;

        var stopwatch = Stopwatch.StartNew();
        solver.StartSolving();
        stopwatch.Stop();
        Console.WriteLine($"PowersOf2To10ToN calculated n = 0 to {PowersOf2To10ToN.LastN} in {stopwatch.Elapsed}.");
        // Measures speed without stopping the solver. Each line costs about the same, so a run to n = 100 takes milliseconds.
        Assert.IsTrue(stopwatch.Elapsed < TimeSpan.FromSeconds(10), $"The run took {stopwatch.Elapsed}.");

        var lines = answer.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .SkipWhile(line => line != $"Last {PowersOf2To10ToN.DigitCount} digits of 2^10^n:").Skip(1).ToList();
        Assert.AreEqual(PowersOf2To10ToN.LastN + 1, lines.Count);

        // Every line must have the next n, an exponent of exactly 10^n, and the last digits of 2 to that exponent.
        // A number shorter than DigitCount digits is shown whole. A longer one shows exactly DigitCount digits, with leading zeros.
        var modulus = BigInteger.Pow(10, PowersOf2To10ToN.DigitCount);
        for (int n = 0; n < lines.Count; n++)
        {
            var exponent = BigInteger.Pow(10, n);
            var lastDigits = BigInteger.ModPow(2, exponent, modulus);
            bool isWhole = exponent < 1000 && BigInteger.Pow(2, (int)exponent) < modulus;
            string expectedDigits = isWhole ? lastDigits.ToString() : lastDigits.ToString().PadLeft(PowersOf2To10ToN.DigitCount, '0');
            Assert.AreEqual($"2^10^{n} = 2^{exponent} = {expectedDigits}.", lines[n]);
        }

        for (int n = 0; n < PowersOf2To10ToNDigitsBefore153.Length; n++)
        {
            string digits = DigitsOf(lines[n]);
            Assert.AreEqual(BigInteger.Parse(PowersOf2To10ToNDigitsBefore153[n]), BigInteger.Parse(digits[^Math.Min(13, digits.Length)..]), $"n = {n}");
        }

        Assert.AreEqual(DigitsOf(lines[^2]), DigitsOf(lines[^1]), "The last digits should have stopped changing.");

        static string DigitsOf(string line) => line[(line.LastIndexOf(" = ") + 3)..].TrimEnd('.');
    }

    /// <summary>Uses a temporary folder, so the run cannot affect the real checkpoints.</summary>
    [TestMethod]
    [TestCategory(SlowCategory)]
    [Timeout(OpenEndedTestLimitMs)]
    public async Task AdditionalProblem001SavesProgressDuringLongRun()
    {
        string directory = Path.Combine(Path.GetTempPath(), "EulerSolverAnswerTests", Guid.NewGuid().ToString("N"));
        try
        {
            using var stop = new CancellationTokenSource(OpenEndedRunTime);
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(
                () => new AdditionalProblem001().RunAsync(stop.Token, directory));
            using var store = new PersistenceCheckpointStore(directory);
            Assert.IsTrue(store.Load()!.Checkpoint.Completed.EvaluatedCount > 0, "The run should have saved completed work.");
        }
        finally
        {
            if (Directory.Exists(directory)) Directory.Delete(directory, true);
        }
    }

    [TestMethod]
    public void EverySolverHasATest()
    {
        var tested = new[] { nameof(SolverProducesExpectedAnswer), nameof(SlowSolverProducesExpectedAnswer) }
            .SelectMany(method => typeof(SolverAnswerTests).GetMethod(method)!.GetCustomAttributes<DataRowAttribute>())
            .Select(row => (Type)row.Data[0]!)
            .Concat(new[] { typeof(PowersOf2To10ToN), typeof(AdditionalProblem001) });
        var solvers = typeof(AbstractEulerProblem).Assembly.GetTypes()
            .Where(type => type.GetCustomAttributes<ProblemSolverAttribute>(false).Any());

        var untested = solvers.Except(tested).Select(type => type.Name).OrderBy(name => name);

        Assert.AreEqual("", string.Join(", ", untested), "Solvers without a test.");
    }
}
