using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;

namespace Songbird.Web.Controllers {
    [ApiController]
    [Route("api/me")]
    public class MeController : Controller {
        private readonly IUserService _userService;
        private readonly ILogger<MeController> _logger;

        public MeController(IUserService userService, ILogger<MeController> logger) {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetProfileAsync(CancellationToken cancellationToken) {
            var user = await _userService.GetUserAsync((ClaimsIdentity)HttpContext.User.Identity, cancellationToken);

            if(user == null)
                throw new Exception("What the..");

            return new {
                userId = user.Id,
                email = user.Email,
                name = user.Name
            };
        }
    }
}
