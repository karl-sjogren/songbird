using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;
using Songbird.Web.ValueConverters;
using System;

namespace Songbird.Web.Extensions.ModelBuilderExtensions;

public static partial class ModelBuilderExtensions {
    public static void AddScheduledStatus(this ModelBuilder modelBuilder) {
        _ = modelBuilder.Entity<ScheduledStatus>(entity => {
            entity.AddModelBaseProperties();

            entity.Property(e => e.ScheduleId)
                .IsRequired();

            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(50)
                .HasConversion<StringToEnumValueConverter<DayOfWeek>>()
                .IsRequired();

            entity.Property(e => e.MorningStatus)
                .HasMaxLength(50)
                .HasConversion<StringToEnumValueConverter<OfficeStatus>>()
                .IsRequired();

            entity.Property(e => e.AfternoonStatus)
                .HasMaxLength(50)
                .HasConversion<StringToEnumValueConverter<OfficeStatus>>()
                .IsRequired();

            entity
                .HasOne(e => e.Schedule)
                .WithMany(e => e.Statuses)
                .HasForeignKey(e => e.ScheduleId)
                .HasPrincipalKey(e => e.Id)
                .IsRequired();
        });
    }
}
