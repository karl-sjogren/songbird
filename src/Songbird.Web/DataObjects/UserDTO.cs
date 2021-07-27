using System;

namespace Songbird.Web.DataObjects {
    public class UserDTO : DataObjectBase {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsEligibleForFikaScheduling { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
