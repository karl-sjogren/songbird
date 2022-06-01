using System;
using System.Collections.Generic;

namespace Songbird.Web.DataObjects;

public class UserScheduleDTO : DataObjectBase {
    public Guid UserId { get; set; }
    public UserDTO User { get; set; }

    public Guid PlanningBoardId { get; set; }

    public IList<PlannedProjectTimeDTO> Projects { get; set; }
    public IList<ScheduledOfficeRoleDTO> Roles { get; set; }
    public IList<ScheduledStatusDTO> Statuses { get; set; }
}
