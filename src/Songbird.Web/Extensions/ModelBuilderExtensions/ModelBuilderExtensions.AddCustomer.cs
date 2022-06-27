using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions;

public static partial class ModelBuilderExtensions {
    public static void AddCustomer(this ModelBuilder modelBuilder) {
        _ = modelBuilder.Entity<Customer>(entity => {
            entity.AddModelBaseProperties();

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Timestamp)
                .IsRowVersion();

            entity
                .HasMany(e => e.Projects)
                .WithOne(e => e.Customer)
                .HasForeignKey(e => e.CustomerId)
                .HasPrincipalKey(e => e.Id);
        });
    }
}
