using Core.Models;
using Core.Models.RoomModels;
using Core.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppContext : DbContext
{
    public AppContext()
    {
    }

    public AppContext(DbContextOptions<AppContext> options)
        : base(options)
    {
    }

    public DbSet<UserModel> User { get; set; }

    public DbSet<RoomModel> Room { get; set; }

    public DbSet<RoomPointsModel> RoomPoints { get; set; }

    public DbSet<UserRoomModel> UserRoom { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // For migrations creation
        optionsBuilder.UseSqlServer("data source=DESKTOP-DJDF8GF\\SQLEXPRESS01;initial catalog=travelsync;trusted_connection=true");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserModel>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}