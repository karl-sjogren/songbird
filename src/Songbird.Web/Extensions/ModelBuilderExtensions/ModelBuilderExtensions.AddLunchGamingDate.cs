using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions {
    public static partial class ModelBuilderExtensions {
        public static void AddLunchGamingDate(this ModelBuilder modelBuilder) {
            _ = modelBuilder.Entity<LunchGamingDate>(entity => {
                entity.AddModelBaseProperties();

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .IsRequired();

                entity
                    .HasMany(e => e.Attendees)
                    .WithOne()
                    .HasForeignKey(e => e.DateId)
                    .HasPrincipalKey(e => e.Id);
            });
        }
    }
}
