using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;

namespace Songbird.Web.Controllers;

[ApiController]
[Route("api/me")]
public class MeController : Controller {
    private readonly IUserService _service;
    private readonly ILogger<MeController> _logger;

    public MeController(IUserService service, ILogger<MeController> logger) {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<object>> GetProfileAsync(CancellationToken cancellationToken) {
        var user = await _service.GetUserAsync((ClaimsIdentity)HttpContext.User.Identity, cancellationToken);

        if(user == null)
            throw new Exception("What the..");

        return new {
            userId = user.Id,
            email = user.Email,
            name = user.Name
        };
    }
}
