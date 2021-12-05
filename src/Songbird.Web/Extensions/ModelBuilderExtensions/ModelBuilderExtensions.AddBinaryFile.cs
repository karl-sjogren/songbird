using Songbird.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Songbird.Web.Extensions.ModelBuilderExtensions {
    public static partial class ModelBuilderExtensions {
        public static void AddBinaryFile(this ModelBuilder modelBuilder) {
            _ = modelBuilder.Entity<BinaryFile>(entity => {
                entity.AddModelBaseProperties();

                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(e => e.Content)
                    .IsRequired();

                entity.Property(e => e.ContentType)
                    .IsRequired();

                entity.Property(e => e.Checksum)
                    .IsRequired();

                entity.HasIndex(e => e.Checksum);
            });
        }
    }
}
