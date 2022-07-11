using System;
using System.Globalization;
using System.Threading;
using Shouldly;
using Songbird.Web.Extensions;
using Xunit;

namespace Songbird.Web.Tests.Extensions;

public class DateTimeExtensionsTests {
    [Theory]
    [InlineData("en-US", "2009-12-27", 52)]
    [InlineData("en-US", "2009-12-28", 53)]
    [InlineData("en-US", "2010-01-03", 53)]
    [InlineData("en-US", "2010-01-04", 1)]

    [InlineData("en-US", "2014-12-31", 1)]
    [InlineData("en-US", "2015-01-01", 1)]

    [InlineData("fi-FI", "2009-12-27", 52)]
    [InlineData("fi-FI", "2009-12-28", 53)]
    [InlineData("fi-FI", "2010-01-03", 53)]
    [InlineData("fi-FI", "2010-01-04", 1)]

    [InlineData("fi-FI", "2014-12-31", 1)]
    [InlineData("fi-FI", "2015-01-01", 1)]

    [InlineData("sv-SE", "2009-12-27", 52)]
    [InlineData("sv-SE", "2009-12-28", 53)]
    [InlineData("sv-SE", "2010-01-03", 53)]
    [InlineData("sv-SE", "2010-01-04", 1)]

    [InlineData("sv-SE", "2013-12-29", 52)]
    [InlineData("sv-SE", "2013-12-30", 1)]
    [InlineData("sv-SE", "2014-01-01", 1)]
    [InlineData("sv-SE", "2014-01-05", 1)]
    [InlineData("sv-SE", "2014-01-06", 2)]

    [InlineData("sv-SE", "2014-12-28", 52)]
    [InlineData("sv-SE", "2014-12-29", 1)]
    [InlineData("sv-SE", "2014-12-31", 1)]
    [InlineData("sv-SE", "2015-01-01", 1)]
    [InlineData("sv-SE", "2015-01-04", 1)]
    [InlineData("sv-SE", "2015-01-05", 2)]
    public void TestWeekNumbers(string culture, DateTime date, int weekNumber) {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

        date.GetWeekNumber().ShouldBe(weekNumber);
    }

    [Theory]
    [InlineData("en-US", "2014-12-28", "2014-12-28")]
    [InlineData("en-US", "2014-12-31", "2014-12-28")]
    [InlineData("en-US", "2015-01-03", "2014-12-28")]
    [InlineData("en-US", "2015-01-04", "2015-01-04")]

    [InlineData("sv-SE", "2014-12-28", "2014-12-22")]
    [InlineData("sv-SE", "2014-12-31", "2014-12-29")]
    [InlineData("sv-SE", "2015-01-03", "2014-12-29")]
    [InlineData("sv-SE", "2015-01-04", "2014-12-29")]
    [InlineData("sv-SE", "2015-01-05", "2015-01-05")]
    public void TestGetWeekStart(string culture, DateTime date, DateTime expectedStart) {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

        date.StartOfWeek().ShouldBe(expectedStart);
    }

    [Theory]
    [InlineData("en-US", 2009, 53, "2009-12-27")]
    [InlineData("en-US", 2015, 1, "2014-12-28")]
    [InlineData("en-US", 2015, 2, "2015-01-04")]

    [InlineData("sv-SE", 2009, 53, "2009-12-28")]
    [InlineData("sv-SE", 2015, 1, "2014-12-29")]
    [InlineData("sv-SE", 2015, 2, "2015-01-05")]

    [InlineData("sv-SE", 2014, 58, "2015-02-02")]
    [InlineData("en-US", 2014, 58, "2015-02-01")]
    public void TestGetDateFromWeek(string culture, int year, int week, DateTime expectedStart) {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

        DateTimeExtensions.GetStartOfWeek(year, week).ShouldBe(expectedStart);
    }
}
