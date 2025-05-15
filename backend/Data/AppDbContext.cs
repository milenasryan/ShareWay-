using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Route = Backend.Models.Route;

namespace Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<RideRequest> RideRequests { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<RideRequest>()
        .HasOne(r => r.Route)
        .WithMany()
        .HasForeignKey(r => r.RouteId);

    modelBuilder.Entity<RideRequest>()
        .HasOne(r => r.User)
        .WithMany()
        .HasForeignKey(r => r.UserId);
}

}
