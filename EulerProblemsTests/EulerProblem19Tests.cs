using EulerProblems.Problems.Problems10to19;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EulerProblemsTests;

[TestClass]
public class EulerProblem19Tests
{
    [DataTestMethod]
    [DataRow(1900, 28)] // A century not divisible by 400.
    [DataRow(1904, 29)]
    [DataRow(2000, 29)] // A century divisible by 400.
    [DataRow(2001, 28)]
    public void FebruaryFollowsTheLeapYearRule(int year, int expectedDays)
        => Assert.AreEqual(expectedDays, DateObject.CalculateNumberOfDaysInFebruary(year));

    [TestMethod]
    public void CalendarMatchesDateTimeForTheWholeTwentiethCentury()
    {
        // 1 Jan 1900 was a Monday, as the problem states.
        var manipulator = new DateManipulator(new Day(EulerProblems.Problems.Problems10to19.DayOfWeek.Monday), new DateObject(1900, Month.January, 1));
        for (var date = new DateTime(1900, 1, 1); date <= new DateTime(2000, 12, 31); date = date.AddDays(1))
        {
            // The solver's enums use the same names as DateTime's months and days of the week.
            string expected = $"{date.Year}.{(Month)date.Month}.{date.Day} ({date.DayOfWeek})";
            Assert.AreEqual(expected, manipulator.ToString());
            manipulator++;
        }
    }
}
