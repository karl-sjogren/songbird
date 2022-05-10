using System;

namespace Songbird.Web.Models;

public class OfficeRole : ModelBase {
    public string Title { get; set; }
    public Guid? ImageId { get; set; }
    public BinaryFile Image { get; set; }
}
