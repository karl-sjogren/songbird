using System;
using System.Threading;
using System.Threading.Tasks;
using Songbird.Web.Models;

namespace Songbird.Web.Contracts;

public interface IPlanningBoardService {
    Task<PlanningBoard> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PlanningBoard> GetCurrentPlanningBoardAsync(CancellationToken cancellationToken);
    Task<PlanningBoard> GeneratePlanningBoardDateAsync(DateTime startDate, CancellationToken cancellationToken);
    Task SetUserProjectTimeAsync(Guid planningBoardId, Guid userId, Guid projectId, double hours, CancellationToken cancellationToken);
    Task ClearUserProjectTimeAsync(Guid planningBoardId, Guid userId, Guid projectId, CancellationToken cancellationToken);
}
