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

namespace Songbird.Web.Services;

public class PlanningBoardService : IPlanningBoardService {
    private readonly SongbirdContext _songbirdContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<PlanningBoardService> _logger;

    public PlanningBoardService(SongbirdContext songbirdContext, IDateTimeProvider dateTimeProvider, ILogger<PlanningBoardService> logger) {
        _songbirdContext = songbirdContext;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<PlanningBoard> GeneratePlanningBoardDateAsync(DateTime startDate, CancellationToken cancellationToken) {
        startDate = startDate.StartOfWeek();
        var lastWeekDate = startDate.AddDays(-7);

        var board = await _songbirdContext
            .PlanningBoards
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.StartDate == startDate, cancellationToken);

        if(board != null) {
            _logger.LogWarning("Can't generate a new planning board for date {startDate} since it already exists.", startDate.ToString("yyyy-MM-dd"));
            return null;
        }

        board = new PlanningBoard {
            StartDate = startDate
        };

        board.CreatedAt = board.UpdatedAt = _dateTimeProvider.Now;

        var users = await _songbirdContext
            .Users
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.IsEligibleForWeeklyPlaning)
            .ToListAsync(cancellationToken);

        if(users.None()) {
            _logger.LogWarning("Can't generate a new planning board for date {startDate} because there are no eligible users available.", startDate.ToString("yyyy-MM-dd"));
            return null;
        }

        var lastWeekBoard = await _songbirdContext
            .PlanningBoards
            .AsNoTracking()
            .Include(x => x.UserSchedules).ThenInclude(x => x.Projects)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.StartDate == lastWeekDate, cancellationToken);

        PopulateNewPlanningBoard(board, users, lastWeekBoard);

        await _songbirdContext.PlanningBoards.AddAsync(board, cancellationToken);
        await _songbirdContext.SaveChangesAsync(cancellationToken);

        return board;
    }

    private static void PopulateNewPlanningBoard(PlanningBoard board, ICollection<User> users, PlanningBoard lastWeekBoard) {
        foreach(var user in users) {
            var schedule = new UserSchedule {
                UserId = user.Id,
                PlanningBoard = board
            };

            if(lastWeekBoard != null) {
                var lastWeekSchedule = lastWeekBoard.UserSchedules.FirstOrDefault(x => x.User.Id == user.Id);
                if(lastWeekSchedule != null) {
                    schedule.Projects = lastWeekSchedule
                        .Projects
                        .Select(x => new PlannedProjectTime {
                            ProjectId = x.ProjectId,
                            Schedule = schedule
                        })
                        .ToList();
                }
            }

            board.UserSchedules.Add(schedule);
        }
    }

    public Task<PlanningBoard> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }

    public Task<PlanningBoard> GetCurrentPlanningBoardAsync(CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }

    public Task SetUserProjectTimeAsync(Guid planningBoardId, Guid userId, Guid projectId, double hours, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }

    public Task ClearUserProjectTimeAsync(Guid planningBoardId, Guid userId, Guid projectId, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
}
