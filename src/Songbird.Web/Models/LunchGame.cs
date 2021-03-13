using System;

namespace Songbird.Web.Models {
    public class LunchGame : ModelBase {
        public string Name { get; set; }
        public Guid? IconId { get; set; }
        public BinaryFile Icon { get; set; }
    }
}
