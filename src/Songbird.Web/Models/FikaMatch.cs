using System.Collections.Generic;

namespace Songbird.Web.Models;

public class FikaMatch : ModelBase {
    public FikaMatch() {
        Users = new List<User>();
    }

    public ICollection<User> Users { get; set; }
}
