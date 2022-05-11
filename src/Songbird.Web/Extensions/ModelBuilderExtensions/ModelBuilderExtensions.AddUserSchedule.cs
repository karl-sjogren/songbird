using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions;

public static partial class ModelBuilderExtensions {
    public static void AddUserSchedule(this ModelBuilder modelBuilder) {
        _ = modelBuilder.Entity<UserSchedule>(entity => {
            entity.AddModelBaseProperties();

            entity
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id);

            entity
                .HasOne(e => e.PlanningBoard)
                .WithMany()
                .HasForeignKey(e => e.PlanningBoardId)
                .HasPrincipalKey(e => e.Id);

            entity
                .HasMany(e => e.Projects)
                .WithOne()
                .HasForeignKey(e => e.ScheduleId)
                .HasPrincipalKey(e => e.Id);

            /*
            entity
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.ScheduleId)
                .HasPrincipalKey(e => e.Id);
                */
        });
    }
}
