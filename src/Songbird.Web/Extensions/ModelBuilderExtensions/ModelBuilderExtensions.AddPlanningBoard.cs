using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions;

public static partial class ModelBuilderExtensions {
    public static void AddPlanningBoard(this ModelBuilder modelBuilder) {
        _ = modelBuilder.Entity<PlanningBoard>(entity => {
            entity.AddModelBaseProperties();

            entity.Property(e => e.StartDate)
                .IsRequired();

            entity.Property(e => e.WeekNumber)
                .IsRequired();

            entity.Property(e => e.Year)
                .IsRequired();

            entity
                .HasMany(e => e.UserSchedules)
                .WithOne(e => e.PlanningBoard)
                .HasForeignKey(e => e.PlanningBoardId)
                .HasPrincipalKey(e => e.Id);
        });
    }
}
