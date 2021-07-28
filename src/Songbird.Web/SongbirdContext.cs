using Microsoft.EntityFrameworkCore;
using Songbird.Web.Extensions;
using Songbird.Web.Models;

namespace Songbird.Web {
    public class SongbirdContext : DbContext {
        public SongbirdContext() : this(null) {
        }

        public SongbirdContext(DbContextOptions<SongbirdContext> options)
            : base(options) {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationLogFilter> ApplicationLogFilters { get; set; }
        public virtual DbSet<BinaryFile> BinaryFiles { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<FikaMatch> FikaMatches { get; set; }
        public virtual DbSet<FikaSchedule> FikaSchedules { get; set; }
        public virtual DbSet<LunchGame> LunchGames { get; set; }
        public virtual DbSet<LunchGamingAttendance> LunchGamingAttendance { get; set; }
        public virtual DbSet<LunchGamingDate> LunchGamingDates { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPhoto> UserPhotos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.AddApplication();
            modelBuilder.AddApplicationLogFilter();
            modelBuilder.AddBinaryFile();
            modelBuilder.AddCustomer();
            modelBuilder.AddFikaMatch();
            modelBuilder.AddFikaSchedule();
            modelBuilder.AddLunchGame();
            modelBuilder.AddLunchGamingAttendance();
            modelBuilder.AddLunchGamingDate();
            modelBuilder.AddProject();
            modelBuilder.AddUser();
            modelBuilder.AddUserPhoto();
        }
    }
}
