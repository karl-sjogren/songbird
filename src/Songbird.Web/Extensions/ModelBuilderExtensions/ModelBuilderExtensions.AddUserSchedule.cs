using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions;

public static partial class ModelBuilderExtensions {
    public static void AddUserSchedule(this ModelBuilder modelBuilder) {
        _ = modelBuilder.Entity<UserSchedule>(entity => {
            entity.AddModelBaseProperties();

            entity.Property(e => e.PlanningBoardId)
                .IsRequired();

            entity
                .HasOne(e => e.User)
                .WithMany(e => e.Schedules)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id);

            entity
                .HasOne(e => e.PlanningBoard)
                .WithMany(e => e.UserSchedules)
                .HasForeignKey(e => e.PlanningBoardId)
                .HasPrincipalKey(e => e.Id);

            entity
                .HasMany(e => e.Projects)
                .WithOne(e => e.Schedule)
                .HasForeignKey(e => e.ScheduleId)
                .HasPrincipalKey(e => e.Id);

            entity
                .HasMany(e => e.Roles)
                .WithOne(e => e.Schedule)
                .HasForeignKey(e => e.ScheduleId)
                .HasPrincipalKey(e => e.Id);
        });
    }
}
