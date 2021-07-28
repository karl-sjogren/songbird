using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.Services {
    public class ApplicationService : EditableEntityServiceBase<Application>, IApplicationService {
        private readonly SongbirdContext _songbirdContext;
        private readonly ILogger<LunchGameService> _logger;

        public ApplicationService(SongbirdContext songbirdContext, IDateTimeProvider dateTimeProvider, ILogger<LunchGameService> logger)
            : base(songbirdContext, dateTimeProvider, logger) {
            _songbirdContext = songbirdContext;
            _logger = logger;
        }

        protected override IQueryable<Application> GetPreparedQuery() {
            return _songbirdContext
                .Applications
                .Include(x => x.LogFilters)
                .Include(x => x.Project).ThenInclude(x => x.Customer);
        }

        protected override void MapChangesToModel(Application existingItem, Application model) {
            existingItem.Name = model.Name;
        }
    }
}
