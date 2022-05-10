using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Songbird.Web.Controllers;

[ApiController]
[Route("api/")]
public class BinaryFileController : Controller {
    private readonly SongbirdContext _songbirdContext;
    private readonly ILogger<BinaryFileController> _logger;

    private readonly byte[] _defaultImageBytes = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x21, 0xF9, 0x04, 0x01, 0x00, 0x00, 0x00, 0x00, 0x2C, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x02, 0x00 };

    public BinaryFileController(SongbirdContext songbirdContext, ILogger<BinaryFileController> logger) {
        _songbirdContext = songbirdContext;
        _logger = logger;
    }

    [HttpGet("file/{id:guid}")]
    public async Task<IActionResult> GetFileAsync(Guid id, CancellationToken cancellationToken) {
        var file = await _songbirdContext
            .BinaryFiles
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return File(file.Content, file.ContentType);
    }

    [HttpGet("image/{id:guid}/{name?}")]
    [ResponseCache(CacheProfileName = "Image")]
    public async Task<IActionResult> GetImageAsync(Guid id, CancellationToken cancellationToken) {
        var file = await _songbirdContext
            .BinaryFiles
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if(file == null)
            return File(_defaultImageBytes, "image/gif");

        if(Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var etag) && file.Checksum?.Equals(etag) == true) {
            return new StatusCodeResult((Int32)HttpStatusCode.NotModified);
        }

        Response.Headers[HeaderNames.ETag] = file.Checksum;
        return File(file.Content, file.ContentType);
    }
}
