using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions {
    public static partial class ModelBuilderExtensions {
        public static void AddApplication(this ModelBuilder modelBuilder) {
            _ = modelBuilder.Entity<Application>(entity => {
                entity.AddModelBaseProperties();

                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(e => e.Timestamp)
                    .IsRowVersion();

                entity
                    .HasOne(e => e.Project)
                    .WithMany(e => e.Applications)
                    .HasForeignKey(e => e.ProjectId)
                    .HasPrincipalKey(e => e.Id);

                entity
                    .HasMany(e => e.LogFilters)
                    .WithOne(e => e.Application)
                    .HasForeignKey(e => e.ApplicationId)
                    .HasPrincipalKey(e => e.Id);
            });
        }
    }
}
