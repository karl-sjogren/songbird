using System;

namespace Songbird.Web.DataObjects;

public class ScheduledOfficeRoleDTO : DataObjectBase {
    public Guid ScheduleId { get; set; }

    public OfficeRoleDTO Role { get; set; }
}
