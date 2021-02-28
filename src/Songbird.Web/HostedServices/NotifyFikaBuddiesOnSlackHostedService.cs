using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;

namespace Songbird.Web.HostedServices {
    public class NotifyFikaBuddiesOnSlackHostedService : DailyHostedService {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<CalculateFikaBuddiesHostedService> _logger;

        public NotifyFikaBuddiesOnSlackHostedService(IServiceScopeFactory serviceScopeFactory, IDateTimeProvider dateTimeProvider, ILogger<CalculateFikaBuddiesHostedService> logger)
            : base(TimeSpan.Parse("09:00:00"), dateTimeProvider, logger) {
            _serviceScopeFactory = serviceScopeFactory;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
            using var scope = _serviceScopeFactory.CreateScope();

            var date = _dateTimeProvider.Now.Date;
            if(date.DayOfWeek != DayOfWeek.Monday) {
                return;
            }

            _logger.LogInformation("Reminding people of fika schedule.");
            var fikaService = scope.ServiceProvider.GetRequiredService<IFikaScheduleService>();
            await fikaService.NotifySlackAboutLatestScheduleAsync(CancellationToken.None);
        }
    }
}
