using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions;

public static partial class ModelBuilderExtensions {
    public static void AddLunchGame(this ModelBuilder modelBuilder) {
        _ = modelBuilder.Entity<LunchGame>(entity => {
            entity.AddModelBaseProperties();

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity
                .HasOne(e => e.Icon)
                .WithMany()
                .HasForeignKey(e => e.IconId)
                .HasPrincipalKey(e => e.Id);
        });
    }
}
