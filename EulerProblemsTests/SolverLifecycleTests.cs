using EulerProblems;
using EulerProblems.Problems.Problems00to09;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EulerProblemsTests;

[TestClass]
public class SolverLifecycleTests
{
    [TestMethod]
    public async Task LegacySolverStillPublishesFinalAnswer()
    {
        var solver = new EulerProblem1();
        string answer = "";
        solver.AnswerAvailableEventHandler += (_, e) => answer = e.Answer;
        await Task.Run(solver.StartSolving);
        StringAssert.Contains(answer, "233168");
        StringAssert.Contains(answer, "Elapsed computation time:");
    }

    [TestMethod]
    public async Task LegacyProgressAndFailureReachCaller()
    {
        var solver = new FailingSolver();
        string progress = "";
        solver.AnswerAvailableEventHandler += (_, e) => progress = e.Answer;
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => Task.Run(solver.StartSolving));
        StringAssert.Contains(progress, "test progress");
    }

    private sealed class FailingSolver : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            UpdateProgress("test progress");
            throw new InvalidOperationException("test failure");
        }
    }
}
