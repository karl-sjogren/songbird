using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Songbird.Web.DataObjects;
using Songbird.Web.Queries;

namespace Songbird.Web.Contracts;

public interface ILogGraphService {
    Task<IReadOnlyCollection<LogGraphEntry>> GetResultsAsync(LogGraphFilter filter, CancellationToken cancellationToken);
}
