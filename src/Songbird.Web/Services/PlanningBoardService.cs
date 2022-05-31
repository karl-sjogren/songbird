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
            StartDate = startDate,
            WeekNumber = startDate.GetWeekNumber(),
            Year = (short)startDate.Year
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
            .FirstOrDefaultAsync(x => x.StartDate == lastWeekDate && !x.IsDeleted, cancellationToken);

        PopulateNewPlanningBoard(board, users, lastWeekBoard);

        await _songbirdContext.PlanningBoards.AddAsync(board, cancellationToken);
        await _songbirdContext.SaveChangesAsync(cancellationToken);

        return board;
    }

    private void PopulateNewPlanningBoard(PlanningBoard board, ICollection<User> users, PlanningBoard lastWeekBoard) {
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
                            Schedule = schedule,
                            CreatedAt = _dateTimeProvider.Now
                        })
                        .ToList();
                }
            }

            board.UserSchedules.Add(schedule);
        }
    }

    public async Task<PlanningBoard> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
        return await _songbirdContext
            .PlanningBoards
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.UserSchedules).ThenInclude(x => x.Projects).ThenInclude(x => x.Project).ThenInclude(x => x.Customer)
            .Include(x => x.UserSchedules).ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<PlanningBoard> GetByDateAsync(DateTime startDate, CancellationToken cancellationToken) {
        return await _songbirdContext
            .PlanningBoards
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.UserSchedules).ThenInclude(x => x.Projects).ThenInclude(x => x.Project).ThenInclude(x => x.Customer)
            .Include(x => x.UserSchedules).ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.StartDate == startDate && !x.IsDeleted, cancellationToken);
    }

    public async Task<PlanningBoard> GetCurrentPlanningBoardAsync(CancellationToken cancellationToken) {
        var startDate = _dateTimeProvider.Now.Date.StartOfWeek();

        return await _songbirdContext
            .PlanningBoards
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.UserSchedules).ThenInclude(x => x.Projects).ThenInclude(x => x.Project).ThenInclude(x => x.Customer)
            .Include(x => x.UserSchedules).ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.StartDate == startDate && !x.IsDeleted, cancellationToken);
    }

    public async Task<PlannedProjectTime> SetUserProjectTimeAsync(Guid planningBoardId, Guid userId, Guid projectId, double hours, CancellationToken cancellationToken) {
        var planningBoard = await _songbirdContext
            .PlanningBoards
            .AsSplitQuery()
            .Include(x => x.UserSchedules).ThenInclude(x => x.Projects)
            .FirstOrDefaultAsync(x => x.Id == planningBoardId && !x.IsDeleted, cancellationToken);

        if(planningBoard == null) {
            _logger.LogInformation("Can't set user project time for user {userId} on planning board {planningBoardId} because the planning board doesn't exist.", userId, planningBoardId);
            return null;
        }

        var userIsEligible = await _songbirdContext
            .Users
            .Where(x => x.Id == userId && !x.IsDeleted && x.IsEligibleForWeeklyPlaning)
            .AnyAsync(cancellationToken);

        if(!userIsEligible) {
            _logger.LogInformation("Can't set user project time for user {userId} on planning board {planningBoardId} because the user is not eligible for weekly planing.", userId, planningBoardId);
            return null;
        }

        var userSchedule = planningBoard.UserSchedules.FirstOrDefault(x => x.UserId == userId);
        if(userSchedule == null) {
            userSchedule = new UserSchedule {
                UserId = userId,
                PlanningBoard = planningBoard
            };
            userSchedule.CreatedAt = userSchedule.UpdatedAt = _dateTimeProvider.Now;
            planningBoard.UserSchedules.Add(userSchedule);
        }

        var project = userSchedule.Projects.FirstOrDefault(x => x.ProjectId == projectId);
        if(project == null) {
            project = new PlannedProjectTime {
                ProjectId = projectId,
                Schedule = userSchedule
            };
            project.CreatedAt = project.UpdatedAt = _dateTimeProvider.Now;
            userSchedule.Projects.Add(project);
        }

        project.Time = hours;

        if(_songbirdContext.Entry(planningBoard).State == EntityState.Modified) {
            planningBoard.UpdatedAt = userSchedule.UpdatedAt = project.UpdatedAt = _dateTimeProvider.Now;
        }

        await _songbirdContext.SaveChangesAsync(cancellationToken);

        return project;
    }

    public async Task ClearUserProjectTimeAsync(Guid planningBoardId, Guid userId, Guid projectId, CancellationToken cancellationToken) {
        var planningBoard = await _songbirdContext
            .PlanningBoards
            .AsSplitQuery()
            .Include(x => x.UserSchedules).ThenInclude(x => x.Projects)
            .FirstOrDefaultAsync(x => x.Id == planningBoardId && !x.IsDeleted, cancellationToken);

        if(planningBoard == null) {
            _logger.LogInformation("Can't clear user project time for user {userId} on planning board {planningBoardId} because the planning board doesn't exist.", userId, planningBoardId);
            return;
        }

        var userSchedule = planningBoard.UserSchedules.FirstOrDefault(x => x.UserId == userId);
        if(userSchedule == null) {
            return;
        }

        var project = userSchedule.Projects.FirstOrDefault(x => x.ProjectId == projectId);
        if(project == null) {
            return;
        }

        _songbirdContext.Remove(project);

        planningBoard.UpdatedAt = userSchedule.UpdatedAt = _dateTimeProvider.Now;

        await _songbirdContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ICollection<Project>> GetEligibleProjectsAsync(CancellationToken cancellationToken) {
        return await _songbirdContext
            .Projects
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Customer)
            .Where(x => !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
