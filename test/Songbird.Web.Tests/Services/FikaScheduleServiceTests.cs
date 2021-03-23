using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Songbird.Web.Contracts;
using Songbird.Web.Models;
using Songbird.Web.Options;
using Songbird.Web.Services;
using Songbird.Web.Tests.Helpers;
using Xunit;

namespace Songbird.Web.Tests.Services {
    public sealed class FikaScheduleServiceTests : IDisposable {
        private readonly SongbirdContext _songbirdContext;
        private readonly ISlackMessagingService _slackMessagingService;
        private readonly IOptionsSnapshot<FikaBuddiesOptions> _optionsSnapshot;
        private readonly MockDateTimeProvider _dateTimeProvider;

        public FikaScheduleServiceTests() {
            var options = new DbContextOptionsBuilder<SongbirdContext>()
                .UseInMemoryDatabase("FikaScheduleServiceTests", new InMemoryDatabaseRoot())
                .Options;

            _songbirdContext = new SongbirdContext(options);
            _slackMessagingService = Mock.Of<ISlackMessagingService>();
            _optionsSnapshot = Mock.Of<IOptionsSnapshot<FikaBuddiesOptions>>();
            _dateTimeProvider = new MockDateTimeProvider(DateTime.Now);

            CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo("sv-SE");
        }

        public void Dispose() => _songbirdContext?.Dispose();

        [Theory]
        [InlineData(new[] { 0d, 0d, 0d, 0d }, new[] { "Sven", "Arne", "Nisse", "Pelle" })]
        [InlineData(new[] { 0d, 1d, 0d, 1d }, new[] { "Sven", "Pelle", "Nisse", "Arne" })]
        [InlineData(new[] { 0.33d, 0.33d, 0.66d, 0.66d }, new[] { "Arne", "Sven", "Pelle", "Nisse" })]
        public async Task ShouldGenerateARandomizedScheduleAsync(double[] randomSequence, string[] expectedOrder) {
            _songbirdContext.Users.AddRange(new User[] {
                new() {
                    Name = "Sven",
                    IsEligibleForFikaScheduling = true
                },
                new() {
                    Name = "Arne",
                    IsEligibleForFikaScheduling = true
                },
                new() {
                    Name = "Nisse",
                    IsEligibleForFikaScheduling = true
                },
                new() {
                    Name = "Pelle",
                    IsEligibleForFikaScheduling = true
                }
            });

            _songbirdContext.SaveChanges();

            var randomGenerator = new FixedOrderRandomGenerator(randomSequence);

            var service = new FikaScheduleService(_songbirdContext, _slackMessagingService, _optionsSnapshot, randomGenerator, _dateTimeProvider, NullLogger<FikaScheduleService>.Instance);

            var schedule = await service.GenerateFikaScheduleForDateAsync(DateTime.Parse("2021-03-22"), CancellationToken.None);
            schedule.StartDate.ShouldBe(DateTime.Parse("2021-03-22"));

            var matches = schedule.Matches;
            matches.Count.ShouldBe(2);

            var users = matches.SelectMany(x => x.Users).ToArray();

            users[0].Name.ShouldBe(expectedOrder[0]);
            users[1].Name.ShouldBe(expectedOrder[1]);
            users[2].Name.ShouldBe(expectedOrder[2]);
            users[3].Name.ShouldBe(expectedOrder[3]);
        }

        [Fact]
        public async Task ShouldGenerateUniqueScheduleComparedToLastWeekAsync() {
            _songbirdContext.Users.AddRange(new User[] {
                new() {
                    Name = "Sven",
                    IsEligibleForFikaScheduling = true
                },
                new() {
                    Name = "Arne",
                    IsEligibleForFikaScheduling = true
                },
                new() {
                    Name = "Nisse",
                    IsEligibleForFikaScheduling = true
                },
                new() {
                    Name = "Pelle",
                    IsEligibleForFikaScheduling = true
                }
            });

            _songbirdContext.SaveChanges();

            var randomGenerator = new FixedOrderRandomGenerator(new[] { 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 1d, 1d, 1d, 1d }); // Three pairs of four to generate three schedules

            var service = new FikaScheduleService(_songbirdContext, _slackMessagingService, _optionsSnapshot, randomGenerator, _dateTimeProvider, NullLogger<FikaScheduleService>.Instance);

            {
                var schedule = await service.GenerateFikaScheduleForDateAsync(DateTime.Parse("2021-03-22"), CancellationToken.None);
                schedule.StartDate.ShouldBe(DateTime.Parse("2021-03-22"));

                var matches = schedule.Matches;
                matches.Count.ShouldBe(2);

                var users = matches.SelectMany(x => x.Users).ToArray();

                users[0].Name.ShouldBe("Sven");
                users[1].Name.ShouldBe("Arne");
                users[2].Name.ShouldBe("Nisse");
                users[3].Name.ShouldBe("Pelle");
            }

            {
                var schedule = await service.GenerateFikaScheduleForDateAsync(DateTime.Parse("2021-03-29"), CancellationToken.None);
                schedule.StartDate.ShouldBe(DateTime.Parse("2021-03-29"));

                var matches = schedule.Matches;
                matches.Count.ShouldBe(2);

                var users = matches.SelectMany(x => x.Users).ToArray();

                users[0].Name.ShouldBe("Nisse");
                users[1].Name.ShouldBe("Sven");
                users[2].Name.ShouldBe("Arne");
                users[3].Name.ShouldBe("Pelle");
            }
        }
    }
}
