using System;

namespace Songbird.Web.Models {
    public class UserPhoto : ModelBase {
        public string ETag { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public Guid UserId { get; set; }
    }
}
