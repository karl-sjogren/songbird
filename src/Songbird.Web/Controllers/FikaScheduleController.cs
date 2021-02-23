using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.Controllers {
    [ApiController]
    [Route("api/fika-schedule")]
    public class FikaScheduleController : Controller {
        private readonly IFikaScheduleService _fikaScheduleService;
        private readonly ILogger<FikaScheduleController> _logger;

        public FikaScheduleController(IFikaScheduleService fikaScheduleService, ILogger<FikaScheduleController> logger) {
            _fikaScheduleService = fikaScheduleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<FikaSchedule>> GetCurrentFikaScheduleAsync(CancellationToken cancellationToken) {
            var schedule = await _fikaScheduleService.GetCurrentFikaScheduleAsync(cancellationToken);
            if(schedule == null)
                return NotFound();

            return schedule;
        }

        [HttpGet("notify-slack")]
        public async Task NotifySlackAboutLatestScheduleAsync(CancellationToken cancellationToken) {
            await _fikaScheduleService.NotifySlackAboutLatestScheduleAsync(cancellationToken);
        }

        [HttpGet("list/{numberOfSchedules:int}")]
        public async Task<ICollection<FikaSchedule>> GetLatestFikaSchedulesAsync(Int32 numberOfSchedules, CancellationToken cancellationToken) {
            return await _fikaScheduleService.GetLatestFikaSchedulesAsync(numberOfSchedules, cancellationToken);
        }

        [HttpGet("{startDate:datetime}")]
        public async Task<ActionResult<FikaSchedule>> GenerateFikaScheduleForDateAsync(DateTime startDate, CancellationToken cancellationToken) {
            var schedule = await _fikaScheduleService.GenerateFikaScheduleForDateAsync(startDate, cancellationToken);
            if(schedule == null)
                return NotFound();

            return schedule;
        }
    }
}
