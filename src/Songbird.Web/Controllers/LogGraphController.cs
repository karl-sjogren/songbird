using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.DataObjects;
using Songbird.Web.Queries;

namespace Songbird.Web.Controllers {
    [ApiController]
    [Route("api/log-graph")]
    public class LogGraphController : ControllerBase {
        private readonly ILogGraphService _service;
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;
        private readonly ILogger<LogSearchController> _logger;

        public LogGraphController(ILogGraphService service, IApplicationService applicationService, IMapper mapper, ILogger<LogSearchController> logger) {
            _service = service;
            _applicationService = applicationService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<LogGraphEntry>>> GetResultAsync([FromQuery] LogGraphFilter filter, CancellationToken cancellationToken) {
            if(filter.DateRange.IsEmpty) {
                return BadRequest();
            }

            var result = await _service.GetResultsAsync(filter, cancellationToken);
            return Ok(result);
        }
    }
}
