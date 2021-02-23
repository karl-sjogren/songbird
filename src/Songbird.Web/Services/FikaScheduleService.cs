using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Songbird.Web.Contracts;
using Songbird.Web.Extensions;
using Songbird.Web.Models;
using Songbird.Web.Options;

namespace Songbird.Web.Services {
    public class FikaScheduleService : IFikaScheduleService {
        private readonly SongbirdContext _songbirdContext;
        private readonly HttpClient _httpClient;
        private readonly FikaBuddiesOptions _options;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<FikaScheduleService> _logger;

        public FikaScheduleService(SongbirdContext songbirdContext, HttpClient httpClient, IOptionsSnapshot<FikaBuddiesOptions> optionsSnapshot, IDateTimeProvider dateTimeProvider, ILogger<FikaScheduleService> logger) {
            _songbirdContext = songbirdContext;
            _httpClient = httpClient;
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

            var slackMessage = new {
                blocks = new[] {
                    new {
                        type = "section",
                        text = new {
                            type = "mrkdwn",
                            text = _options.SlackMessage
                        },
                        accessory = new {
                            type = "button",
                            text = new {
                                type = "plain_text",
                                text = _options.SlackButtonText,
                                emoji = true
                            },
                            value = "button_value",
                            url = _options.SlackButtonUrl,
                            action_id = "button-action"
                        }
                    }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _options.SlackWebhook) {
                Content = new StringContent(JsonConvert.SerializeObject(slackMessage), Encoding.UTF8, "text/json")
            };

            var response = await _httpClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();
        }
    }
}

/*

{
	"blocks": [
		{
			"type": "section",
			"text": {
				"type": "mrkdwn",
				"text": "Nytt fikaschema finns ute nu!"
			},
			"accessory": {
				"type": "button",
				"text": {
					"type": "plain_text",
					"text": "Visa schema",
					"emoji": true
				},
				"value": "click_me_123",
				"url": "https://google.com",
				"action_id": "button-action"
			}
		}
	]
}
*/
