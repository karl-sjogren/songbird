using System.Linq;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.Services {
    public class LunchGameService : EditableEntityServiceBase<LunchGame>, ILunchGameService {
        private readonly SongbirdContext _songbirdContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<LunchGameService> _logger;

        public LunchGameService(SongbirdContext songbirdContext, IDateTimeProvider dateTimeProvider, ILogger<LunchGameService> logger)
            : base(songbirdContext, dateTimeProvider, logger) {
            _songbirdContext = songbirdContext;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        protected override IQueryable<LunchGame> GetPreparedQuery() {
            return _songbirdContext
                .LunchGames;
        }

        protected override void MapChangesToModel(LunchGame existingItem, LunchGame model) {
            existingItem.Name = model.Name;
            existingItem.IconId = model.IconId;
        }
    }
}
