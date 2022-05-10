using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Songbird.Web.Models;

namespace Songbird.Web.Contracts;

public interface IUserService {
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User> GetUserAsync(ClaimsIdentity identity, CancellationToken cancellationToken);
    Task<User> CreateOrUpdateUserAsync(ClaimsIdentity identity, CancellationToken cancellationToken);
    Task UpdateLastLoggedInAsync(Guid userId, CancellationToken cancellationToken);
}
