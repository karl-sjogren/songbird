using System.Collections.Generic;

namespace Songbird.Web.DataObjects;

public class FikaMatchDTO : DataObjectBase {
    public FikaMatchDTO() {
        Users = new List<UserDTO>();
    }

    public ICollection<UserDTO> Users { get; set; }
}
