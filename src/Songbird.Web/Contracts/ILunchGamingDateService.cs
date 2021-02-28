using System;
using System.Threading;
using System.Threading.Tasks;
using Songbird.Web.Models;

namespace Songbird.Web.Contracts {
    public interface ILunchGamingDateService : IEditableEntityServiceBase<LunchGamingDate> {
        Task<LunchGamingDate> GetUpcomingAsync(CancellationToken cancellationToken);
        Task<LunchGamingDate> AddAttendeeAsync(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<LunchGamingDate> RemoveAttendeeAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    }
}
