using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Models;

namespace OffshoreInsights.Persistence.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<ApiCallUsage> ApiCallUsage { get; set; }
    public DbSet<AccountSubscription> AccountSubscriptions { get; set; }
    public DbSet<Buoy> Buoys { get; set; }
    public DbSet<BuoyCurrentPosition> BuoyCurrentPositions { get; set; }
    public DbSet<BuoyPositionHistory> BuoyPositionHistory { get; set; }
    public DbSet<Geofence> Geofences { get; set; }
    public DbSet<GeofenceEvent> GeofenceEvents { get; set; }
    public DbSet<Webhook> Webhooks { get; set; }
    public DbSet<Vessel> Vessels { get; set; }
    public DbSet<VesselCurrentPosition> VesselCurrentPositions { get; set; }
    public DbSet<VesselPositionHistory> VesselPositionHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}