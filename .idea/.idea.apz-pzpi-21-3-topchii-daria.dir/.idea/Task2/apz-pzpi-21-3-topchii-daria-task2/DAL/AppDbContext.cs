using Core.Models;
using Core.Models.RoomModels;
using Core.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> User { get; set; }
        public DbSet<RoomModel> Room { get; set; }
        public DbSet<UserRoomModel> UserRoom { get; set; }
        public DbSet<TrackingDataModel> TrackingData { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "data source=DESKTOP-DJDF8GF\\SQLEXPRESS01;initial catalog=travelsync;trusted_connection=true",
                x => x.UseNetTopologySuite());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserModel>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<RoomModel>()
                .Property(r => r.StartLocation)
                .HasColumnType("geography");
            builder.Entity<TrackingDataModel>()
                .Property(r => r.Location)
                .HasColumnType("geography");
            builder.Entity<TrackingDataModel>()
                .Property(r => r.Timestamp)
                .HasColumnType("datetime");
            builder.Entity<RoomModel>()
                .Property(r => r.EndLocation)
                .HasColumnType("geography");
            builder.Entity<RoomModel>()
                .Property(r => r.DateTime)
                .HasColumnType("datetime");
        }
    }
}