using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions;

public static partial class ModelBuilderExtensions {
    public static void AddUser(this ModelBuilder modelBuilder) {
        _ = modelBuilder.Entity<User>(entity => {
            entity.AddModelBaseProperties();

            entity.Property(e => e.ExternalId)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.IsEligibleForFikaScheduling)
                .HasDefaultValue(false)
                .IsRequired();

            entity.Property(e => e.IsEligibleForWeeklyPlaning)
                .HasDefaultValue(false)
                .IsRequired();

            entity.Property(e => e.LastLogin);

            entity.HasIndex(e => e.ExternalId);
        });
    }
}
