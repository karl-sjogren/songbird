using System;
using System.Collections.Generic;

namespace Songbird.Web.Models;

public class Project : ModelBase {
    public Project() {
        Applications = new List<Application>();
    }

    public string Name { get; set; }
    public decimal MontlyHours { get; set; }
    public string AccentColor { get; set; }
    public byte[] Timestamp { get; set; }

    public Guid? IconId { get; set; }
    public BinaryFile Icon { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public ICollection<Application> Applications { get; set; }
}
