using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class GeofenceConfiguration : ExternalEntityConfiguration<Geofence>
{
    protected override void PostConfigure(EntityTypeBuilder<Geofence> builder)
    {
        builder.ToTable("geofences");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid");
        builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("uuid");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Colour).HasColumnName("colour");
        builder.Property(x => x.ShapeType).HasColumnName("shape_type");
        builder.Property(x => x.Geometry).HasColumnName("geometry").HasColumnType("jsonb");
        builder.Property(x => x.CircleCenter).HasColumnName("circle_center").HasColumnType("jsonb");
        builder.Property(x => x.CircleRadiusKm).HasColumnName("circle_radius_km");
        builder.Property(x => x.VesselTypeFilter).HasColumnName("vessel_type_filter").HasColumnType("text[]");
        builder.Property(x => x.NotificationsEnabled).HasColumnName("notifications_enabled");
        builder.Property(x => x.WatchedBuoyMmsi).HasColumnName("watched_buoy_mmsi");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
    }
}
