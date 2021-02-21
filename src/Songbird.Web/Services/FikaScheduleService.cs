using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.Extensions;
using Songbird.Web.Models;

namespace Songbird.Web.Services {
    public class FikaScheduleService : IFikaScheduleService {
        private readonly SongbirdContext _songbirdContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<FikaScheduleService> _logger;

        public FikaScheduleService(SongbirdContext songbirdContext, IDateTimeProvider dateTimeProvider, ILogger<FikaScheduleService> logger) {
            _songbirdContext = songbirdContext;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task<FikaSchedule> GenerateFikaScheduleForDateAsync(DateTime startDate, CancellationToken cancellationToken) {
            if(startDate.DayOfWeek != DayOfWeek.Monday) {
                _logger.LogWarning($"Can't generate a new fika schedule for date {startDate:yyyy-MM-dd} because a schedule has to start on a monday.");
                return null;
            }

            var schedule = await _songbirdContext
                .FikaSchedules
                .AsNoTracking()
                .Include(x => x.Matches).ThenInclude(x => x.Users)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.StartDate == startDate, cancellationToken);

            if(schedule != null) {
                _logger.LogWarning($"Can't generate a new fika schedule for date {startDate:yyyy-MM-dd} since it already exists.");
                return schedule;
            }

            schedule = new FikaSchedule {
                StartDate = startDate
            };
            schedule.CreatedAt = schedule.UpdatedAt = _dateTimeProvider.Now;

            var users = await _songbirdContext
                .Users
                .Where(x => !x.IsDeleted)
                .ToListAsync(cancellationToken);

            if(users.None()) {
                _logger.LogWarning($"Can't generate a new fika schedule for date {startDate:yyyy-MM-dd} because there are no users available.");
                return null;
            }

            Shuffle(users);

            var matches = users
                .GroupBy(x => Math.Floor(users.IndexOf(x) / 2d))
                .Select(grouping => {
                    var match = new FikaMatch();
                    match.CreatedAt = match.UpdatedAt = _dateTimeProvider.Now;
                    foreach(var user in grouping) {
                        match.Users.Add(user);
                    }
                    return match;
                })
                .ToList();

            var lastMatch = matches.Last();
            if(lastMatch.Users.Count < 2) {
                matches.Remove(lastMatch);
                matches.Last().Users.Add(lastMatch.Users.First());
            }

            schedule.Matches = matches;
            await _songbirdContext.FikaSchedules.AddAsync(schedule, cancellationToken);
            await _songbirdContext.SaveChangesAsync(cancellationToken);

            return schedule;
        }

        private static void Shuffle<T>(IList<T> list) {
            var count = list.Count;
            var random = new Random();

            while(count > 1) {
                count--;
                var randomIndex = random.Next(count + 1);
                var value = list[randomIndex];
                list[randomIndex] = list[count];
                list[count] = value;
            }
        }

        public async Task<FikaSchedule> GetCurrentFikaScheduleAsync(CancellationToken cancellationToken) {
            var startDate = _dateTimeProvider.Now.Date;

            if(startDate.DayOfWeek != DayOfWeek.Monday) {
                var diff = DayOfWeek.Monday - startDate.DayOfWeek;
                startDate = startDate.AddDays(diff - 7).Date;
            }

            return await _songbirdContext
                .FikaSchedules
                .AsNoTracking()
                .Include(x => x.Matches).ThenInclude(x => x.Users)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.StartDate == startDate, cancellationToken);
        }
    }
}
