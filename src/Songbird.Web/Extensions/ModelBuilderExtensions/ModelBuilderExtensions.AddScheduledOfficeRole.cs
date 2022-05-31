using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;
using Songbird.Web.ValueConverters;

namespace Songbird.Web.Extensions.ModelBuilderExtensions;

public static partial class ModelBuilderExtensions {
    public static void AddScheduledOfficeRole(this ModelBuilder modelBuilder) {
        _ = modelBuilder.Entity<ScheduledOfficeRole>(entity => {
            entity.AddModelBaseProperties();

            entity.Property(e => e.ScheduleId)
                .IsRequired();

            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasConversion<StringToEnumValueConverter<OfficeRole>>()
                .IsRequired();

            entity
                .HasOne(e => e.Schedule)
                .WithMany()
                .HasForeignKey(e => e.ScheduleId)
                .HasPrincipalKey(e => e.Id)
                .IsRequired();
        });
    }
}
