using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.Services {
    public class LunchGamingDateService : ILunchGamingDateService {
        private readonly SongbirdContext _songbirdContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<LunchGamingDateService> _logger;

        public LunchGamingDateService(SongbirdContext songbirdContext, IDateTimeProvider dateTimeProvider, ILogger<LunchGamingDateService> logger) {
            _songbirdContext = songbirdContext;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task<LunchGamingDate> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
            return await _songbirdContext
                .LunchGamingDates
                .Include(x => x.Attendees).ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        }

        public async Task<LunchGamingDate> GetUpcomingAsync(CancellationToken cancellationToken) {
            return await _songbirdContext
                .LunchGamingDates
                .Include(x => x.Attendees).ThenInclude(x => x.User)
                .Where(x => !x.IsDeleted && x.Date >= _dateTimeProvider.Now.Date)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<LunchGamingDate> CreateAsync(LunchGamingDate model, CancellationToken cancellationToken) {
            if(model.Id != Guid.Empty) {
                return null;
            }

            model.CreatedAt = model.UpdatedAt = _dateTimeProvider.Now;

            _songbirdContext.Add(model);
            await _songbirdContext.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task<LunchGamingDate> UpdateAsync(LunchGamingDate model, CancellationToken cancellationToken) {
            var existingItem = await GetByIdAsync(model.Id, cancellationToken);
            if(existingItem == null) {
                return null;
            }

            existingItem.Date = model.Date;

            if(_songbirdContext.Entry(existingItem).State == EntityState.Modified) {
                existingItem.UpdatedAt = _dateTimeProvider.Now;
            }

            await _songbirdContext.SaveChangesAsync(cancellationToken);

            return existingItem;
        }

        public async Task<LunchGamingDate> AddAttendeeAsync(Guid id, Guid userId, CancellationToken cancellationToken) {
            var existingItem = await GetByIdAsync(id, cancellationToken);
            if(existingItem == null) {
                return null;
            }

            if(existingItem.Attendees.Any(x => x.UserId == userId)) {
                return existingItem;
            }

            var attendee = new LunchGamingAttendance() {
                DateId = existingItem.Id,
                UserId = userId
            };

            attendee.CreatedAt = attendee.UpdatedAt = _dateTimeProvider.Now;

            existingItem.Attendees.Add(attendee);

            await _songbirdContext.SaveChangesAsync(cancellationToken);

            return existingItem;
        }

        public async Task<LunchGamingDate> RemoveAttendeeAsync(Guid id, Guid userId, CancellationToken cancellationToken) {
            var existingItem = await GetByIdAsync(id, cancellationToken);
            if(existingItem == null) {
                return null;
            }

            var existingAttendee = existingItem.Attendees.FirstOrDefault(x => x.UserId == userId);
            if(existingAttendee == null) {
                return existingItem;
            }

            existingItem.Attendees.Remove(existingAttendee);

            await _songbirdContext.SaveChangesAsync(cancellationToken);

            return existingItem;
        }
    }
}
