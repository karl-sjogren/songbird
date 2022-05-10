using System.Threading;
using System.Threading.Tasks;
using Songbird.Web.Models;

namespace Songbird.Web.Contracts;

public interface ISlackMessagingService {
    Task SendMessageAsync(SlackMessage message, CancellationToken cancellationToken);
}
