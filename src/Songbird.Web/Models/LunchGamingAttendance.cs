using System;

namespace Songbird.Web.Models;

public class LunchGamingAttendance : ModelBase {
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid DateId { get; set; }
    public LunchGamingDate Date { get; set; }
}
