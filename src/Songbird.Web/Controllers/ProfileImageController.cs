using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Songbird.Web.Controllers {
    [ApiController]
    [Route("api/profile-image")]
    public class ProfileImageController : Controller {
        private readonly SongbirdContext _songbirdContext;
        private readonly ILogger<ProfileImageController> _logger;

        private readonly byte[] _defaultImageBytes = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x21, 0xF9, 0x04, 0x01, 0x00, 0x00, 0x00, 0x00, 0x2C, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x02, 0x00 };

        public ProfileImageController(SongbirdContext songbirdContext, ILogger<ProfileImageController> logger) {
            _songbirdContext = songbirdContext;
            _logger = logger;
        }

        [HttpGet("{userId:guid}/{name}")]
        [ResponseCache(CacheProfileName = "Image")]
        public async Task<IActionResult> GetProfileAsync(Guid userId, CancellationToken cancellationToken) {
            var userPhoto = await _songbirdContext
                .UserPhotos
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if(userPhoto == null)
                return File(_defaultImageBytes, "image/gif");

            if(Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var etag) && userPhoto.ETag?.Equals(etag) == true) {
                return new StatusCodeResult((Int32)HttpStatusCode.NotModified);
            }

            Response.Headers[HeaderNames.ETag] = userPhoto.ETag;
            return File(userPhoto.Content, userPhoto.ContentType);
        }
    }
}
