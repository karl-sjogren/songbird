using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions {
    public static partial class ModelBuilderExtensions {
        public static void AddFikaMatch(this ModelBuilder modelBuilder) {
            _ = modelBuilder.Entity<FikaMatch>(entity => {
                entity.AddModelBaseProperties();

                entity
                    .HasMany(e => e.Users)
                    .WithMany(e => e.FikaMatches);
            });
        }
    }
}
