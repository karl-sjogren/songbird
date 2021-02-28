using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.Controllers {
    [ApiController]
    public abstract class EditableEntityControllerBase<T> : Controller where T : ModelBase {
        private readonly IEditableEntityServiceBase<T> _service;
        private readonly ILogger _logger;

        protected EditableEntityControllerBase(IEditableEntityServiceBase<T> service, ILogger logger) {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<T>> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
            var item = await _service.GetByIdAsync(id, cancellationToken);

            if(item == null) {
                return NotFound();
            }

            return item;
        }

        [HttpGet]
        public async Task<PagedResult<T>> GetPagedAsync([FromQuery] Int32 pageIndex, [FromQuery] Int32 pageSize, CancellationToken cancellationToken) {
            return await _service.GetPagedAsync(pageIndex, pageSize, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<T>> CreateAsync([FromBody] T model, CancellationToken cancellationToken) {
            var item = await _service.CreateAsync(model, cancellationToken);

            if(item == null) {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<T>> UpdateAsync([FromBody] T model, CancellationToken cancellationToken) {
            var item = await _service.UpdateAsync(model, cancellationToken);

            if(item == null) {
                return NotFound();
            }

            return item;
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> RemoveAttendeeAsync(Guid id, CancellationToken cancellationToken) {
            await _service.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
