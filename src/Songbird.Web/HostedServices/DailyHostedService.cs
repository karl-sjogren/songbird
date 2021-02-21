using System;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;

namespace Songbird.Web.HostedServices {
    public abstract class DailyHostedService : TimedHostedService {
        protected DailyHostedService(TimeSpan timeOfDay, IDateTimeProvider dateTimeProvider, ILogger<DailyHostedService> logger)
            : base(TimeSpan.FromDays(1), CalculateWaitBeforeStart(dateTimeProvider, timeOfDay), logger) {
        }

        internal static TimeSpan CalculateWaitBeforeStart(IDateTimeProvider dateTimeProvider, TimeSpan timeOfDay) {
            var now = dateTimeProvider.Now;
            if(timeOfDay >= now.TimeOfDay)
                return timeOfDay - now.TimeOfDay;

            return (timeOfDay - now.TimeOfDay).Add(TimeSpan.FromDays(1));
        }
    }
}
