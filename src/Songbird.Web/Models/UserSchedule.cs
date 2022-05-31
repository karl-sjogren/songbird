using System;
using System.Collections.Generic;

namespace Songbird.Web.Models;

public class UserSchedule : ModelBase {
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid PlanningBoardId { get; set; }
    public PlanningBoard PlanningBoard { get; set; }

    public IList<PlannedProjectTime> Projects { get; set; } = new List<PlannedProjectTime>();
    public IList<ScheduledOfficeRole> Roles { get; set; } = new List<ScheduledOfficeRole>();
    public IList<ScheduledStatus> Statuses { get; set; } = new List<ScheduledStatus>();
}
