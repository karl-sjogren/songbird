using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Songbird.Web.Contracts;
using Songbird.Web.Extensions;
using Songbird.Web.Models;
using Songbird.Web.Options;

namespace Songbird.Web.Services;

public class FikaScheduleService : IFikaScheduleService {
    private readonly SongbirdContext _songbirdContext;
    private readonly ISlackMessagingService _slackMessagingService;
    private readonly IRandomNumberGenerator _randomNumberGenerator;
    private readonly FikaBuddiesOptions _options;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<FikaScheduleService> _logger;

    public FikaScheduleService(SongbirdContext songbirdContext, ISlackMessagingService slackMessagingService, IOptionsSnapshot<FikaBuddiesOptions> optionsSnapshot, IRandomNumberGenerator randomNumberGenerator, IDateTimeProvider dateTimeProvider, ILogger<FikaScheduleService> logger) {
        _songbirdContext = songbirdContext;
        _slackMessagingService = slackMessagingService;
        _randomNumberGenerator = randomNumberGenerator;
        _options = optionsSnapshot.Value;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<FikaSchedule> GenerateFikaScheduleForDateAsync(DateTime startDate, CancellationToken cancellationToken) {
        startDate = startDate.StartOfWeek();

        var schedule = await _songbirdContext
            .FikaSchedules
            .AsNoTracking()
            .Include(x => x.Matches).ThenInclude(x => x.Users)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.StartDate == startDate, cancellationToken);

        if(schedule != null) {
            _logger.LogWarning($"Can't generate a new fika schedule for date {startDate:yyyy-MM-dd} since it already exists.");
            return null;
        }

        schedule = new FikaSchedule {
            StartDate = startDate
        };
        schedule.CreatedAt = schedule.UpdatedAt = _dateTimeProvider.Now;

        var users = await _songbirdContext
            .Users
            .Where(x => !x.IsDeleted && x.IsEligibleForFikaScheduling)
            .ToListAsync(cancellationToken);

        if(users.None()) {
            _logger.LogWarning($"Can't generate a new fika schedule for date {startDate:yyyy-MM-dd} because there are no users available.");
            return null;
        }

        var lastWeekDate = startDate.AddDays(-7);
        var lastWeekSchedule = await _songbirdContext
            .FikaSchedules
            .AsNoTracking()
            .Include(x => x.Matches).ThenInclude(x => x.Users)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.StartDate == lastWeekDate, cancellationToken);

        do {
            schedule.Matches = GenerateMatches(users);
        } while(CheckIfTooSimilar(schedule, lastWeekSchedule));

        await _songbirdContext.FikaSchedules.AddAsync(schedule, cancellationToken);
        await _songbirdContext.SaveChangesAsync(cancellationToken);

        return schedule;
    }

    private static bool CheckIfTooSimilar(FikaSchedule schedule, FikaSchedule lastWeekSchedule) {
        if(lastWeekSchedule == null)
            return false;

        foreach(var match in schedule.Matches) {
            var users = match.Users;
            foreach(var user in users) {
                var lastWeekMatch = lastWeekSchedule.Matches.FirstOrDefault(m => m.Users.Any(u => u.Id == user.Id));
                if(lastWeekMatch == null)
                    continue;

                var lastWeekUserIds = lastWeekMatch.Users.Where(x => x.Id != user.Id).Select(x => x.Id);
                if(lastWeekUserIds.Any(userId => users.Any(x => x.Id == userId)))
                    return true;
            }
        }

        return false;
    }

    private List<FikaMatch> GenerateMatches(List<User> users) {
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

        return matches;
    }

    private void Shuffle<T>(IList<T> list) {
        var listLength = list.Count;
        for(var current = 0; current < (listLength - 1); current++) {
            var randomINdex = current + _randomNumberGenerator.Next(listLength - current);
            var item = list[randomINdex];
            list[randomINdex] = list[current];
            list[current] = item;
        }
    }

    public async Task<FikaSchedule> GetCurrentFikaScheduleAsync(CancellationToken cancellationToken) {
        var startDate = _dateTimeProvider.Now.Date.StartOfWeek();

        return await _songbirdContext
            .FikaSchedules
            .AsNoTracking()
            .Include(x => x.Matches).ThenInclude(x => x.Users)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.StartDate == startDate, cancellationToken);
    }

    public async Task<ICollection<FikaSchedule>> GetLatestFikaSchedulesAsync(Int32 numberOfSchedules, CancellationToken cancellationToken) {
        return await _songbirdContext
            .FikaSchedules
            .AsNoTracking()
            .Include(x => x.Matches).ThenInclude(x => x.Users)
            .AsSplitQuery()
            .OrderByDescending(x => x.StartDate)
            .Take(numberOfSchedules)
            .ToListAsync(cancellationToken);
    }

    public async Task NotifySlackAboutLatestScheduleAsync(CancellationToken cancellationToken) {
        if(string.IsNullOrWhiteSpace(_options.SlackWebhook)) {
            return;
        }

        var startDate = _dateTimeProvider.Now.Date.StartOfWeek();

        var schedule = await _songbirdContext
            .FikaSchedules
            .AsNoTracking()
            .Include(x => x.Matches).ThenInclude(x => x.Users)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.StartDate == startDate, cancellationToken);

        var slackMessage = new SlackMessageWithLinkButton {
            Webhook = _options.SlackWebhook,
            Message = _options.SlackMessage,
            ButtonText = _options.SlackButtonText,
            ButtonUrl = _options.SlackButtonUrl
        };

        await _slackMessagingService.SendMessageAsync(slackMessage, cancellationToken);
    }
}
