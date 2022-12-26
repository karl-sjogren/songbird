using System.ComponentModel;
using Songbird.Web.Queries;
using Songbird.Web.TypeConverters;

namespace Songbird.Web.Tests.TypeConverters;

public class DateWithRangeTypeConverterTests {
    private readonly DateWithRangeTypeConverter _converter;

    public DateWithRangeTypeConverterTests() {
        _converter = (DateWithRangeTypeConverter)TypeDescriptor.GetConverter(typeof(DateWithRange));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("2020-01-08-2020-01-10")]
    public void ReturnsNullForEmptyOrInvalidValues(string input) {
        var result = _converter.ConvertFromString(input) as DateWithRange;

        Assert.Null(result);
    }

    [Theory]
    [InlineData("2020-01-08")]
    public void ParsesRangeWithSingleValue(string input) {
        var result = _converter.ConvertFromString(input) as DateWithRange;

        Assert.False(result.IsEmpty);
        Assert.True(result.IsSingleDate);
        Assert.Equal(result.MinDate, new DateTime(2020, 1, 8));
        Assert.Equal(result.MaxDate.Date, new DateTime(2020, 1, 8));
    }

    [Theory]
    [InlineData("2020-01-08 - 2020-01-10")]
    public void ParsesExactRange(string input) {
        var result = _converter.ConvertFromString(input) as DateWithRange;

        Assert.False(result.IsEmpty);
        Assert.False(result.IsSingleDate);
        Assert.Equal(result.MinDate, new DateTime(2020, 1, 8));
        Assert.Equal(result.MaxDate.Date, new DateTime(2020, 1, 10));
    }

    [Theory]
    [InlineData("\t2020-01-08 -   2020-01-10")]
    public void ParsesRangeWithUnevenWhitespace(string input) {
        var result = _converter.ConvertFromString(input) as DateWithRange;

        Assert.False(result.IsEmpty);
        Assert.False(result.IsSingleDate);
        Assert.Equal(result.MinDate, new DateTime(2020, 1, 8));
        Assert.Equal(result.MaxDate.Date, new DateTime(2020, 1, 10));
    }
}
