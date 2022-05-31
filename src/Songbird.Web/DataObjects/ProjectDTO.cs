using System;

namespace Songbird.Web.DataObjects;

public class ProjectDTO : DataObjectBase {
    public string Name { get; set; }
    public decimal MontlyHours { get; set; }
    public string AccentColor { get; set; }
    public byte[] Timestamp { get; set; }

    public Guid? IconId { get; set; }

    public Guid CustomerId { get; set; }
    public CustomerDTO Customer { get; set; }
}
