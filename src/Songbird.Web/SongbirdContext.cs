using Microsoft.EntityFrameworkCore;
using Songbird.Web.Extensions;
using Songbird.Web.Models;

namespace Songbird.Web {
    public partial class SongbirdContext : DbContext {
        public SongbirdContext() : this(null) {
        }

        public SongbirdContext(DbContextOptions<SongbirdContext> options)
            : base(options) {
        }

        public virtual DbSet<FikaMatch> FikaMatches { get; set; }
        public virtual DbSet<FikaSchedule> FikaSchedules { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPhoto> UserPhotos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.AddFikaMatch();
            modelBuilder.AddFikaSchedule();
            modelBuilder.AddUser();
            modelBuilder.AddUserPhoto();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
