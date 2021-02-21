using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions {
    public static partial class ModelBuilderExtensions {
        public static void AddFikaSchedule(this ModelBuilder modelBuilder) {
            _ = modelBuilder.Entity<FikaSchedule>(entity => {
                entity.AddModelBaseProperties();

                entity.Property(e => e.StartDate)
                    .IsRequired();

                entity
                    .HasMany(e => e.Matches)
                    .WithOne();
            });
        }
    }
}
