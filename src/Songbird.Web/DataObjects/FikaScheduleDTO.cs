using System;
using System.Collections.Generic;

namespace Songbird.Web.DataObjects {
    public class FikaScheduleDTO : DataObjectBase {
        public FikaScheduleDTO() {
            Matches = new List<FikaMatchDTO>();
        }

        public DateTime StartDate { get; set; }
        public ICollection<FikaMatchDTO> Matches { get; set; }
    }
}
