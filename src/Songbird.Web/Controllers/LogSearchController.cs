using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.DataObjects;
using Songbird.Web.Models;
using Songbird.Web.Queries;

namespace Songbird.Web.Controllers {
    [ApiController]
    [Route("api/log-search")]
    public class LogSearchController : ControllerBase {
        private readonly ILogSearchService _service;
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;
        private readonly ILogger<LogSearchController> _logger;

        public LogSearchController(ILogSearchService service, IApplicationService applicationService, IMapper mapper, ILogger<LogSearchController> logger) {
            _service = service;
            _applicationService = applicationService;
            _mapper = mapper;
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

        [HttpGet("facets/application")]
        public async Task<ICollection<LogSearchApplicationFilterDTO>> GetApplicationFacetsAsync(CancellationToken cancellationToken) {
            var result = await _applicationService.GetPagedAsync(0, Int32.MaxValue, cancellationToken);

            var filters = result.Items.SelectMany(x => x.LogFilters);
            return _mapper.Map<IEnumerable<ApplicationLogFilter>, ICollection<LogSearchApplicationFilterDTO>>(filters);
        }
    }
}
