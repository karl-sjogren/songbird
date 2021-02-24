using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.Services {
    public class SlackMessagingService : ISlackMessagingService {
        private readonly HttpClient _httpClient;
        private readonly MvcNewtonsoftJsonOptions _mvcJsonOptions;
        private readonly ILogger<FikaScheduleService> _logger;

        public SlackMessagingService(HttpClient httpClient, IOptions<MvcNewtonsoftJsonOptions> mvcJsonOptions, ILogger<FikaScheduleService> logger) {
            _httpClient = httpClient;
            _mvcJsonOptions = mvcJsonOptions.Value;
            _logger = logger;
        }

        public async Task SendMessageAsync(SlackMessage message, CancellationToken cancellationToken) {
            if(string.IsNullOrWhiteSpace(message.Webhook)) {
                return;
            }

            var slackMessage = new {
                blocks = new[] {
                    GetMessageBlock(message)
                }
            };

            var messageJson = JsonConvert.SerializeObject(slackMessage, _mvcJsonOptions.SerializerSettings);

            var request = new HttpRequestMessage(HttpMethod.Post, message.Webhook) {
                Content = new StringContent(messageJson, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();
        }

        private static object GetMessageBlock(SlackMessage message) {
            if(message is SlackMessageWithLinkButton messageWithLinkButton) {
                return new {
                    type = "section",
                    text = new {
                        type = "mrkdwn",
                        text = message.Message
                    },
                    accessory = new {
                        type = "button",
                        text = new {
                            type = "plain_text",
                            text = messageWithLinkButton.ButtonText,
                            emoji = true
                        },
                        value = "button_value",
                        url = messageWithLinkButton.ButtonUrl,
                        action_id = "button-action"
                    }
                };
            }

            return new {
                type = "section",
                text = new {
                    type = "mrkdwn",
                    text = message.Message
                }
            };
        }
    }
}
