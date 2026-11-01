using EulerProblems.Problems.Problems20to29;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QBits.Intuition.Mathematics.Primes;

namespace EulerProblemsTests;

/// <summary>
/// Regression tests for race conditions (#122, #123). A race only shows up on some runs, so each test repeats its work.
/// </summary>
[TestClass]
public class SolverConcurrencyTests
{
    [TestMethod]
    public void PrimeSolverGivesCorrectResultsUnderParallelUse()
    {
        for (int run = 0; run < 20; run++)
        {
            var primeSolver = new PrimeSolver();
            Enumerable.Range(0, 2_000_000).AsParallel().ForAll(number => primeSolver.IsPrime(number));
            Assert.AreEqual(142913828922L, primeSolver.KnownPrimes.Sum());
        }
    }

    [TestMethod]
    public void KnownPrimesIsNotChangedByLaterPrimes()
    {
        var primeSolver = new PrimeSolver();
        primeSolver.IsPrime(2);
        var knownPrimes = primeSolver.KnownPrimes;
        primeSolver.IsPrime(3);
        Assert.AreEqual(1, knownPrimes.Count());
    }

    [TestMethod]
    public void EulerProblem21CountsEachAmicableNumberOnce()
    {
        for (int run = 0; run < 200; run++)
        {
            var solver = new EulerProblem21();
            string answer = "";
            solver.AnswerAvailableEventHandler += (_, e) => answer = e.Answer;
            solver.StartSolving();
            StringAssert.Contains(answer, "under 10000 is 31626.");
        }
    }
}
