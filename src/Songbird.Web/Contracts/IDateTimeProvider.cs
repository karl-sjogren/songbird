using System;

namespace Songbird.Web.Contracts {
    public interface IDateTimeProvider {
        DateTime Now { get; }
    }
}
