using System;

namespace Songbird.Web.DataObjects;

public class PlannedProjectTimeDTO : DataObjectBase {
    public Guid ScheduleId { get; set; }

    public double Time { get; set; }
    public byte[] Timestamp { get; set; }

    public Guid ProjectId { get; set; }
    public ProjectDTO Project { get; set; }
}
