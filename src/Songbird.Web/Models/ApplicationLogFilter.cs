using System;

namespace Songbird.Web.Models;

public class ApplicationLogFilter : ModelBase {
    public ApplicationLogEnvironment Environment { get; set; }
    public string FilterValue { get; set; }

    public byte[] Timestamp { get; set; }

    public Guid ApplicationId { get; set; }
    public Application Application { get; set; }
}
