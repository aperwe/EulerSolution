using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems.Problems10to19
{
    /// <summary/>
    [ProblemSolver("Counting Sundays", displayName = "Problem 19", problemDefinition =
@"You are given the following information, but you may prefer to do some research for yourself.

• 1 Jan 1900 was a Monday.
• Thirty days has September,
  April, June and November.
  All the rest have thirty-one,
  Saving February alone,
  Which has twenty-eight, rain or shine.
  And on leap years, twenty-nine.
• A leap year occurs on any year evenly divisible by 4, but not on a century unless it is divisible by 400.

How many Sundays fell on the first of the month during the twentieth century (1 Jan 1901 to 31 Dec 2000)?"
        )]
    public class EulerProblem19 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            Day currentDay = new Day(DayOfWeek.Monday);
            DateObject currentDate = new DateObject(1900, Month.January, 1);
            DateManipulator dateManipulator = new DateManipulator(currentDay, currentDate);

            while (!dateManipulator.Equals(1901, Month.January, 1))
            {
                dateManipulator++;
            }

            int sundaysCount = 0;
            while (!dateManipulator.Equals(2000, Month.December, 31))
            {
                dateManipulator++;
                if (dateManipulator.IsSundayOnFirstDayOfTheMonth())
                {
                    sundaysCount++;
                }
            }

            answer = string.Format("The number of Sundays that fell on the first of the month during the twentieth century (1 Jan 1901 to 31 Dec 2000) is {0}.", sundaysCount);
        }
    }

    class DateManipulator
    {
        public override string ToString()
        {
            return string.Format("{0} ({1})", currentDate.ToString(), dayOfWeek);
        }
        private DateObject currentDate;
        private Day dayOfWeek;

        public DateManipulator(Day dayOfWeek, DateObject date)
        {
            this.dayOfWeek = dayOfWeek;
            this.currentDate = date;
        }

        internal bool Equals(int comparedYear, Month comparedMonth, int comparedDay)
        {
            return currentDate.Equals(comparedYear, comparedMonth, comparedDay);
        }

        internal bool IsSundayOnFirstDayOfTheMonth()
        {
            return (dayOfWeek.currentDay == DayOfWeek.Sunday && currentDate.DayOfMonth == 1);
        }

        public static DateManipulator operator++(DateManipulator manipulator)
        {
            manipulator.dayOfWeek++;
            manipulator.currentDate++;
            return manipulator;
        }
    }

    class DateObject
    {
        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", Year, Month, DayOfMonth);
        }

        int Year;
        Month Month;
        internal int DayOfMonth;
        Dictionary<Month, int> daysInMonth;

        public DateObject(int year, Month month, int dayOfMonth)
        {
            this.Year = year;
            this.Month = month;
            this.DayOfMonth = dayOfMonth;
            ConstructDictionary();
        }

        private void ConstructDictionary()
        {
            daysInMonth = new Dictionary<Month, int>();
            daysInMonth.Add(Month.January, 31);
            daysInMonth.Add(Month.February, 28);
            daysInMonth.Add(Month.March, 31);
            daysInMonth.Add(Month.April, 30);
            daysInMonth.Add(Month.May, 31);
            daysInMonth.Add(Month.June, 30);
            daysInMonth.Add(Month.July, 31);
            daysInMonth.Add(Month.August, 31);
            daysInMonth.Add(Month.September, 30);
            daysInMonth.Add(Month.October, 31);
            daysInMonth.Add(Month.November, 30);
            daysInMonth.Add(Month.December, 31);
        }

        /// <summary>
        /// Adds one day to the date. Turns the month where necessary. Turns the year, where necessary.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateObject operator ++(DateObject date)
        {
            date.AddOneDay();
            return date;
        }

        private void AddOneDay()
        {
            DayOfMonth++;
            int maxDaysInThisMonth = daysInMonth[Month];
            if (Month == Month.February) //Special case for February. Check if it has 28 (default) or 29 days.
            {
                maxDaysInThisMonth = CalculateNumberOfDaysInFebruary();
            }
            if (DayOfMonth > maxDaysInThisMonth) //Turn of the month
            {
                Month++;
                DayOfMonth = 1;
            }
            if (Month > Month.December) //Turn of the year
            {
                Month = Month.January;
                Year++;
            }
        }

        private int CalculateNumberOfDaysInFebruary()
        {
            int februaryDuration = 28; //Starting default
            if (Year % 4 == 0) //Is leap year every fourth year?
            {
                if (Year % 100 != 0) //Except every full century?
                {
                    if (Year % 400 == 0) //Unless divisible by 400
                    {
                        februaryDuration = 29;
                    }

                }
            }

            return februaryDuration;
        }

        internal bool Equals(int comparedYear, Month comparedMonth, int comparedDay)
        {
            return Year == comparedYear && Month == comparedMonth && DayOfMonth == comparedDay;
        }
    }

    class Day
    {
        public override string ToString()
        {
            return currentDay.ToString();
        }
        internal DayOfWeek currentDay { get; private set; }

        public Day(DayOfWeek day)
        {
            currentDay = day;
        }
        public static Day operator ++(Day day)
        {
            day.currentDay++;
            if (day.currentDay > DayOfWeek.Sunday) day.currentDay = DayOfWeek.Monday;
            return day;
        }
    }

    enum DayOfWeek
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    enum Month
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
}
