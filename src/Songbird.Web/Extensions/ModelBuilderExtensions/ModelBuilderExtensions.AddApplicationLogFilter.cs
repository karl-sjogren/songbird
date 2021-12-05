using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions {
    public static partial class ModelBuilderExtensions {
        public static void AddApplicationLogFilter(this ModelBuilder modelBuilder) {
            _ = modelBuilder.Entity<ApplicationLogFilter>(entity => {
                entity.AddModelBaseProperties();

                entity.Property(e => e.Environment)
                    .IsRequired()
                    .HasConversion<string>();

                entity.Property(e => e.FilterValue)
                    .IsRequired();

                entity.Property(e => e.Timestamp)
                    .IsRowVersion();

                entity
                    .HasOne(e => e.Application)
                    .WithMany(e => e.LogFilters)
                    .HasForeignKey(e => e.ApplicationId)
                    .HasPrincipalKey(e => e.Id);
            });
        }
    }
}
