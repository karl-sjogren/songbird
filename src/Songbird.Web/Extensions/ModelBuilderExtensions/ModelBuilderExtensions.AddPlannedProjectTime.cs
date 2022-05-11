using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions;

public static partial class ModelBuilderExtensions {
    public static void AddPlannedProjectTime(this ModelBuilder modelBuilder) {
        _ = modelBuilder.Entity<PlannedProjectTime>(entity => {
            entity.AddModelBaseProperties();

            entity.Property(e => e.Time)
                .HasColumnType("decimal(5, 2)")
                .IsRequired();

            entity.Property(e => e.Timestamp)
                .IsRowVersion();

            entity
                .HasOne(e => e.Schedule)
                .WithMany()
                .HasForeignKey(e => e.ScheduleId)
                .HasPrincipalKey(e => e.Id);

            entity
                .HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(e => e.ProjectId)
                .HasPrincipalKey(e => e.Id);
        });
    }
}
