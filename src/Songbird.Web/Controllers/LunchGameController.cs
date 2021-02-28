using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.Controllers {
    [ApiController]
    [Route("api/lunch-game")]
    public class LunchGameController : EditableEntityControllerBase<LunchGame> {
        public LunchGameController(ILunchGameService service, ILogger<LunchGameController> logger)
            : base(service, logger) {
        }
    }
}
