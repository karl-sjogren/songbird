using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;

namespace Songbird.Web.HostedServices {
    public class CalculateFikaBuddiesHostedService : DailyHostedService {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<CalculateFikaBuddiesHostedService> _logger;

        public CalculateFikaBuddiesHostedService(IServiceScopeFactory serviceScopeFactory, IDateTimeProvider dateTimeProvider, ILogger<CalculateFikaBuddiesHostedService> logger)
            : base(TimeSpan.Parse("06:00:00"), dateTimeProvider, logger) {
            _serviceScopeFactory = serviceScopeFactory;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
            using var scope = _serviceScopeFactory.CreateScope();

            // We use the RequestLocalizationMiddleware for this for requests but
            // since this is triggered on a background thread we need to set it here
            // as well.
            CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new("sv-SE");

            var date = _dateTimeProvider.Now.Date;
            if(date.DayOfWeek != DayOfWeek.Monday) {
                return;
            }

            _logger.LogInformation("Generating new fika buddies.");
            var fikaService = scope.ServiceProvider.GetRequiredService<IFikaScheduleService>();
            await fikaService.GenerateFikaScheduleForDateAsync(date, CancellationToken.None);
        }
    }
}
