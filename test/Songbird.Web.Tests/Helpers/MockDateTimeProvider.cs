using System;
using Songbird.Web.Contracts;

namespace Songbird.Web.Tests.Helpers;

public class MockDateTimeProvider : IDateTimeProvider {
    public MockDateTimeProvider(DateTime? now = null) {
        Now = now ?? DateTime.Now;
    }

    public MockDateTimeProvider(string dateTimeValue) {
        Now = DateTime.Parse(dateTimeValue);
    }

    public DateTime Now { get; set; }

    public static implicit operator MockDateTimeProvider(DateTime date) => new(date);
    public static implicit operator MockDateTimeProvider(string date) => new(date);
}
