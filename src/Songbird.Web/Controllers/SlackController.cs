using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Songbird.Web.Controllers {
    [ApiController]
    [Route("api/slack")]
    public class SlackController : Controller {
        private readonly ILogger<MeController> _logger;

        public SlackController(ILogger<MeController> logger) {
            _logger = logger;
        }

        [HttpPost("events")]
        public async Task<ActionResult<object>> HandleEventsAsync() {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8, true, 1024, true);
            var json = await reader.ReadToEndAsync();

            var urlVerificationEvent = JsonSerializer.Deserialize<SlackUrlVerificationEvent>(json);

            return new {
                challenge = urlVerificationEvent.Challenge
            };
        }

        public class SlackUrlVerificationEvent {
            public string Token { get; set; }
            public string Challenge { get; set; }
            public string Type { get; set; }
        }

        public class SlackEvent {
            public SlackMessageEvent Event { get; set; }
        }

        public class SlackMessageEvent {
            public string Token { get; set; }
            public string Text { get; set; }
        }
    }
}
