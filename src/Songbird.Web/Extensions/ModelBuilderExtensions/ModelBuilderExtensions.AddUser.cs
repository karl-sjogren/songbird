using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions {
    public static partial class ModelBuilderExtensions {
        public static void AddUser(this ModelBuilder modelBuilder) {
            _ = modelBuilder.Entity<User>(entity => {
                entity.AddModelBaseProperties();

                entity.Property(e => e.ExternalId)
                    .IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired();

                entity.Property(e => e.LastLogin);

                entity.HasIndex(e => e.ExternalId);
            });
        }
    }
}
