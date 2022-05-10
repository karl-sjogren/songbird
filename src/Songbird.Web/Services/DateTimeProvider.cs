using System;
using Songbird.Web.Contracts;

namespace Songbird.Web.Services;

public class DateTimeProvider : IDateTimeProvider {
    public DateTime Now => DateTime.Now;
}
