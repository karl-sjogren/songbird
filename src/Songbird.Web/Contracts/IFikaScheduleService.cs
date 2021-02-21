using System;
using System.Threading;
using System.Threading.Tasks;
using Songbird.Web.Models;

namespace Songbird.Web.Contracts {
    public interface IFikaScheduleService {
        Task<FikaSchedule> GetCurrentFikaScheduleAsync(CancellationToken cancellationToken);
        Task<FikaSchedule> GenerateFikaScheduleForDateAsync(DateTime startDate, CancellationToken cancellationToken);
    }
}
