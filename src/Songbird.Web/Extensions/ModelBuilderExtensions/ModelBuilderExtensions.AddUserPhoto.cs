using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions;

public static partial class ModelBuilderExtensions {
    public static void AddUserPhoto(this ModelBuilder modelBuilder) {
        _ = modelBuilder.Entity<UserPhoto>(entity => {
            entity.AddModelBaseProperties();

            entity.Property(e => e.ETag)
                .IsRequired();

            entity.Property(e => e.ContentType)
                .IsRequired();

            entity.Property(e => e.Content)
                .IsRequired();

            entity.Property(e => e.UserId)
                .IsRequired();
        });
    }
}
