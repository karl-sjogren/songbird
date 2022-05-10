using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions;

public static partial class ModelBuilderExtensions {
    public static void AddProject(this ModelBuilder modelBuilder) {
        _ = modelBuilder.Entity<Project>(entity => {
            entity.AddModelBaseProperties();

            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.MontlyHours)
                .IsRequired();

            entity.Property(e => e.AccentColor)
                .IsRequired();

            entity.Property(e => e.Timestamp)
                .IsRowVersion();

            entity
                .HasOne(e => e.Customer)
                .WithMany(e => e.Projects)
                .HasForeignKey(e => e.CustomerId)
                .HasPrincipalKey(e => e.Id);

            entity
                .HasMany(e => e.Applications)
                .WithOne(e => e.Project)
                .HasForeignKey(e => e.ProjectId)
                .HasPrincipalKey(e => e.Id);
        });
    }
}
