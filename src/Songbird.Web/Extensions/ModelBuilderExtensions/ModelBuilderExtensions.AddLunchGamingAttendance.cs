using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions {
    public static partial class ModelBuilderExtensions {
        public static void AddLunchGamingAttendance(this ModelBuilder modelBuilder) {
            _ = modelBuilder.Entity<LunchGamingAttendance>(entity => {
                entity.AddModelBaseProperties();

                entity
                    .HasOne(e => e.Date)
                    .WithMany()
                    .HasForeignKey(e => e.DateId)
                    .HasPrincipalKey(e => e.Id);

                entity
                    .HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .HasPrincipalKey(e => e.Id);
            });
        }
    }
}
