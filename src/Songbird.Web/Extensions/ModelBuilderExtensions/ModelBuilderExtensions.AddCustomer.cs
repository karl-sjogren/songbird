using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions {
    public static partial class ModelBuilderExtensions {
        public static void AddCustomer(this ModelBuilder modelBuilder) {
            _ = modelBuilder.Entity<Customer>(entity => {
                entity.AddModelBaseProperties();

                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(e => e.Timestamp)
                    .IsRowVersion();

                entity
                    .HasMany(e => e.Projects)
                    .WithOne()
                    .HasForeignKey(e => e.CustomerId)
                    .HasPrincipalKey(e => e.Id);
            });
        }
    }
}
