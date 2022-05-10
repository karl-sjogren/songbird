using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.Services;

public class SlackMessagingService : ISlackMessagingService {
    private readonly HttpClient _httpClient;
    private readonly JsonOptions _jsonOptions;
    private readonly ILogger<FikaScheduleService> _logger;

    public SlackMessagingService(HttpClient httpClient, IOptions<JsonOptions> mvcJsonOptions, ILogger<FikaScheduleService> logger) {
        _httpClient = httpClient;
        _jsonOptions = mvcJsonOptions.Value;
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

        var messageJson = JsonSerializer.Serialize(slackMessage, _jsonOptions.JsonSerializerOptions);

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
