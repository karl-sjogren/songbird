using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.Services {
    public class UserService : IUserService {
        private readonly SongbirdContext _songbirdContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<UserService> _logger;

        public UserService(SongbirdContext songbirdContext, IDateTimeProvider dateTimeProvider, ILogger<UserService> logger) {
            _songbirdContext = songbirdContext;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
            return await _songbirdContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        }

        public async Task<User> GetUserAsync(ClaimsIdentity identity, CancellationToken cancellationToken) {
            var externalId = identity.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            return await _songbirdContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ExternalId == externalId && !x.IsDeleted, cancellationToken);
        }

        public async Task<User> CreateOrUpdateUserAsync(ClaimsIdentity identity, CancellationToken cancellationToken) {
            User user = null;

            var name = identity.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty;
            var externalId = identity.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value ?? string.Empty;
            var email = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;

            _logger.LogDebug($"Trying to create or update user with the following values: {name}, {externalId}, {email}.");

            if(externalId != null) {
                user = await _songbirdContext
                    .Users
                    .FirstOrDefaultAsync(x => x.ExternalId == externalId, cancellationToken);

                if(user != null)
                    _logger.LogInformation($"Matched user on externalId {externalId}.");
            }

            if(user == null && email != null) {
                user = await _songbirdContext
                    .Users
                    .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

                if(user != null)
                    _logger.LogInformation($"Matched user on email {email}.");
            }

            if(user == null) {
                user = new User {
                    CreatedAt = _dateTimeProvider.Now
                };
                _logger.LogInformation($"Creating new user for externalId {externalId} with externalId {externalId}.");
            }

            user.Name = name;
            user.Email = email;
            user.ExternalId = externalId;
            user.IsEligibleForFikaScheduling = false;
            user.UpdatedAt = _dateTimeProvider.Now;

            if(user.Id == Guid.Empty) {
                await _songbirdContext.Users.AddAsync(user, cancellationToken);
            }

            await _songbirdContext.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task UpdateLastLoggedInAsync(Guid userId, CancellationToken cancellationToken) {
            var user = await _songbirdContext
                .Users
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if(user == null)
                return;

            user.LastLogin = _dateTimeProvider.Now;

            await _songbirdContext.SaveChangesAsync(cancellationToken);
        }
    }
}
