using System.Diagnostics.CodeAnalysis;
using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Songbird.Web.Extensions {
    public static class EntityTypeBuilderExtensions {
        public static EntityTypeBuilder<T> AddModelBaseProperties<T>([NotNull] this EntityTypeBuilder<T> builder) where T : ModelBase {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("getdate()")
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("getdate()")
                .IsRequired();

            builder.Property(e => e.IsDeleted)
                .HasDefaultValueSql("0")
                .IsRequired();

            return builder;
        }
    }
}
