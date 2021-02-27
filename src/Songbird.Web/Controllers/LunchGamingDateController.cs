using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.Controllers {
    [ApiController]
    [Route("api/lunch-gaming")]
    public class LunchGamingDateController : Controller {
        private readonly ILunchGamingDateService _service;
        private readonly ILogger<LunchGamingDateController> _logger;

        public LunchGamingDateController(ILunchGamingDateService service, ILogger<LunchGamingDateController> logger) {
            _service = service;
            _logger = logger;
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<LunchGamingDate>> GetUpcomingAsync(CancellationToken cancellationToken) {
            var item = await _service.GetUpcomingAsync(cancellationToken);

            if(item == null) {
                return NotFound();
            }

            return item;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LunchGamingDate>> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
            var item = await _service.GetByIdAsync(id, cancellationToken);

            if(item == null) {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<LunchGamingDate>> CreateAsync([FromBody] LunchGamingDate lunchGamingDate, CancellationToken cancellationToken) {
            var item = await _service.CreateAsync(lunchGamingDate, cancellationToken);

            if(item == null) {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<LunchGamingDate>> UpdateAsync([FromBody] LunchGamingDate lunchGamingDate, CancellationToken cancellationToken) {
            var item = await _service.UpdateAsync(lunchGamingDate, cancellationToken);

            if(item == null) {
                return NotFound();
            }

            return item;
        }

        [HttpPost("{id:guid}/attendees/{userId:guid}")]
        public async Task<ActionResult<LunchGamingDate>> AddAttendeeAsync(Guid id, Guid userId, CancellationToken cancellationToken) {
            var item = await _service.AddAttendeeAsync(id, userId, cancellationToken);

            if(item == null) {
                return NotFound();
            }

            return item;
        }

        [HttpDelete("{id:guid}/attendees/{userId:guid}")]
        public async Task<ActionResult<LunchGamingDate>> RemoveAttendeeAsync(Guid id, Guid userId, CancellationToken cancellationToken) {
            var item = await _service.RemoveAttendeeAsync(id, userId, cancellationToken);

            if(item == null) {
                return NotFound();
            }

            return item;
        }
    }
}
