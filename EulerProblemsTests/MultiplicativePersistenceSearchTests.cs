using EulerProblems.Problems.AdditionalProblems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EulerProblemsTests;

[TestClass]
public class MultiplicativePersistenceSearchTests
{
    [DataTestMethod]
    [DataRow(0UL, 0)]
    [DataRow(9UL, 0)]
    [DataRow(10UL, 1)]
    [DataRow(25UL, 2)]
    [DataRow(39UL, 3)]
    [DataRow(3456UL, 2)]
    [DataRow(277777788888899UL, 11)]
    public void KnownValues(ulong number, int expected)
        => Assert.AreEqual(expected, MultiplicativePersistenceSearch.Calculate(number));

    [DataTestMethod]
    [DataRow(0UL, 10000)]
    [DataRow(277857000000000UL, 1000)]
    [DataRow(18446744073709551515UL, 101)]
    public void BoundedRangeMatchesIndependentReference(ulong start, int count)
    {
        int best = -1;
        ulong bestNumber = start;
        for (int i = 0; i < count; i++)
        {
            ulong number = checked(start + (ulong)i);
            int expected = ReferencePersistence(number);
            Assert.AreEqual(expected, MultiplicativePersistenceSearch.Calculate(number), $"Candidate {number}");
            if (expected > best)
            {
                best = expected;
                bestNumber = number;
            }
        }
        Assert.AreEqual(new PersistenceSearchResult((ulong)count, bestNumber, best),
            MultiplicativePersistenceSearch.Search(start, (ulong)count));
    }

    [TestMethod]
    public void EmptyRangeHasNoBestResult()
        => Assert.AreEqual(new PersistenceSearchResult(0, null, null),
            MultiplicativePersistenceSearch.Search(ulong.MaxValue, 0));

    [TestMethod]
    public void TiesChooseSmallestNumber()
        => Assert.AreEqual(new PersistenceSearchResult(10, 0, 0),
            MultiplicativePersistenceSearch.Search(0, 10));

    [TestMethod]
    public void SingleCandidateAtUpperBoundaryIsIncluded()
        => Assert.AreEqual(new PersistenceSearchResult(1, ulong.MaxValue, ReferencePersistence(ulong.MaxValue)),
            MultiplicativePersistenceSearch.Search(ulong.MaxValue, 1));

    [TestMethod]
    public void RangePastUpperBoundaryIsRejected()
        => Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => MultiplicativePersistenceSearch.Search(ulong.MaxValue, 2));

    // Independent arithmetic implementation: no strings, LINQ, or shortcut products.
    private static int ReferencePersistence(ulong number)
    {
        int steps = 0;
        while (number >= 10)
        {
            ulong product = 1;
            do
            {
                product = checked(product * (number % 10));
                number /= 10;
            } while (number != 0);
            number = product;
            steps++;
        }
        return steps;
    }
}
