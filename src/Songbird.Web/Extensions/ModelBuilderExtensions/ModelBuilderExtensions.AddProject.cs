using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions {
    public static partial class ModelBuilderExtensions {
        public static void AddProject(this ModelBuilder modelBuilder) {
            _ = modelBuilder.Entity<Project>(entity => {
                entity.AddModelBaseProperties();

                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(e => e.MontlyHours)
                    .IsRequired();

                entity.Property(e => e.HexColor)
                    .IsRequired();

                entity.Property(e => e.Timestamp)
                    .IsRowVersion();

                entity
                    .HasOne(e => e.Customer)
                    .WithMany()
                    .HasForeignKey(e => e.CustomerId)
                    .HasPrincipalKey(e => e.Id);
            });
        }
    }
}
