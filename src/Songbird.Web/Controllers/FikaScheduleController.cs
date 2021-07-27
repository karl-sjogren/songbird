using System;
using System.Collections.Generic;
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
    [Route("api/fika-schedule")]
    public class FikaScheduleController : Controller {
        private readonly IFikaScheduleService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<FikaScheduleController> _logger;

        public FikaScheduleController(IFikaScheduleService service, IMapper mapper, ILogger<FikaScheduleController> logger) {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<FikaScheduleDTO>> GetCurrentFikaScheduleAsync(CancellationToken cancellationToken) {
            var schedule = await _service.GetCurrentFikaScheduleAsync(cancellationToken);
            if(schedule == null)
                return NotFound();

            return _mapper.Map<FikaSchedule, FikaScheduleDTO>(schedule);
        }

        [HttpGet("notify-slack")]
        public async Task NotifySlackAboutLatestScheduleAsync(CancellationToken cancellationToken) {
            await _service.NotifySlackAboutLatestScheduleAsync(cancellationToken);
        }

        [HttpGet("latest/{numberOfSchedules:int}")]
        public async Task<ICollection<FikaScheduleDTO>> GetLatestFikaSchedulesAsync(Int32 numberOfSchedules, CancellationToken cancellationToken) {
            var items = await _service.GetLatestFikaSchedulesAsync(numberOfSchedules, cancellationToken);

            return _mapper.Map<ICollection<FikaSchedule>, ICollection<FikaScheduleDTO>>(items);
        }

        [HttpGet("{startDate:datetime}")]
        public async Task<ActionResult<FikaScheduleDTO>> GenerateFikaScheduleForDateAsync(DateTime startDate, CancellationToken cancellationToken) {
            var schedule = await _service.GenerateFikaScheduleForDateAsync(startDate, cancellationToken);
            if(schedule == null)
                return NotFound();

            return _mapper.Map<FikaSchedule, FikaScheduleDTO>(schedule);
        }
    }
}
