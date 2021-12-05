using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.DataObjects;
using Songbird.Web.Queries;

namespace Songbird.Web.Controllers {
    [ApiController]
    [Route("api/log-search")]
    public class LogSearchController : ControllerBase {
        private readonly ILogSearchService _service;
        private readonly ILogger<LogSearchController> _logger;

        public LogSearchController(ILogSearchService service, ILogger<LogSearchController> logger) {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<PagedResult<Log4StashEntry>> GetResultAsync([FromQuery] LogSearchQuery searchQuery, [FromQuery] LogSearchFilter filter, [FromQuery] LogSearchSettings settings, CancellationToken cancellationToken) {
            return await _service.GetResultsAsync(searchQuery, filter, settings, cancellationToken);
        }

        [HttpGet("facets")]
        public async Task<ICollection<Facet>> GetFacetsAsync([FromQuery] LogSearchQuery searchQuery, [FromQuery] LogSearchFilter filter, [FromQuery] LogSearchSettings settings, CancellationToken cancellationToken) {
            return await _service.GetFacetsAsync(searchQuery, filter, settings, cancellationToken);
        }
    }
}
