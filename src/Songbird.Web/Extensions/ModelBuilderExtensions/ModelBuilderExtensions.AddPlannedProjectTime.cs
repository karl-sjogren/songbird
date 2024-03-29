using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions;

public static partial class ModelBuilderExtensions {
    public static void AddPlannedProjectTime(this ModelBuilder modelBuilder) {
        _ = modelBuilder.Entity<PlannedProjectTime>(entity => {
            entity.AddModelBaseProperties();

            entity.Property(e => e.Time)
                .IsRequired();

            entity.Property(e => e.Timestamp)
                .IsRowVersion();

            entity.Property(e => e.ScheduleId)
                .IsRequired();

            entity.Property(e => e.ProjectId)
                .IsRequired();

            entity
                .HasOne(e => e.Schedule)
                .WithMany()
                .HasForeignKey(e => e.ScheduleId)
                .HasPrincipalKey(e => e.Id)
                .IsRequired();

            entity
                .HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(e => e.ProjectId)
                .HasPrincipalKey(e => e.Id)
                .IsRequired();
        });
    }
}
