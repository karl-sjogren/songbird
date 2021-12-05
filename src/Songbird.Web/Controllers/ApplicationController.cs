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

namespace Songbird.Web.Controllers {
    [ApiController]
    [Route("api/application")]
    public class ApplicationController : EditableEntityControllerBase<Application> {
        private readonly IApplicationService _service;
        private readonly IMapper _mapper;

        public ApplicationController(IApplicationService service, IMapper mapper, ILogger<ApplicationController> logger)
            : base(service, logger) {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("log-filters")]
        public async Task<ICollection<LogSearchApplicationFilterDTO>> GetLogFiltersAsync(CancellationToken cancellationToken) {
            var result = await _service.GetPagedAsync(0, Int32.MaxValue, cancellationToken);

            var filters = result.Items.SelectMany(x => x.LogFilters);
            return _mapper.Map<IEnumerable<ApplicationLogFilter>, ICollection<LogSearchApplicationFilterDTO>>(filters);
        }
    }
}
